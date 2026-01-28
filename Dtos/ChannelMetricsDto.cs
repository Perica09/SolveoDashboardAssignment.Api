namespace SolveoDashboardAssignment.Api.Dtos;

public class ChannelMetricsDto
{
    public string Channel { get; set; } = string.Empty;
    public decimal ConversionRate { get; set; }
    public int TotalSessions { get; set; }
    public int TotalSignups { get; set; }
    public int AverageSessionDuration { get; set; }
    public decimal BounceRate { get; set; }
    public decimal PagesPerSession { get; set; }
    public int MonthCount { get; set; }
}
