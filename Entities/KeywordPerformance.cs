using System.ComponentModel.DataAnnotations;

namespace SolveoDashboardAssignment.Api.Entities;

/// <summary>
/// Entity representing keyword performance metrics comparing 2024 and 2025 data
/// </summary>
public class KeywordPerformance
{
    [Key] 
    public int Id { get; set; }
    public required string Keyword { get; set; }
    public required string Category { get; set; }
    public int Traffic2024 { get; set; }
    public int Traffic2025 { get; set; }
    public decimal TrafficChangePct { get; set; }
    public int Position2024 { get; set; }
    public int Position2025 { get; set; }
    public int PositionChange { get; set; }
    public int Signups2024 { get; set; }
    public int Signups2025 { get; set; }
    public decimal ConversionRate2024 { get; set; }
    public decimal ConversionRate2025 { get; set; }
    public bool AiOverviewTriggered { get; set; }
    public int DifficultyScore { get; set; }
    public decimal CpcUsd { get; set; }
}
