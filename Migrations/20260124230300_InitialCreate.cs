using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SolveoDashboardAssignment.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Month = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Channel = table.Column<string>(type: "text", nullable: false),
                    Sessions = table.Column<int>(type: "integer", nullable: false),
                    Signups = table.Column<int>(type: "integer", nullable: false),
                    ConversionRate = table.Column<decimal>(type: "numeric", nullable: false),
                    AvgSessionDurationSec = table.Column<int>(type: "integer", nullable: false),
                    BounceRate = table.Column<decimal>(type: "numeric", nullable: false),
                    PagesPerSession = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPerformances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeywordPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Traffic2024 = table.Column<int>(type: "integer", nullable: false),
                    Traffic2025 = table.Column<int>(type: "integer", nullable: false),
                    TrafficChangePct = table.Column<decimal>(type: "numeric", nullable: false),
                    Position2024 = table.Column<int>(type: "integer", nullable: false),
                    Position2025 = table.Column<int>(type: "integer", nullable: false),
                    PositionChange = table.Column<int>(type: "integer", nullable: false),
                    Signups2024 = table.Column<int>(type: "integer", nullable: false),
                    Signups2025 = table.Column<int>(type: "integer", nullable: false),
                    ConversionRate2024 = table.Column<decimal>(type: "numeric", nullable: false),
                    ConversionRate2025 = table.Column<decimal>(type: "numeric", nullable: false),
                    AiOverviewTriggered = table.Column<bool>(type: "boolean", nullable: false),
                    DifficultyScore = table.Column<int>(type: "integer", nullable: false),
                    CpcUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordPerformances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Month = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WebsiteTraffic = table.Column<int>(type: "integer", nullable: false),
                    UniqueSignups = table.Column<int>(type: "integer", nullable: false),
                    TrialsStarted = table.Column<int>(type: "integer", nullable: false),
                    PaidConversions = table.Column<int>(type: "integer", nullable: false),
                    MrrUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    ChurnRate = table.Column<decimal>(type: "numeric", nullable: false),
                    SignupToTrialRate = table.Column<decimal>(type: "numeric", nullable: false),
                    TrialToPaidRate = table.Column<decimal>(type: "numeric", nullable: false),
                    NetNewMrr = table.Column<decimal>(type: "numeric", nullable: false),
                    ExpansionMrr = table.Column<decimal>(type: "numeric", nullable: false),
                    ChurnedMrr = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionalPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Region = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Month = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrganicTraffic = table.Column<int>(type: "integer", nullable: false),
                    PaidTraffic = table.Column<int>(type: "integer", nullable: false),
                    TotalTraffic = table.Column<int>(type: "integer", nullable: false),
                    TrialsStarted = table.Column<int>(type: "integer", nullable: false),
                    PaidConversions = table.Column<int>(type: "integer", nullable: false),
                    TrialToPaidRate = table.Column<decimal>(type: "numeric", nullable: false),
                    MrrUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    CacUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    LtvUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalPerformances", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelPerformances");

            migrationBuilder.DropTable(
                name: "KeywordPerformances");

            migrationBuilder.DropTable(
                name: "MonthlyMetrics");

            migrationBuilder.DropTable(
                name: "RegionalPerformances");
        }
    }
}
