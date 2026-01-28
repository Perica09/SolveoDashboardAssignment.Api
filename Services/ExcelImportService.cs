using SolveoDashboardAssignment.Api.Data;
using SolveoDashboardAssignment.Api.Entities;
using SolveoDashboardAssignment.Api.Interfaces;
using OfficeOpenXml;
using SolveoDashboardAssignment.Api.Dtos;
using SolveoDashboardAssignment.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace SolveoDashboardAssignment.Api.Services;

/// <summary>
/// Service for importing dashboard data from Excel files
/// </summary>
public class ExcelImportService : IExcelImportService
{
    private readonly AppDbContext _context;

    public ExcelImportService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Imports data from an Excel file containing multiple sheets with dashboard metrics
    /// </summary>
    /// <param name="file">Excel file (.xlsx) with Keyword Performance, Channel Performance, Monthly Metrics, and Regional Performance sheets</param>
    /// <returns>Statistics about the import operation including rows processed and any errors encountered</returns>
    public async Task<ImportStatisticsDto> ImportAsync(IFormFile file)
    {
        // Set EPPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var stats = new ImportStatisticsDto();

        using var stream = file.OpenReadStream();
        using var package = new ExcelPackage(stream);

        // Process all four sheets
        await ProcessSheet(
            package, 
            "Keyword Performance", 
            MapRowToKeywordPerformance, 
            _context.KeywordPerformances,
            ["keyword", "category"], 
            stats);

        await ProcessSheet(
            package, 
            "Channel Performance", 
            MapRowToChannelPerformance, 
            _context.ChannelPerformances, 
            ["channel"], 
            stats);

        await ProcessSheet(
            package, 
            "Monthly Metrics", 
            MapRowToMonthlyMetrics, 
            _context.MonthlyMetrics, 
            [], 
            stats);

        await ProcessSheet(
            package, 
            "Regional Performance", 
            MapRowToRegionalPerformance, 
            _context.RegionalPerformances, 
            ["region", "country", "city"], 
            stats);

        // Calculate overall statistics
        foreach (var sheetStat in stats.SheetStats.Values)
        {
            stats.TotalRowsAllSheets += sheetStat.TotalRows;
            stats.TotalImportedAllSheets += sheetStat.ImportedRows;
            stats.TotalSkippedAllSheets += sheetStat.SkippedRows;
        }

        return stats;
    }

    private async Task ProcessSheet<TEntity>(
        ExcelPackage package,
        string sheetName,
        Func<ExcelWorksheet, int, Dictionary<string, int>, TEntity> mapFunction,
        DbSet<TEntity> dbSet,
        string[] requiredColumns,
        ImportStatisticsDto stats)
        where TEntity : class
    {
        var sheetStats = new SheetStatistics { SheetName = sheetName };
        stats.SheetStats[sheetName] = sheetStats;

        try
        {
            // Find the worksheet
            var worksheet = package.Workbook.Worksheets[sheetName];
            if (worksheet == null)
            {
                var errorMsg = $"Worksheet '{sheetName}' not found in the Excel file";
                sheetStats.Errors.Add(errorMsg);
                stats.GlobalErrors.Add(errorMsg);
                return;
            }

           var columnMap = ImportServiceHelpers.MapColumnHeaders(worksheet);

            try
            {
                ImportServiceHelpers.ValidateRequiredColumns(columnMap, requiredColumns);
            }
            catch (Exception ex)
            {
                sheetStats.Errors.Add(ex.Message);
                stats.GlobalErrors.Add(ex.Message);
                return; // 🚀 STOP processing this sheet
            }

            var entities = ReadSheetWithStats(worksheet, mapFunction, columnMap, sheetStats);

            // Clear existing data and replace with new data in a transaction
            // This ensures data integrity - either all changes succeed or all are rolled back
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await dbSet.ExecuteDeleteAsync();
                await dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            var errorMsg = $"Error processing sheet '{sheetName}': {ex.Message}";
            sheetStats.Errors.Add(errorMsg);
            stats.GlobalErrors.Add(errorMsg);
        }
    }

    private static List<TEntity> ReadSheetWithStats<TEntity>(
        ExcelWorksheet worksheet,
        Func<ExcelWorksheet, int, Dictionary<string, int>, TEntity> mapFunction,
        Dictionary<string, int> columnMap,
        SheetStatistics stats)
    {
        var results = new List<TEntity>();

        // Skip header row (row 1), start from row 2
        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
        {
            stats.TotalRows++;

            // Skip empty rows
            if (ImportServiceHelpers.IsRowEmpty(worksheet, row))
            {
                stats.SkippedRows++;
                continue;
            }

            try
            {
                var entity = mapFunction(worksheet, row, columnMap);
                results.Add(entity);
                stats.ImportedRows++;
            }
            catch (Exception ex)
            {
                stats.Errors.Add($"Row {row}: {ex.Message}");
            }
        }

        return results;
    }

    // Mapper methods for each entity type

    private static KeywordPerformance MapRowToKeywordPerformance(
        ExcelWorksheet worksheet,
        int row,
        Dictionary<string, int> columnMap)
    {
        return new KeywordPerformance
        {
            Keyword = ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "keyword"),
            Category = ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "category"),
            Traffic2024 = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "traffic2024"),
            Traffic2025 = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "traffic2025"),
            TrafficChangePct = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "trafficchangepct"),
            Position2024 = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "position2024"),
            Position2025 = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "position2025"),
            PositionChange = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "positionchange"),
            Signups2024 = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "signups2024"),
            Signups2025 = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "signups2025"),
            ConversionRate2024 = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "conversionrate2024"),
            ConversionRate2025 = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "conversionrate2025"),
            AiOverviewTriggered = ImportServiceHelpers.ParseBooleanValue(ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "aioverviewtriggered")),
            DifficultyScore = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "difficultyscore"),
            CpcUsd = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "cpcusd")
        };
    }

    private static ChannelPerformance MapRowToChannelPerformance(
        ExcelWorksheet worksheet,
        int row,
        Dictionary<string, int> columnMap)
    {
        return new ChannelPerformance
        {
            Month = ImportServiceHelpers.ParseMonthToDateTime(
                ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "month")),
            Channel = ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "channel"),
            Sessions = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "sessions"),
            Signups = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "signups"),
            ConversionRate = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "conversionrate"),
            AvgSessionDurationSec = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "avgsessiondurationsec"),
            BounceRate = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "bouncerate"),
            PagesPerSession = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "pagespersession")
        };
    }

    private static MonthlyMetrics MapRowToMonthlyMetrics(
        ExcelWorksheet worksheet,
        int row,
        Dictionary<string, int> columnMap)
    {
        return new MonthlyMetrics
        {
            Month = ImportServiceHelpers.ParseMonthToDateTime(
                ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "month")),
            WebsiteTraffic = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "websitetraffic"),
            UniqueSignups = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "uniquesignups"),
            TrialsStarted = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "trialsstarted"),
            PaidConversions = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "paidconversions"),
            MrrUsd = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "mrrusd"),
            ChurnRate = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "churnrate"),
            SignupToTrialRate = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "signuptotrialrate"),
            TrialToPaidRate = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "trialtopaidrate"),
            NetNewMrr = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "netnewmrr"),
            ExpansionMrr = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "expansionmrr"),
            ChurnedMrr = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "churnedmrr")
        };
    }

    private static RegionalPerformance MapRowToRegionalPerformance(
        ExcelWorksheet worksheet,
        int row,
        Dictionary<string, int> columnMap)
    {
        return new RegionalPerformance
        {
            Region = ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "region"),
            Country = ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "country"),
            City = ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "city"),
            Month = ImportServiceHelpers.ParseMonthToDateTime(
                ImportServiceHelpers.GetCellValue<string>(worksheet, row, columnMap, "month")),
            OrganicTraffic = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "organictraffic"),
            PaidTraffic = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "paidtraffic"),
            TotalTraffic = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "totaltraffic"),
            TrialsStarted = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "trialsstarted"),
            PaidConversions = ImportServiceHelpers.GetCellValue<int>(worksheet, row, columnMap, "paidconversions"),
            TrialToPaidRate = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "trialtopaidrate"),
            MrrUsd = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "mrrusd"),
            CacUsd = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "cacusd"),
            LtvUsd = ImportServiceHelpers.GetCellValue<decimal>(worksheet, row, columnMap, "ltvusd")
        };
    }
}
