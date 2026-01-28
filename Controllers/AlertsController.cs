using Microsoft.AspNetCore.Mvc;
using SolveoDashboardAssignment.Api.Interfaces;

namespace SolveoDashboardAssignment.Api.Controllers;

/// <summary>
/// Controller for detecting and retrieving performance alerts
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertsService;
    private readonly ILogger<AlertsController> _logger;

    public AlertsController(IAlertService alertsService, ILogger<AlertsController> logger)
    {
        _alertsService = alertsService;
        _logger = logger;
    }

    /// <summary>
    /// Get all alerts sorted by severity
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAlerts(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _alertsService.GetAllAlertsAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all alerts");
            return StatusCode(500, "An error occurred while retrieving alerts");
        }
    }

    /// <summary>
    /// Detect keywords with high traffic but low conversion rates
    /// </summary>
    [HttpGet("high-traffic-low-conversion")]
    public async Task<IActionResult> GetHighTrafficLowConversionAlerts(
        [FromQuery] int minTraffic = 2000,
        [FromQuery] decimal maxConversion = 1.5m)
    {
        try
        {
            var result = await _alertsService.DetectHighTrafficLowConversionKeywordsAsync(minTraffic, maxConversion);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting high traffic low conversion keywords");
            return StatusCode(500, "An error occurred while detecting high traffic low conversion alerts");
        }
    }

    /// <summary>
    /// Detect keywords affected by AI Overview cannibalization
    /// </summary>
    [HttpGet("ai-overview-cannibalization")]
    public async Task<IActionResult> GetAiOverviewCannibalizationAlerts(
        [FromQuery] decimal minDeclinePercentage = 10.0m)
    {
        try
        {
            var result = await _alertsService.DetectAiOverviewCannibalizationAsync(minDeclinePercentage);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting AI Overview cannibalization");
            return StatusCode(500, "An error occurred while detecting AI Overview cannibalization alerts");
        }
    }

    /// <summary>
    /// Detect underperforming regions
    /// </summary>
    [HttpGet("regional-underperformance")]
    public async Task<IActionResult> GetRegionalUnderperformanceAlerts(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _alertsService.DetectRegionalUnderperformanceAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting regional underperformance");
            return StatusCode(500, "An error occurred while detecting regional underperformance alerts");
        }
    }

    /// <summary>
    /// Detect seasonal dips in performance
    /// </summary>
    [HttpGet("seasonal-dips")]
    public async Task<IActionResult> GetSeasonalDipsAlerts(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _alertsService.DetectSeasonalDipsAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting seasonal dips");
            return StatusCode(500, "An error occurred while detecting seasonal dips alerts");
        }
    }

    /// <summary>
    /// Detect channels with wasteful spending (high sessions, low conversion)
    /// </summary>
    [HttpGet("channel-waste")]
    public async Task<IActionResult> GetChannelWasteAlerts(
        [FromQuery] decimal maxConversion = 2.0m,
        [FromQuery] int minSessions = 10000,
        [FromQuery] List<string>? channelsToCheck = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _alertsService.DetectChannelWasteAsync(maxConversion, minSessions, channelsToCheck, startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting channel waste");
            return StatusCode(500, "An error occurred while detecting channel waste alerts");
        }
    }
}
