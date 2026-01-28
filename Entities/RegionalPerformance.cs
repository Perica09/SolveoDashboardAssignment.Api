using System.ComponentModel.DataAnnotations;

namespace SolveoDashboardAssignment.Api.Entities;

/// <summary>
/// Entity representing monthly performance metrics by geographic region
/// </summary>
public class RegionalPerformance
{
    [Key]
    public int Id { get; set; }
    public required string Region { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public DateTime Month { get; set; }
    public int OrganicTraffic { get; set; }
    public int PaidTraffic { get; set; }
    public int TotalTraffic { get; set; }
    public int TrialsStarted { get; set; }
    public int PaidConversions { get; set; }
    public decimal TrialToPaidRate { get; set; }
    public decimal MrrUsd { get; set; }
    public decimal CacUsd { get; set; }
    public decimal LtvUsd { get; set; }
}
