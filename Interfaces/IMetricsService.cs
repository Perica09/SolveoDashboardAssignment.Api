using SolveoDashboardAssignment.Api.Dtos;
using SolveoDashboardAssignment.Api.Entities;

namespace SolveoDashboardAssignment.Api.Interfaces;

/// <summary>
/// Service for retrieving and analyzing dashboard metrics
/// </summary>
public interface IMetricsService
{
    // Monthly Metrics
    
    /// <summary>
    /// Get the latest monthly metrics
    /// </summary>
    /// <returns>Latest monthly metrics or null if no data exists</returns>
    Task<MonthlyMetricsDto?> GetLatestMonthlyMetricsAsync();
    
    /// <summary>
    /// Get monthly metrics within a date range
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>List of monthly metrics</returns>
    Task<List<MonthlyMetricsDto>> GetMonthlyMetricsRangeAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null);
    
    /// <summary>
    /// Get MRR (Monthly Recurring Revenue) history for the specified number of months
    /// </summary>
    /// <param name="months">Number of months to retrieve</param>
    /// <returns>List of monthly MRR data</returns>
    Task<List<MonthlyMrrDto>> GetMrrHistoryAsync(int months = 12);
    
    // Regional Performance
    
    /// <summary>
    /// Get all regional performance data without aggregation
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="regions">Optional list of regions to filter</param>
    /// <param name="countries">Optional list of countries to filter</param>
    /// <param name="cities">Optional list of cities to filter</param>
    /// <returns>List of all regional performance records</returns>
    Task<List<RegionalPerformance>> GetAllRegionalDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        List<string>? regions = null,
        List<string>? countries = null,
        List<string>? cities = null);

    /// <summary>
    /// Get aggregated regional performance metrics
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="regions">Optional list of regions to filter</param>
    /// <param name="countries">Optional list of countries to filter</param>
    /// <param name="cities">Optional list of cities to filter</param>
    /// <returns>List of aggregated regional metrics</returns>
    Task<List<RegionalMetricsDto>> GetRegionalMetricsAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null, 
        List<string>? regions = null,
        List<string>? countries = null,
        List<string>? cities = null);
    
    /// <summary>
    /// Get average trial-to-paid conversion rate by region
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>Dictionary mapping region names to average trial-to-paid rates</returns>
    Task<Dictionary<string, decimal>> GetAverageTrialToPaidByRegionAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null);
    
    /// <summary>
    /// Get traffic trends by region
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>Dictionary mapping region names to traffic trend percentages</returns>
    Task<Dictionary<string, decimal>> GetTrafficTrendByRegionAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null);
    
    /// <summary>
    /// Get CAC/LTV (Customer Acquisition Cost / Lifetime Value) ratio by region
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="regions">Optional list of regions to filter</param>
    /// <returns>Dictionary mapping region names to CAC/LTV ratios</returns>
    Task<Dictionary<string, decimal>> GetCacLtvRatioByRegionAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null, 
        List<string>? regions = null);
    
    // Channel Performance
    
    /// <summary>
    /// Get all channel performance records without aggregation
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="channels">Optional list of channels to filter</param>
    /// <returns>List of all channel performance records</returns>
    Task<List<ChannelMonthlyDto>> GetAllChannelPerformanceAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        List<string>? channels = null);

    /// <summary>
    /// Get aggregated channel performance metrics
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="channels">Optional list of channels to filter</param>
    /// <returns>List of aggregated channel metrics</returns>
    Task<List<ChannelMetricsDto>> GetChannelMetricsAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null, 
        List<string>? channels = null);
    
    /// <summary>
    /// Get conversion rates by channel
    /// </summary>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>Dictionary mapping channel names to conversion rates</returns>
    Task<Dictionary<string, decimal>> GetConversionRateByChannelAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null);
    
    // Keyword Performance
    
    /// <summary>
    /// Get keyword performance metrics with optional filters
    /// </summary>
    /// <param name="categories">Optional list of categories to filter</param>
    /// <param name="minTraffic">Optional minimum traffic threshold</param>
    /// <param name="maxTraffic">Optional maximum traffic threshold</param>
    /// <returns>List of keyword metrics</returns>
    Task<List<KeywordMetricsDto>> GetKeywordMetricsAsync(
        List<string>? categories = null,
        int? minTraffic = null,
        int? maxTraffic = null);
    
    /// <summary>
    /// Get year-over-year traffic change for keywords
    /// </summary>
    /// <param name="minChangePercentage">Optional minimum change percentage threshold</param>
    /// <param name="categories">Optional list of categories to filter</param>
    /// <returns>List of keyword metrics with traffic change data</returns>
    Task<List<KeywordMetricsDto>> GetTrafficChangeYoYAsync(
        decimal? minChangePercentage = null,
        List<string>? categories = null);
}
