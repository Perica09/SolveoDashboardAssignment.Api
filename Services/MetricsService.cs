using Microsoft.EntityFrameworkCore;
using SolveoDashboardAssignment.Api.Data;
using SolveoDashboardAssignment.Api.Dtos;
using SolveoDashboardAssignment.Api.Entities;
using SolveoDashboardAssignment.Api.Interfaces;

namespace SolveoDashboardAssignment.Api.Services;

/// <summary>
/// Service for retrieving and analyzing dashboard metrics
/// </summary>
public class MetricsService : IMetricsService
{
    private readonly AppDbContext _context;
    private readonly ILogger<MetricsService> _logger;

    public MetricsService(AppDbContext context, ILogger<MetricsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Monthly Metrics Methods

    public async Task<MonthlyMetricsDto?> GetLatestMonthlyMetricsAsync()
    {
        try
        {
            var latestMonth = await _context.MonthlyMetrics
                .OrderByDescending(m => m.Month)
                .FirstOrDefaultAsync();

            if (latestMonth == null)
                return null;

            var previousMonth = await _context.MonthlyMetrics
                .Where(m => m.Month < latestMonth.Month)
                .OrderByDescending(m => m.Month)
                .FirstOrDefaultAsync();

            var growthPercentage = previousMonth != null && previousMonth.MrrUsd != 0
                ? (latestMonth.MrrUsd - previousMonth.MrrUsd) / previousMonth.MrrUsd * 100
                : 0;

            return new MonthlyMetricsDto
            {
                LatestMrr = latestMonth.MrrUsd,
                GrowthPercentageMoM = growthPercentage,
                SignupToTrialPercentage = latestMonth.SignupToTrialRate,
                TrialToPaidPercentage = latestMonth.TrialToPaidRate,
                Month = latestMonth.Month.ToString("yyyy-MM"),
                PreviousMonthMrr = previousMonth?.MrrUsd,
                WebsiteTraffic = latestMonth.WebsiteTraffic,
                UniqueSignups = latestMonth.UniqueSignups,
                TrialsStarted = latestMonth.TrialsStarted,
                PaidConversions = latestMonth.PaidConversions,
                ChurnRate = latestMonth.ChurnRate
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting latest monthly metrics");
            throw;
        }
    }

    public async Task<List<MonthlyMetricsDto>> GetMonthlyMetricsRangeAsync(
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        try
        {
            var query = _context.MonthlyMetrics.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(m => m.Month >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(m => m.Month <= endDate.Value);

            var monthlyData = await query
                .OrderBy(m => m.Month)
                .ToListAsync();

            var result = new List<MonthlyMetricsDto>();

            for (int i = 0; i < monthlyData.Count; i++)
            {
                var current = monthlyData[i];
                var previous = i > 0 ? monthlyData[i - 1] : null;

                var growthPercentage = previous != null && previous.MrrUsd != 0
                    ? (current.MrrUsd - previous.MrrUsd) / previous.MrrUsd * 100
                    : 0;

                result.Add(new MonthlyMetricsDto
                {
                    LatestMrr = current.MrrUsd,
                    GrowthPercentageMoM = growthPercentage,
                    SignupToTrialPercentage = current.SignupToTrialRate,
                    TrialToPaidPercentage = current.TrialToPaidRate,
                    Month = current.Month.ToString("yyyy-MM"),
                    PreviousMonthMrr = previous?.MrrUsd,
                    WebsiteTraffic = current.WebsiteTraffic,
                    UniqueSignups = current.UniqueSignups,
                    TrialsStarted = current.TrialsStarted,
                    PaidConversions = current.PaidConversions,
                    ChurnRate = current.ChurnRate
                });
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting monthly metrics range");
            throw;
        }
    }

    public async Task<List<MonthlyMrrDto>> GetMrrHistoryAsync(int months = 12)
    {
        try
        {
            var recentMonths = await _context.MonthlyMetrics
                .OrderByDescending(m => m.Month)   // get latest first
                .Take(months)
                .OrderBy(m => m.Month)            // reorder ascending for chart
                .ToListAsync();

            return recentMonths.Select(m => new MonthlyMrrDto
            {
                Month = m.Month.Month,            // numeric month 1-12
                Year = m.Month.Year,
                MrrUsd = m.MrrUsd
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching MRR history");
            throw;
        }
    }


    #endregion

    #region Regional Performance Methods

    public async Task<List<RegionalPerformance>> GetAllRegionalDataAsync(
    DateTime? startDate = null,
    DateTime? endDate = null,
    List<string>? regions = null,
    List<string>? countries = null,
    List<string>? cities = null)
{
    var query = _context.RegionalPerformances.AsQueryable();

    if (startDate.HasValue)
        query = query.Where(r => r.Month >= startDate.Value);

    if (endDate.HasValue)
        query = query.Where(r => r.Month <= endDate.Value);

    if (regions != null && regions.Any())
        query = query.Where(r => regions.Contains(r.Region));

    if (countries != null && countries.Any())
        query = query.Where(r => countries.Contains(r.Country));

    if (cities != null && cities.Any())
        query = query.Where(r => cities.Contains(r.City));

    return await query.OrderBy(r => r.Region)
                      .ThenBy(r => r.Country)
                      .ThenBy(r => r.City)
                      .ThenBy(r => r.Month)
                      .ToListAsync();
}


    public async Task<List<RegionalMetricsDto>> GetRegionalMetricsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        List<string>? regions = null,
        List<string>? countries = null,
        List<string>? cities = null)
    {
        try
        {
            var query = _context.RegionalPerformances.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(r => r.Month >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.Month <= endDate.Value);

            if (regions != null && regions.Any())
                query = query.Where(r => regions.Contains(r.Region));
                
             if (countries != null && countries.Any())
                query = query.Where(r => countries.Contains(r.Country));

            if (cities != null && cities.Any())
                query = query.Where(r => cities.Contains(r.City));

            var regionalData = await query.ToListAsync();

            var groupedData = regionalData
                .GroupBy(r => new { r.Region, r.Country, r.City })
                .Select(g =>
                {
                    var orderedData = g.OrderBy(r => r.Month).ToList();
                    var midPoint = orderedData.Count / 2;
                    var firstHalf = orderedData.Take(midPoint).ToList();
                    var secondHalf = orderedData.Skip(midPoint).ToList();

                    var firstHalfAvgTraffic = firstHalf.Any() ? firstHalf.Average(r => r.TotalTraffic) : 0;
                    var secondHalfAvgTraffic = secondHalf.Any() ? secondHalf.Average(r => r.TotalTraffic) : 0;

                    var trafficTrend = firstHalfAvgTraffic != 0
                        ? (secondHalfAvgTraffic - firstHalfAvgTraffic) / firstHalfAvgTraffic * 100
                        : 0;

                    var avgCac = g.Average(r => r.CacUsd);
                    var avgLtv = g.Average(r => r.LtvUsd);
                    var cacLtvRatio = avgLtv != 0 ? avgCac / avgLtv : 0;

                    return new RegionalMetricsDto
                    {
                        Region = g.Key.Region,
                        Country = g.Key.Country,
                        City = g.Key.City,
                        AverageTrialToPaidRate = Math.Round(g.Average(r => r.TrialToPaidRate), 2),
                        TrafficTrendPercentage = Math.Round((decimal)trafficTrend, 2),
                        CacLtvRatio = Math.Round(cacLtvRatio, 2),
                        TotalTraffic = g.Sum(r => r.TotalTraffic),
                        TotalConversions = g.Sum(r => r.PaidConversions),
                        AverageCac = Math.Round(avgCac, 2),
                        AverageLtv = Math.Round(avgLtv, 2),
                        MonthCount = g.Count()
                    };
                })
                .ToList();

            return groupedData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting regional metrics");
            throw;
        }
    }

    public async Task<Dictionary<string, decimal>> GetAverageTrialToPaidByRegionAsync(
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        try
        {
            var query = _context.RegionalPerformances.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(r => r.Month >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.Month <= endDate.Value);

            var result = await query
                .GroupBy(r => r.Region)
                .Select(g => new
                {
                    Region = g.Key,
                    AverageRate = Math.Round(g.Average(r => r.TrialToPaidRate), 2)
                })
                .ToDictionaryAsync(x => x.Region, x => x.AverageRate);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting average trial to paid by region");
            throw;
        }
    }

    public async Task<Dictionary<string, decimal>> GetTrafficTrendByRegionAsync(
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        try
        {
            var query = _context.RegionalPerformances.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(r => r.Month >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.Month <= endDate.Value);

            var regionalData = await query.ToListAsync();

            var result = regionalData
                .GroupBy(r => r.Region)
                .Select(g =>
                {
                    var orderedData = g.OrderBy(r => r.Month).ToList();
                    var midPoint = orderedData.Count / 2;
                    var firstHalf = orderedData.Take(midPoint).ToList();
                    var secondHalf = orderedData.Skip(midPoint).ToList();

                    var firstHalfAvg = firstHalf.Any() ? firstHalf.Average(r => r.TotalTraffic) : 0;
                    var secondHalfAvg = secondHalf.Any() ? secondHalf.Average(r => r.TotalTraffic) : 0;

                    var trend = firstHalfAvg != 0
                        ? Math.Round((decimal)((secondHalfAvg - firstHalfAvg) / firstHalfAvg * 100), 2)
                        : 0m;

                    return new { Region = g.Key, Trend = trend };
                })
                .ToDictionary(x => x.Region, x => x.Trend);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting traffic trend by region");
            throw;
        }
    }

    public async Task<Dictionary<string, decimal>> GetCacLtvRatioByRegionAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        List<string>? regions = null)
    {
        try
        {
            var query = _context.RegionalPerformances.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(r => r.Month >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.Month <= endDate.Value);

            if (regions != null && regions.Any())
                query = query.Where(r => regions.Contains(r.Region));

            var result = await query
                .GroupBy(r => r.Region)
                .Select(g => new
                {
                    Region = g.Key,
                    AvgCac = g.Average(r => r.CacUsd),
                    AvgLtv = g.Average(r => r.LtvUsd)
                })
                .ToDictionaryAsync(
                    x => x.Region,
                    x => x.AvgLtv != 0 ? Math.Round(x.AvgCac / x.AvgLtv, 2) : 0m);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting CAC/LTV ratio by region");
            throw;
        }
    }

    #endregion

    #region Channel Performance Methods

    public async Task<List<ChannelMonthlyDto>> GetAllChannelPerformanceAsync(
    DateTime? startDate = null,
    DateTime? endDate = null,
    List<string>? channels = null)
{
    try
    {
        var query = _context.ChannelPerformances.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(c => c.Month >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.Month <= endDate.Value);

        if (channels != null && channels.Any())
            query = query.Where(c => channels.Contains(c.Channel));

        var data = await query
            .OrderBy(c => c.Month)
            .Select(c => new ChannelMonthlyDto
            {
                Month = c.Month.ToString("yyyy-MM"), // 👈 FORMAT HERE
                Channel = c.Channel,
                Sessions = c.Sessions,
                Signups = c.Signups,
                ConversionRate = c.ConversionRate,
                AvgSessionDurationSec = c.AvgSessionDurationSec,
                BounceRate = c.BounceRate,
                PagesPerSession = c.PagesPerSession
            })
            .ToListAsync();

        return data;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting all channel performance data");
        throw;
    }
}

    public async Task<List<ChannelMetricsDto>> GetChannelMetricsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        List<string>? channels = null)
    {
        try
        {
            var query = _context.ChannelPerformances.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.Month >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.Month <= endDate.Value);

            if (channels != null && channels.Any())
                query = query.Where(c => channels.Contains(c.Channel));

            var channelData = await query.ToListAsync();

            var result = channelData
                .GroupBy(c => c.Channel)
                .Select(g =>
                {
                    var totalSessions = g.Sum(c => c.Sessions);
                    var totalSignups = g.Sum(c => c.Signups);
                    var conversionRate = totalSessions > 0
                        ? (decimal)totalSignups / totalSessions * 100
                        : 0;

                    return new ChannelMetricsDto
                    {
                        Channel = g.Key,
                        ConversionRate = Math.Round(conversionRate, 2),
                        TotalSessions = totalSessions,
                        TotalSignups = totalSignups,
                        AverageSessionDuration = (int)g.Average(c => c.AvgSessionDurationSec),
                        BounceRate = g.Average(c => c.BounceRate),
                        PagesPerSession = Math.Round(g.Average(c => c.PagesPerSession), 2),
                        MonthCount = g.Count()
                    };
                })
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel metrics");
            throw;
        }
    }

    public async Task<Dictionary<string, decimal>> GetConversionRateByChannelAsync(
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        try
        {
            var query = _context.ChannelPerformances.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.Month >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.Month <= endDate.Value);

            var channelData = await query.ToListAsync();

            var result = channelData
                .GroupBy(c => c.Channel)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var totalSessions = g.Sum(c => c.Sessions);
                        var totalSignups = g.Sum(c => c.Signups);
                        return totalSessions > 0
                            ? Math.Round((decimal)totalSignups / totalSessions * 100, 2)
                            : 0m;
                    });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting conversion rate by channel");
            throw;
        }
    }

    #endregion

    #region Keyword Performance Methods

    public async Task<List<KeywordMetricsDto>> GetKeywordMetricsAsync(
        List<string>? categories = null,
        int? minTraffic = null,
        int? maxTraffic = null)
    {
        try
        {
            var query = _context.KeywordPerformances.AsQueryable();

            if (categories != null && categories.Any())
                query = query.Where(k => categories.Contains(k.Category));

            if (minTraffic.HasValue)
                query = query.Where(k => k.Traffic2025 >= minTraffic.Value);

            if (maxTraffic.HasValue)
                query = query.Where(k => k.Traffic2025 <= maxTraffic.Value);

            var keywords = await query.ToListAsync();

            var result = keywords.Select(k => new KeywordMetricsDto
            {
                Keyword = k.Keyword,
                Category = k.Category,
                TrafficChangeYoY = k.TrafficChangePct,
                Traffic2024 = k.Traffic2024,
                Traffic2025 = k.Traffic2025,
                ConversionRate2024 = k.ConversionRate2024,
                ConversionRate2025 = k.ConversionRate2025,
                Position2024 = k.Position2024,
                Position2025 = k.Position2025,
                PositionChange = k.PositionChange,
                AiOverviewTriggered = k.AiOverviewTriggered ? "Yes" : "No"
            }).ToList();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting keyword metrics");
            throw;
        }
    }

    public async Task<List<KeywordMetricsDto>> GetTrafficChangeYoYAsync(
        decimal? minChangePercentage = null,
        List<string>? categories = null)
    {
        try
        {
            var query = _context.KeywordPerformances.AsQueryable();

            if (categories != null && categories.Any())
                query = query.Where(k => categories.Contains(k.Category));

            var keywords = await query.ToListAsync();

            var result = keywords
                .Where(k => !minChangePercentage.HasValue || k.TrafficChangePct >= minChangePercentage.Value)
                .OrderByDescending(k => k.TrafficChangePct)
                .Select(k => new KeywordMetricsDto
                {
                    Keyword = k.Keyword,
                    Category = k.Category,
                    TrafficChangeYoY = k.TrafficChangePct,
                    Traffic2024 = k.Traffic2024,
                    Traffic2025 = k.Traffic2025,
                    ConversionRate2024 = k.ConversionRate2024,
                    ConversionRate2025 = k.ConversionRate2025,
                    Position2024 = k.Position2024,
                    Position2025 = k.Position2025,
                    PositionChange = k.PositionChange,
                    AiOverviewTriggered = k.AiOverviewTriggered ? "Yes" : "No"
                })
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting traffic change YoY");
            throw;
        }
    }

    #endregion
}
