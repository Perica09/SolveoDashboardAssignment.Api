using Microsoft.EntityFrameworkCore;
using SolveoDashboardAssignment.Api.Entities;

namespace SolveoDashboardAssignment.Api.Data;

/// <summary>
/// Database context for the Solveo Dashboard application
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the AppDbContext
    /// </summary>
    /// <param name="options">Database context options</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Channel performance data
    /// </summary>
    public DbSet<ChannelPerformance> ChannelPerformances { get; set; }
    
    /// <summary>
    /// Keyword performance data
    /// </summary>
    public DbSet<KeywordPerformance> KeywordPerformances { get; set; }
    
    /// <summary>
    /// Monthly metrics data
    /// </summary>
    public DbSet<MonthlyMetrics> MonthlyMetrics { get; set; }
    
    /// <summary>
    /// Regional performance data
    /// </summary>
    public DbSet<RegionalPerformance> RegionalPerformances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Add indexes for frequently queried columns to improve performance
        modelBuilder.Entity<ChannelPerformance>()
            .HasIndex(c => c.Month)
            .HasDatabaseName("IX_ChannelPerformance_Month");

        modelBuilder.Entity<ChannelPerformance>()
            .HasIndex(c => c.Channel)
            .HasDatabaseName("IX_ChannelPerformance_Channel");

        modelBuilder.Entity<MonthlyMetrics>()
            .HasIndex(m => m.Month)
            .HasDatabaseName("IX_MonthlyMetrics_Month");

        modelBuilder.Entity<RegionalPerformance>()
            .HasIndex(r => r.Month)
            .HasDatabaseName("IX_RegionalPerformance_Month");

        modelBuilder.Entity<RegionalPerformance>()
            .HasIndex(r => r.Region)
            .HasDatabaseName("IX_RegionalPerformance_Region");

        modelBuilder.Entity<KeywordPerformance>()
            .HasIndex(k => k.Category)
            .HasDatabaseName("IX_KeywordPerformance_Category");

        modelBuilder.Entity<KeywordPerformance>()
            .HasIndex(k => k.AiOverviewTriggered)
            .HasDatabaseName("IX_KeywordPerformance_AiOverviewTriggered");
    }
}
