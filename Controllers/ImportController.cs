using Microsoft.AspNetCore.Mvc;
using SolveoDashboardAssignment.Api.Dtos;
using SolveoDashboardAssignment.Api.Interfaces;

namespace SolveoDashboardAssignment.Api.Controllers;

/// <summary>
/// Controller for importing dashboard data from Excel files
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExcelImportController : ControllerBase
{
    private readonly IExcelImportService _excelImportService;

    public ExcelImportController(IExcelImportService excelImportService)
    {
        _excelImportService = excelImportService;
    }

    /// <summary>
    /// Import data from Excel file (.xlsx format)
    /// </summary>
    /// <param name="file">Excel file containing dashboard data</param>
    /// <returns>Import statistics including rows processed and any errors</returns>
    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ImportStatisticsDto>> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        // Validate file type
        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only .xlsx files are supported");

        // Validate file size (10MB limit)
        const int maxFileSizeBytes = 10 * 1024 * 1024;
        if (file.Length > maxFileSizeBytes)
            return BadRequest("File size exceeds 10MB limit");

        var stats = await _excelImportService.ImportAsync(file);
        return Ok(stats);
    }
}
