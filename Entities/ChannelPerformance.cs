using System.ComponentModel.DataAnnotations;

namespace SolveoDashboardAssignment.Api.Entities;

/// <summary>
/// Entity representing monthly performance metrics for marketing channels
/// </summary>
public class ChannelPerformance
{
    [Key]
    public int Id { get; set;}
    public DateTime Month { get; set; }
    public required string Channel { get; set; }
    public int Sessions { get; set; }
    public int Signups { get; set; }
    public decimal ConversionRate { get; set; }
    public int AvgSessionDurationSec { get; set; }
    public decimal BounceRate { get; set; }
    public decimal PagesPerSession { get; set; }
}
