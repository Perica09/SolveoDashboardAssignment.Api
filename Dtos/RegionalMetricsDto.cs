namespace SolveoDashboardAssignment.Api.Dtos;

public class RegionalMetricsDto
{
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal AverageTrialToPaidRate { get; set; }
    public decimal TrafficTrendPercentage { get; set; }
    public decimal CacLtvRatio { get; set; }
    public int TotalTraffic { get; set; }
    public int TotalConversions { get; set; }
    public decimal AverageCac { get; set; }
    public decimal AverageLtv { get; set; }
    public int MonthCount { get; set; }
}
