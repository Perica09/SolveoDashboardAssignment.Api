namespace SolveoDashboardAssignment.Api.Dtos;

public class ChannelMonthlyDto
{
    public string Month { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public int Sessions { get; set; }
    public int Signups { get; set; }
    public decimal ConversionRate { get; set; }
    public int AvgSessionDurationSec { get; set; }
    public decimal BounceRate { get; set; }
    public decimal PagesPerSession { get; set; }
}
