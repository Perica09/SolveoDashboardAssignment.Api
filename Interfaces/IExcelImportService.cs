using SolveoDashboardAssignment.Api.Dtos;

namespace SolveoDashboardAssignment.Api.Interfaces;

/// <summary>
/// Service for importing data from Excel files
/// </summary>
public interface IExcelImportService
{
    /// <summary>
    /// Imports data from an Excel file containing multiple sheets with dashboard metrics
    /// </summary>
    /// <param name="file">Excel file (.xlsx) with Keyword Performance, Channel Performance, Monthly Metrics, and Regional Performance sheets</param>
    /// <returns>Statistics about the import operation including rows processed and any errors encountered</returns>
    public Task<ImportStatisticsDto> ImportAsync (IFormFile file);
}
