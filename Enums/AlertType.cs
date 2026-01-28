namespace SolveoDashboardAssignment.Api.Enums;

/// <summary>
/// Types of performance alerts that can be detected
/// </summary>
public enum AlertType
{
    /// <summary>
    /// Keywords with high traffic but low conversion rates
    /// </summary>
    HighTrafficLowConversion,
    
    /// <summary>
    /// Keywords affected by AI Overview cannibalization
    /// </summary>
    AiOverviewCannibalization,
    
    /// <summary>
    /// Regions with poor performance metrics
    /// </summary>
    RegionalUnderperformance,
    
    /// <summary>
    /// Seasonal dips in performance
    /// </summary>
    SeasonalDip,
    
    /// <summary>
    /// Channels with wasteful spending (high sessions, low conversion)
    /// </summary>
    ChannelWaste,
    
    /// <summary>
    /// Poor Customer Acquisition Cost to Lifetime Value ratio
    /// </summary>
    PoorCacLtvRatio,
    
    /// <summary>
    /// Monthly Recurring Revenue decline
    /// </summary>
    MrrDecline
}
