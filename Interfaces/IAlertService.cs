using SolveoDashboardAssignment.Api.Dtos;

namespace SolveoDashboardAssignment.Api.Interfaces;

/// <summary>
/// Service for detecting and managing performance alerts
/// </summary>
public interface IAlertService
{
    /// <summary>
    /// Get all alerts from all detection methods sorted by severity
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>List of all detected alerts</returns>
    Task<List<AlertDto>> GetAllAlertsAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null);
    
    /// <summary>
    /// Detect keywords with high traffic but low conversion rates
    /// </summary>
    /// <param name="minTraffic">Minimum traffic threshold (default: 2000)</param>
    /// <param name="maxConversion">Maximum conversion rate threshold (default: 1.5%)</param>
    /// <returns>List of alerts for high traffic, low conversion keywords</returns>
    Task<List<AlertDto>> DetectHighTrafficLowConversionKeywordsAsync(
        int minTraffic = 2000, 
        decimal maxConversion = 1.5m);
    
    /// <summary>
    /// Detect keywords affected by AI Overview cannibalization (traffic decline with AI Overview triggered)
    /// </summary>
    /// <param name="minDeclinePercentage">Minimum traffic decline percentage threshold (default: 10%)</param>
    /// <returns>List of alerts for AI Overview cannibalization</returns>
    Task<List<AlertDto>> DetectAiOverviewCannibalizationAsync(
        decimal minDeclinePercentage = 10.0m);
    
    /// <summary>
    /// Detect underperforming regions based on CAC/LTV ratio
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>List of alerts for underperforming regions</returns>
    Task<List<AlertDto>> DetectRegionalUnderperformanceAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null);
    
    /// <summary>
    /// Detect seasonal dips in performance (significant month-over-month declines)
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>List of alerts for seasonal performance dips</returns>
    Task<List<AlertDto>> DetectSeasonalDipsAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null);
    
    /// <summary>
    /// Detect channels with wasteful spending (high sessions but low conversion rates)
    /// </summary>
    /// <param name="maxConversion">Maximum conversion rate threshold (default: 2.0%)</param>
    /// <param name="minSessions">Minimum sessions threshold (default: 10000)</param>
    /// <param name="channelsToCheck">Optional list of specific channels to check</param>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>List of alerts for channels with wasteful spending</returns>
    Task<List<AlertDto>> DetectChannelWasteAsync(
        decimal maxConversion = 2.0m,
        int minSessions = 10000,
        List<string>? channelsToCheck = null,
        DateTime? startDate = null, 
        DateTime? endDate = null);
}
