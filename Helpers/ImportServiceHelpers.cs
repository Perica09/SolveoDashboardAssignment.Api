using OfficeOpenXml;

namespace SolveoDashboardAssignment.Api.Helpers;

/// <summary>
/// Helper methods for Excel import operations
/// </summary>
public static class ImportServiceHelpers
{
    /// <summary>
    /// Gets a cell value from an Excel worksheet and converts it to the specified type
    /// </summary>
    /// <typeparam name="T">The target type to convert the cell value to</typeparam>
    /// <param name="worksheet">The Excel worksheet to read from</param>
    /// <param name="row">The row number (1-based)</param>
    /// <param name="columnMap">Dictionary mapping normalized column names to column indices</param>
    /// <param name="columnKey">The normalized column name to look up</param>
    /// <param name="defaultValue">Default value to return if column is not found or cell is empty</param>
    /// <returns>The cell value converted to type T, or the default value</returns>
    /// <exception cref="InvalidOperationException">Thrown when the cell value cannot be converted to the target type</exception>
    public static T GetCellValue<T>(ExcelWorksheet worksheet, int row, Dictionary<string, int> columnMap, string columnKey, T defaultValue = default!)
    {
        if (!columnMap.TryGetValue(columnKey, out int colIndex))
        {
            return defaultValue;
        }

        var cellValue = worksheet.Cells[row, colIndex].Value;

        if (cellValue == null)
        {
            return default!;
        }

        try
        {
            return (T)Convert.ChangeType(cellValue, typeof(T));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to convert value '{cellValue}' to type {typeof(T).Name} at row {row}, column {columnKey}",
                ex);
        }
    }

    /// <summary>
    /// Normalizes a column name by removing spaces and underscores, and converting to lowercase
    /// </summary>
    /// <param name="columnName">The column name to normalize</param>
    /// <returns>Normalized column name</returns>
    public static string NormalizeColumnName(string columnName)
    {
        // Remove spaces, underscores, and convert to lowercase
        return columnName.Replace(" ", "").Replace("_", "").ToLowerInvariant();
    }

    /// <summary>
    /// Parses a string value to a boolean (supports "yes", "true", "1")
    /// </summary>
    /// <param name="value">The string value to parse</param>
    /// <returns>True if the value is "yes", "true", or "1" (case-insensitive); otherwise false</returns>
    public static bool ParseBooleanValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var normalized = value.Trim().ToLowerInvariant();
        return normalized == "yes" || normalized == "true" || normalized == "1";
    }

    /// <summary>
    /// Checks if a row in an Excel worksheet is empty (all cells are null)
    /// </summary>
    /// <param name="worksheet">The Excel worksheet to check</param>
    /// <param name="row">The row number to check (1-based)</param>
    /// <returns>True if the row is empty; otherwise false</returns>
    public static bool IsRowEmpty(ExcelWorksheet worksheet, int row)
    {
        // Handle empty worksheets
        if (worksheet.Dimension == null)
            return true;
            
        for (int col = 1; col <= worksheet.Dimension.Columns; col++)
        {
            if (worksheet.Cells[row, col].Value != null)
                return false;
        }
        return true;
    }
    
    /// <summary>
    /// Maps column headers from the first row of an Excel worksheet to their column indices
    /// </summary>
    /// <param name="worksheet">The Excel worksheet to read headers from</param>
    /// <returns>Dictionary mapping normalized column names to their column indices (1-based)</returns>
    public static Dictionary<string, int> MapColumnHeaders(ExcelWorksheet worksheet)
    {
        var columnMap = new Dictionary<string, int>();
        var headerRow = 1; // Assuming first row is header

        for (int col = 1; col <= worksheet.Dimension.Columns; col++)
        {
            var headerValue = worksheet.Cells[headerRow, col].Value?.ToString();
            if (!string.IsNullOrWhiteSpace(headerValue))
            {
                var normalizedHeader = NormalizeColumnName(headerValue);
                columnMap[normalizedHeader] = col;
            }
        }

        return columnMap;
    }

    /// <summary>
    /// Parses a month string in "YYYY-MM" format to a DateTime object (first day of the month in UTC)
    /// </summary>
    /// <param name="monthValue">Month string in "YYYY-MM" or "YYYY-M" format</param>
    /// <returns>DateTime representing the first day of the specified month in UTC</returns>
    /// <exception cref="ArgumentException">Thrown when the month value is empty</exception>
    /// <exception cref="FormatException">Thrown when the month value is not in the expected format</exception>
    public static DateTime ParseMonthToDateTime(string monthValue)
    {
        // Input: "2024-01" or "2024-1"
        // Output: DateTime(2024, 1, 1) in UTC
        
        if (string.IsNullOrWhiteSpace(monthValue))
            throw new ArgumentException("Month value cannot be empty");
        
        var parts = monthValue.Trim().Split('-');
        if (parts.Length != 2)
            throw new FormatException($"Invalid month format: {monthValue}. Expected YYYY-MM");
        
        if (!int.TryParse(parts[0], out int year) || !int.TryParse(parts[1], out int month))
            throw new FormatException($"Invalid month format: {monthValue}");
        
        return new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
    }
    
    /// <summary>
    /// Validates that all required columns are present in the column map
    /// </summary>
    /// <param name="columnMap">Dictionary mapping normalized column names to column indices</param>
    /// <param name="requiredColumns">Array of required column names (normalized)</param>
    /// <exception cref="InvalidOperationException">Thrown when a required column is not found</exception>
    public static void ValidateRequiredColumns(
        Dictionary<string, int> columnMap,
        string[] requiredColumns)
    {
        foreach (var required in requiredColumns)
        {
            if (!columnMap.ContainsKey(required))
            {
                throw new InvalidOperationException(
                    $"Required column '{required}' not found in worksheet");
            }
        }
    }
}
