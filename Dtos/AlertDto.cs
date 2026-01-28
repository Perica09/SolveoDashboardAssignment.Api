namespace SolveoDashboardAssignment.Api.Dtos;

/// <summary>
/// Data transfer object for performance alerts
/// </summary>
public class AlertDto
{
    /// <summary>
    /// Type of alert
    /// </summary>
    public string AlertType { get; set; } = string.Empty;
    
    /// <summary>
    /// Severity level of the alert
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// Descriptive message about the alert
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Entity (keyword, channel, region, etc.) that triggered the alert
    /// </summary>
    public string Entity { get; set; } = string.Empty;
    
    /// <summary>
    /// Current value of the metric
    /// </summary>
    public decimal Value { get; set; }
    
    /// <summary>
    /// Threshold value that was exceeded or not met
    /// </summary>
    public decimal Threshold { get; set; }
    
    /// <summary>
    /// Recommended action to address the alert
    /// </summary>
    public string RecommendedAction { get; set; } = string.Empty;
    
    /// <summary>
    /// Timestamp when the alert was detected
    /// </summary>
    public DateTime? DetectedAt { get; set; }
}
