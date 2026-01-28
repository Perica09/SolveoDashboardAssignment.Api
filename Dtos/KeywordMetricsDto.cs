namespace SolveoDashboardAssignment.Api.Dtos;

public class KeywordMetricsDto
{
    public string Keyword { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal TrafficChangeYoY { get; set; }
    public int Traffic2024 { get; set; }
    public int Traffic2025 { get; set; }
    public decimal ConversionRate2024 { get; set; }
    public decimal ConversionRate2025 { get; set; }
    public int Position2024 { get; set; }
    public int Position2025 { get; set; }
    public int PositionChange { get; set; }
    public string AiOverviewTriggered { get; set; } = string.Empty;
}
