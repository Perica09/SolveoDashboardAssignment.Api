using System.ComponentModel.DataAnnotations;

namespace SolveoDashboardAssignment.Api.Entities;

/// <summary>
/// Entity representing monthly business metrics including traffic, conversions, and MRR
/// </summary>
public class MonthlyMetrics
{
    [Key]
    public int Id { get; set; }
    public DateTime Month { get; set; }
    public int WebsiteTraffic { get; set; }
    public int UniqueSignups { get; set; }
    public int TrialsStarted { get; set; }
    public int PaidConversions { get; set; }
    public decimal MrrUsd { get; set; }
    public decimal ChurnRate { get; set; }
    public decimal SignupToTrialRate { get; set; }
    public decimal TrialToPaidRate { get; set; }
    public decimal NetNewMrr { get; set; }
    public decimal ExpansionMrr { get; set; }
    public decimal ChurnedMrr { get; set; }
}
