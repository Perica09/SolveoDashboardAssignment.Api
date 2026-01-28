namespace SolveoDashboardAssignment.Api.Dtos;

public class SheetStatistics
{
    public string SheetName { get; set; } = string.Empty;
    public int TotalRows { get; set; }
    public int ImportedRows { get; set; }
    public int SkippedRows { get; set; }
    public List<string> Errors { get; set; } = new();
}

public class ImportStatisticsDto
{
    public Dictionary<string, SheetStatistics> SheetStats { get; set; } = new();
    public int TotalRowsAllSheets { get; set; }
    public int TotalImportedAllSheets { get; set; }
    public int TotalSkippedAllSheets { get; set; }
    public List<string> GlobalErrors { get; set; } = new();
}
