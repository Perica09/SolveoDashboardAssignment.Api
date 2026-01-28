namespace SolveoDashboardAssignment.Api.Enums;

/// <summary>
/// Severity levels for performance alerts
/// </summary>
public enum AlertSeverity
{
    /// <summary>
    /// Low severity - informational alert
    /// </summary>
    Low,
    
    /// <summary>
    /// Medium severity - requires attention
    /// </summary>
    Medium,
    
    /// <summary>
    /// High severity - requires immediate attention
    /// </summary>
    High,
    
    /// <summary>
    /// Critical severity - requires urgent action
    /// </summary>
    Critical
}
