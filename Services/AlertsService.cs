using Microsoft.EntityFrameworkCore;
using SolveoDashboardAssignment.Api.Data;
using SolveoDashboardAssignment.Api.Dtos;
using SolveoDashboardAssignment.Api.Enums;
using SolveoDashboardAssignment.Api.Interfaces;

namespace SolveoDashboardAssignment.Api.Services;

/// <summary>
/// Service for detecting and managing performance alerts
/// </summary>
public class AlertsService : IAlertService
{
    private readonly AppDbContext _context;
    private readonly IMetricsService _metricsService;
    private readonly ILogger<AlertsService> _logger;

    public AlertsService(
        AppDbContext context,
        IMetricsService metricsService,
        ILogger<AlertsService> logger)
    {
        _context = context;
        _metricsService = metricsService;
        _logger = logger;
    }

    public async Task<List<AlertDto>> GetAllAlertsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        // Convert to UTC to avoid PostgreSQL timestamptz errors
        DateTime? startUtc = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : null;

        DateTime? endUtc = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : null;
        
        try
        {
            var allAlerts = new List<AlertDto>();

            // Collect all alerts from different detection methods with error isolation
            try
            {
                var highTrafficLowConversion = await DetectHighTrafficLowConversionKeywordsAsync();
                allAlerts.AddRange(highTrafficLowConversion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting high traffic low conversion alerts, skipping");
            }

            try
            {
                var aiOverviewCannibalization = await DetectAiOverviewCannibalizationAsync();
                allAlerts.AddRange(aiOverviewCannibalization);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting AI overview cannibalization alerts, skipping");
            }

            try
            {
                var regionalUnderperformance = await DetectRegionalUnderperformanceAsync(startUtc, endUtc);
                allAlerts.AddRange(regionalUnderperformance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting regional underperformance alerts, skipping");
            }

            try
            {
                var seasonalDips = await DetectSeasonalDipsAsync(startUtc, endUtc);
                allAlerts.AddRange(seasonalDips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting seasonal dips alerts, skipping");
            }

            try
            {
                var channelWaste = await DetectChannelWasteAsync(startDate: startUtc, endDate: endUtc);
                allAlerts.AddRange(channelWaste);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting channel waste alerts, skipping");
            }

            // Sort by severity (Critical > High > Medium > Low)
            var severityOrder = new Dictionary<string, int>
            {
                { AlertSeverity.Critical.ToString(), 0 },
                { AlertSeverity.High.ToString(), 1 },
                { AlertSeverity.Medium.ToString(), 2 },
                { AlertSeverity.Low.ToString(), 3 }
            };

            return allAlerts
                .OrderBy(a => severityOrder.ContainsKey(a.Severity) ? severityOrder[a.Severity] : 4)
                .ThenByDescending(a => a.Value)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all alerts");
            throw;
        }
    }

    public async Task<List<AlertDto>> DetectHighTrafficLowConversionKeywordsAsync(
        int minTraffic = 2000,
        decimal maxConversion = 1.5m)
    {
        try
        {
            var alerts = new List<AlertDto>();

            var keywords = await _context.KeywordPerformances
                .Where(k => k.Traffic2025 >= minTraffic
                    && k.ConversionRate2025 < maxConversion)
                .ToListAsync();

            foreach (var keyword in keywords)
            {
                var severity = keyword.Traffic2025 > 10000
                    ? AlertSeverity.High
                    : keyword.Traffic2025 > 5000
                        ? AlertSeverity.Medium
                        : AlertSeverity.Low;

                alerts.Add(new AlertDto
                {
                    AlertType = Enums.AlertType.HighTrafficLowConversion.ToString(),
                    Severity = severity.ToString(),
                    Message = $"Keyword '{keyword.Keyword}' has {keyword.Traffic2025} visits but only {keyword.ConversionRate2025:F2}% conversion rate",
                    Entity = keyword.Keyword,
                    Value = keyword.ConversionRate2025,
                    Threshold = maxConversion,
                    RecommendedAction = "Review landing page content and optimize for conversion. Consider creating targeted trial offer for educational content visitors.",
                    DetectedAt = DateTime.UtcNow
                });
            }

            return alerts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting high traffic low conversion keywords");
            throw;
        }
    }

    public async Task<List<AlertDto>> DetectAiOverviewCannibalizationAsync(
        decimal minDeclinePercentage = 10.0m)
    {
        try
        {
            var alerts = new List<AlertDto>();

            var keywords = await _context.KeywordPerformances
                .Where(k => k.AiOverviewTriggered
                    && k.TrafficChangePct < -minDeclinePercentage)
                .ToListAsync();

            foreach (var keyword in keywords)
            {
                var declineAbs = Math.Abs(keyword.TrafficChangePct);
                var severity = declineAbs > 25
                    ? AlertSeverity.High
                    : AlertSeverity.Medium;

                alerts.Add(new AlertDto
                {
                    AlertType = Enums.AlertType.AiOverviewCannibalization.ToString(),
                    Severity = severity.ToString(),
                    Message = $"Keyword '{keyword.Keyword}' with AI Overview triggered shows {keyword.TrafficChangePct:F2}% YoY traffic decline",
                    Entity = keyword.Keyword,
                    Value = keyword.TrafficChangePct,
                    Threshold = -minDeclinePercentage,
                    RecommendedAction = "Consider creating more comprehensive content that provides value beyond AI Overview snippets. Focus on unique insights and practical examples.",
                    DetectedAt = DateTime.UtcNow
                });
            }

            return alerts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting AI Overview cannibalization");
            throw;
        }
    }

 public async Task<List<AlertDto>> DetectRegionalUnderperformanceAsync(
    DateTime? startDate = null,
    DateTime? endDate = null)
{
    var alerts = new List<AlertDto>();
    var regionalMetrics = await _metricsService.GetRegionalMetricsAsync(startDate, endDate);

    if (regionalMetrics.Count == 0)
        return alerts;

    // Aggregate metrics per region
    var aggregated = regionalMetrics
        .GroupBy(r => r.Region)
        .Select(g => new
        {
            Region = g.Key,
            AvgTraffic = g.Average(x => x.TrafficTrendPercentage),
            AvgTrialToPaid = g.Average(x => x.AverageTrialToPaidRate)
        })
        .ToList();

    // Convert decimal to double for Math.Sqrt and Math.Pow
    var trafficAvg = aggregated.Average(r => r.AvgTraffic);
    var trafficStd = (decimal)Math.Sqrt(aggregated.Average(r => Math.Pow((double)(r.AvgTraffic - trafficAvg), 2)));

    var trialAvg = aggregated.Average(r => r.AvgTrialToPaid);
    var trialStd = (decimal)Math.Sqrt(aggregated.Average(r => Math.Pow((double)(r.AvgTrialToPaid - trialAvg), 2)));

    // Now trafficThreshold and trialThreshold can be decimal
    var trafficThreshold = trafficAvg - trafficStd;
    var trialThreshold = trialAvg - trialStd;

    foreach (var region in aggregated)
    {
        bool trafficUnder = region.AvgTraffic < trafficThreshold;
        bool trialUnder = region.AvgTrialToPaid < trialThreshold;

        if (!trafficUnder && !trialUnder) continue; // skip regions performing okay

        string severity = (trafficUnder && trialUnder) ? "High" : "Medium";

        alerts.Add(new AlertDto
        {
            AlertType = Enums.AlertType.RegionalUnderperformance.ToString(),
            Severity = severity,
            Message = $"{region.Region} region is underperforming: " +
                      $"traffic trend {region.AvgTraffic:F2}% vs threshold {trafficThreshold:F2}%, " +
                      $"trial-to-paid rate {region.AvgTrialToPaid:F2}% vs threshold {trialThreshold:F2}%",
            Entity = region.Region,
            Value = Math.Round(region.AvgTrialToPaid, 2),
            Threshold = Math.Round(trialThreshold, 2),
            RecommendedAction = $"Investigate regional barriers to conversion. Consider localized pricing, payment methods, language support, or onboarding improvements specific to {region.Region}.",
            DetectedAt = DateTime.UtcNow
        });
    }

    return alerts;
}

public async Task<List<AlertDto>> DetectSeasonalDipsAsync(
    DateTime? startDate = null,
    DateTime? endDate = null)
{
    try
    {
        var alerts = new List<AlertDto>();

        // Fetch metrics, optionally filtered by date
        var metricsQuery = _context.MonthlyMetrics.AsQueryable();
        if (startDate.HasValue) metricsQuery = metricsQuery.Where(m => m.Month >= startDate.Value);
        if (endDate.HasValue) metricsQuery = metricsQuery.Where(m => m.Month <= endDate.Value);

        var allMetrics = await metricsQuery.OrderBy(m => m.Month).ToListAsync();

        // Group metrics by year and quarter
        var quarters = allMetrics
            .GroupBy(m => new
            {
                Year = m.Month.Year,
                Quarter = (m.Month.Month - 1) / 3 + 1
            })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Quarter)
            .ToList();

        for (int i = 1; i < quarters.Count; i++)
        {
            var prevQuarter = quarters[i - 1];
            var currentQuarter = quarters[i];

            // Calculate averages per quarter
            decimal prevTraffic = (decimal)prevQuarter.Average(m => (double)m.WebsiteTraffic);
            decimal currTraffic = (decimal)currentQuarter.Average(m => (double)m.WebsiteTraffic);

            decimal prevConversion = (decimal)prevQuarter.Average(m => (double)m.PaidConversions);
            decimal currConversion = (decimal)currentQuarter.Average(m => (double)m.PaidConversions);

            decimal prevChurn = (decimal)prevQuarter.Average(m => (double)m.ChurnRate);
            decimal currChurn = (decimal)currentQuarter.Average(m => (double)m.ChurnRate);

            // Calculate percentage changes
            decimal trafficDrop = prevTraffic != 0 ? (currTraffic - prevTraffic) / prevTraffic * 100 : 0;
            decimal conversionDrop = prevConversion != 0 ? (currConversion - prevConversion) / prevConversion * 100 : 0;
            decimal churnSpike = prevChurn != 0 ? (currChurn - prevChurn) / prevChurn * 100 : 0;

            // Strict rule: all three metrics must be bad to trigger an alert
            bool isTrafficBad = trafficDrop < 0;
            bool isConversionBad = conversionDrop < 0;
            bool isChurnBad = churnSpike > 0;

            if (isTrafficBad && isConversionBad && isChurnBad)
            {
                string quarterName = $"Q{currentQuarter.Key.Quarter} {currentQuarter.Key.Year}";

                // Determine worst value for severity
                decimal worstValue = new[]
                {
                    trafficDrop,
                    conversionDrop,
                    -churnSpike // negative for spike
                }.Min();

                // Build message
                var messages = new List<string>
                {
                    $"traffic drop of {trafficDrop:F2}%",
                    $"conversion drop of {conversionDrop:F2}%",
                    $"churn spike of {churnSpike:F2}%"
                };

                alerts.Add(new AlertDto
                {
                    AlertType = Enums.AlertType.SeasonalDip.ToString(),
                    Severity = worstValue <= -10 ? AlertSeverity.High.ToString() : AlertSeverity.Medium.ToString(),
                    Message = $"{quarterName} shows {string.Join(", ", messages)} compared to previous quarter",
                    Entity = quarterName,
                    Value = Math.Round(worstValue, 2),
                    Threshold = -10m,
                    RecommendedAction = "Analyze seasonal patterns and plan proactive campaigns. Consider targeted promotions, engagement strategies, or operational improvements.",
                    DetectedAt = DateTime.UtcNow
                });
            }
        }

        return alerts;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error detecting seasonal dips");
        throw;
    }
}

    public async Task<List<AlertDto>> DetectChannelWasteAsync(
    decimal maxConversion = 2.0m,
    int minSessions = 10000,
    List<string>? channelsToCheck = null,
    DateTime? startDate = null,
    DateTime? endDate = null)
{
    try
    {
        var alerts = new List<AlertDto>();

        var channelMetrics = await _metricsService.GetChannelMetricsAsync(startDate, endDate);

        // if no specific channels given, check all
        var filteredChannels = channelMetrics
            .Where(c => (channelsToCheck == null || channelsToCheck.Contains(c.Channel))
                        && c.ConversionRate < maxConversion
                        && c.TotalSessions > minSessions)
            .ToList();

        foreach (var channel in filteredChannels)
        {
            alerts.Add(new AlertDto
            {
                AlertType = Enums.AlertType.ChannelWaste.ToString(),
                Severity = AlertSeverity.Medium.ToString(),
                Message = $"{channel.Channel} channel has {channel.ConversionRate:F2}% conversion rate with {channel.TotalSessions} sessions",
                Entity = channel.Channel,
                Value = channel.ConversionRate,
                Threshold = maxConversion,
                RecommendedAction = "Review targeting, content, or creative for this channel. Consider reallocating resources to better-performing channels.",
                DetectedAt = DateTime.UtcNow
            });
        }

        return alerts;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error detecting channel waste");
        throw;
    }
}

}