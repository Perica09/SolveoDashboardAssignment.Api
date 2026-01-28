using Microsoft.AspNetCore.Mvc;
using SolveoDashboardAssignment.Api.Dtos;
using SolveoDashboardAssignment.Api.Interfaces;

namespace SolveoDashboardAssignment.Api.Controllers;

/// <summary>
/// Controller for retrieving dashboard metrics and analytics
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MetricsController : ControllerBase
{
    private readonly IMetricsService _metricsService;
    private readonly ILogger<MetricsController> _logger;

    public MetricsController(IMetricsService metricsService, ILogger<MetricsController> logger)
    {
        _metricsService = metricsService;
        _logger = logger;
    }

    /// <summary>
    /// Get the latest monthly metrics
    /// </summary>
    [HttpGet("monthly/latest")]
    public async Task<IActionResult> GetLatestMonthlyMetrics()
    {
        try
        {
            var result = await _metricsService.GetLatestMonthlyMetricsAsync();
            if (result == null)
                return NotFound("No monthly metrics found");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting latest monthly metrics");
            return StatusCode(500, "An error occurred while retrieving monthly metrics");
        }
    }

    /// <summary>
    /// Get monthly metrics within a date range
    /// </summary>
    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlyMetricsRange(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _metricsService.GetMonthlyMetricsRangeAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting monthly metrics range");
            return StatusCode(500, "An error occurred while retrieving monthly metrics");
        }
    }

    /// <summary>
    /// Get MRR (Monthly Recurring Revenue) history for the specified number of months
    /// </summary>
    /// <param name="months">Number of months to retrieve (default: 12)</param>
    /// <returns>List of monthly MRR data</returns>
    [HttpGet("mrr-history")]
    public async Task<ActionResult<List<MonthlyMrrDto>>> GetMrrHistory([FromQuery] int months = 12)
    {
        try
        {
            var recentMonths = await _metricsService.GetMrrHistoryAsync(months);
            return Ok(recentMonths);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch MRR history");
            return StatusCode(500, "An error occurred while retrieving MRR history");
        }
    }

    /// <summary>
    /// Get all regional performance data without aggregation
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="regions">Optional list of regions to filter</param>
    /// <param name="countries">Optional list of countries to filter</param>
    /// <param name="cities">Optional list of cities to filter</param>
    /// <returns>List of all regional performance records</returns>
    [HttpGet("regional/all")]
    public async Task<IActionResult> GetAllRegionalData(
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null,
    [FromQuery] List<string>? regions = null,
    [FromQuery] List<string>? countries = null,
    [FromQuery] List<string>? cities = null)
{
    try
    {
        var result = await _metricsService.GetAllRegionalDataAsync(startDate, endDate, regions, countries, cities);
        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting all regional data");
        return StatusCode(500, "An error occurred while retrieving all regional data");
    }
}


    /// <summary>
    /// Get regional performance metrics
    /// </summary>
    [HttpGet("regional")]
    public async Task<IActionResult> GetRegionalMetrics(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] List<string>? regions = null,
        [FromQuery] List<string>? countries = null,
        [FromQuery] List<string>? cities = null)
    {
        try
        {
            var result = await _metricsService.GetRegionalMetricsAsync(startDate, endDate, regions, countries, cities);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting regional metrics");
            return StatusCode(500, "An error occurred while retrieving regional metrics");
        }
    }

    /// <summary>
    /// Get average trial-to-paid conversion rate by region
    /// </summary>
    [HttpGet("regional/trial-to-paid")]
    public async Task<IActionResult> GetAverageTrialToPaidByRegion(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _metricsService.GetAverageTrialToPaidByRegionAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting trial to paid by region");
            return StatusCode(500, "An error occurred while retrieving trial to paid metrics");
        }
    }

    /// <summary>
    /// Get traffic trends by region
    /// </summary>
    [HttpGet("regional/traffic-trends")]
    public async Task<IActionResult> GetTrafficTrendsByRegion(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _metricsService.GetTrafficTrendByRegionAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting traffic trends by region");
            return StatusCode(500, "An error occurred while retrieving traffic trends");
        }
    }

    /// <summary>
    /// Get CAC/LTV ratio by region
    /// </summary>
    [HttpGet("regional/cac-ltv")]
    public async Task<IActionResult> GetCacLtvRatioByRegion(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] List<string>? regions = null)
    {
        try
        {
            var result = await _metricsService.GetCacLtvRatioByRegionAsync(startDate, endDate, regions);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting CAC/LTV ratio by region");
            return StatusCode(500, "An error occurred while retrieving CAC/LTV ratios");
        }
    }

    /// <summary>
    /// Get all channel performance records (monthly, no aggregation)
    /// </summary>
    [HttpGet("channels/all")]
public async Task<IActionResult> GetAllChannelPerformance(
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null,
    [FromQuery] List<string>? channels = null)
{
    try
    {
        var result = await _metricsService
            .GetAllChannelPerformanceAsync(startDate, endDate, channels);

        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting all channel performance data");
        return StatusCode(500, "An error occurred while retrieving channel performance data");
    }
}

    /// <summary>
    /// Get channel performance metrics
    /// </summary>
    [HttpGet("channels")]
    public async Task<IActionResult> GetChannelMetrics(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] List<string>? channels = null)
    {
        try
        {
            var result = await _metricsService.GetChannelMetricsAsync(startDate, endDate, channels);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting channel metrics");
            return StatusCode(500, "An error occurred while retrieving channel metrics");
        }
    }

    /// <summary>
    /// Get conversion rates by channel
    /// </summary>
    [HttpGet("channels/conversion-rates")]
    public async Task<IActionResult> GetConversionRatesByChannel(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _metricsService.GetConversionRateByChannelAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting conversion rates by channel");
            return StatusCode(500, "An error occurred while retrieving conversion rates");
        }
    }

    /// <summary>
    /// Get keyword performance metrics
    /// </summary>
    [HttpGet("keywords")]
    public async Task<IActionResult> GetKeywordMetrics(
        [FromQuery] List<string>? categories = null,
        [FromQuery] int? minTraffic = null,
        [FromQuery] int? maxTraffic = null)
    {
        try
        {
            var result = await _metricsService.GetKeywordMetricsAsync(categories, minTraffic, maxTraffic);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting keyword metrics");
            return StatusCode(500, "An error occurred while retrieving keyword metrics");
        }
    }

    /// <summary>
    /// Get year-over-year traffic change for keywords
    /// </summary>
    [HttpGet("keywords/traffic-change")]
    public async Task<IActionResult> GetTrafficChangeYoY(
        [FromQuery] decimal? minChangePercentage = null,
        [FromQuery] List<string>? categories = null)
    {
        try
        {
            var result = await _metricsService.GetTrafficChangeYoYAsync(minChangePercentage, categories);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting traffic change YoY");
            return StatusCode(500, "An error occurred while retrieving traffic change data");
        }
    }
}
