namespace SolveoDashboardAssignment.Api.Dtos;

public class MonthlyMetricsDto
{
    public decimal LatestMrr { get; set; }
    public decimal GrowthPercentageMoM { get; set; }
    public decimal SignupToTrialPercentage { get; set; }
    public decimal TrialToPaidPercentage { get; set; }
    public string Month { get; set; } = string.Empty;
    public decimal? PreviousMonthMrr { get; set; }
    public int WebsiteTraffic { get; set; }
    public int UniqueSignups { get; set; }
    public int TrialsStarted { get; set; }
    public int PaidConversions { get; set; }
    public decimal ChurnRate { get; set; }
}
