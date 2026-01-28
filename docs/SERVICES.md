# Services Documentation

## Overview

This document provides detailed information about the service layer of the Solveo Dashboard API. Services contain the business logic and data access operations for the application.

---

## Table of Contents

- [Alert Service](#alert-service)
- [Excel Import Service](#excel-import-service)
- [Metrics Service](#metrics-service)
- [Service Dependencies](#service-dependencies)

---

## Alert Service

**Namespace:** `SolveoDashboardAssignment.Api.Services`  
**Interface:** [`IAlertService`](../SolveoDashboardAssignment.Api/Interfaces/IAlertService.cs)  
**Implementation:** [`AlertsService`](../SolveoDashboardAssignment.Api/Services/AlertsService.cs)

### Purpose

The Alert Service is responsible for detecting and managing performance alerts across various metrics including keywords, channels, regions, and seasonal trends. It analyzes data to identify potential issues and opportunities for optimization.

### Dependencies

- `AppDbContext` - Database context for data access
- `IMetricsService` - Metrics service for retrieving performance data
- `ILogger<AlertsService>` - Logger for error tracking and diagnostics

### Methods

#### GetAllAlertsAsync

Retrieves all alerts from all detection methods sorted by severity.

**Signature:**
```csharp
Task<List<AlertDto>> GetAllAlertsAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Filter alerts from this date onwards
- `endDate` (DateTime?, optional) - Filter alerts up to this date

**Returns:** `Task<List<AlertDto>>` - List of all detected alerts sorted by severity (Critical → High → Medium → Low)

**Features:**
- Aggregates alerts from all detection methods
- Error isolation - if one detection method fails, others continue
- Automatic severity-based sorting
- UTC date conversion for PostgreSQL compatibility

---

#### DetectHighTrafficLowConversionKeywordsAsync

Detects keywords with high traffic but low conversion rates, indicating potential optimization opportunities.

**Signature:**
```csharp
Task<List<AlertDto>> DetectHighTrafficLowConversionKeywordsAsync(
    int minTraffic = 2000, 
    decimal maxConversion = 1.5m)
```

**Parameters:**
- `minTraffic` (int, default: 2000) - Minimum traffic threshold
- `maxConversion` (decimal, default: 1.5) - Maximum conversion rate threshold (%)

**Returns:** `Task<List<AlertDto>>` - List of alerts for high traffic, low conversion keywords

**Alert Severity:** High

**Recommended Action:** "Review landing page content and optimize conversion funnel"

---

#### DetectAiOverviewCannibalizationAsync

Detects keywords affected by AI Overview cannibalization (traffic decline with AI Overview triggered).

**Signature:**
```csharp
Task<List<AlertDto>> DetectAiOverviewCannibalizationAsync(
    decimal minDeclinePercentage = 10.0m)
```

**Parameters:**
- `minDeclinePercentage` (decimal, default: 10.0) - Minimum traffic decline percentage threshold

**Returns:** `Task<List<AlertDto>>` - List of alerts for AI Overview cannibalization

**Alert Severity:** Critical

**Recommended Action:** "Optimize content for AI Overview or target alternative keywords"

---

#### DetectRegionalUnderperformanceAsync

Detects underperforming regions based on CAC/LTV ratio.

**Signature:**
```csharp
Task<List<AlertDto>> DetectRegionalUnderperformanceAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Filter from this date onwards
- `endDate` (DateTime?, optional) - Filter up to this date

**Returns:** `Task<List<AlertDto>>` - List of alerts for underperforming regions

**Alert Severity:** High (CAC/LTV < 3.0)

**Recommended Action:** "Review marketing spend and customer acquisition strategies for this region"

**Detection Logic:**
- Calculates CAC/LTV ratio for each region
- Flags regions with ratio < 3.0 (industry standard threshold)

---

#### DetectSeasonalDipsAsync

Detects seasonal dips in performance (significant month-over-month declines).

**Signature:**
```csharp
Task<List<AlertDto>> DetectSeasonalDipsAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Filter from this date onwards
- `endDate` (DateTime?, optional) - Filter up to this date

**Returns:** `Task<List<AlertDto>>` - List of alerts for seasonal performance dips

**Alert Severity:** 
- Critical: MRR decline > 20%
- High: MRR decline > 15%
- Medium: MRR decline > 10%

**Recommended Action:** "Investigate seasonal factors and plan promotional campaigns"

**Detection Logic:**
- Compares month-over-month MRR changes
- Flags declines > 10%

---

#### DetectChannelWasteAsync

Detects channels with wasteful spending (high sessions but low conversion rates).

**Signature:**
```csharp
Task<List<AlertDto>> DetectChannelWasteAsync(
    decimal maxConversion = 2.0m,
    int minSessions = 10000,
    List<string>? channelsToCheck = null,
    DateTime? startDate = null, 
    DateTime? endDate = null)
```

**Parameters:**
- `maxConversion` (decimal, default: 2.0) - Maximum conversion rate threshold (%)
- `minSessions` (int, default: 10000) - Minimum sessions threshold
- `channelsToCheck` (List<string>?, optional) - Specific channels to check
- `startDate` (DateTime?, optional) - Filter from this date onwards
- `endDate` (DateTime?, optional) - Filter up to this date

**Returns:** `Task<List<AlertDto>>` - List of alerts for channels with wasteful spending

**Alert Severity:** High

**Recommended Action:** "Review ad targeting and landing page optimization for this channel"

**Detection Logic:**
- Identifies channels with high traffic (> minSessions) but low conversion (< maxConversion)
- Aggregates data across specified date range

---

## Excel Import Service

**Namespace:** `SolveoDashboardAssignment.Api.Services`  
**Interface:** [`IExcelImportService`](../SolveoDashboardAssignment.Api/Interfaces/IExcelImportService.cs)  
**Implementation:** [`ExcelImportService`](../SolveoDashboardAssignment.Api/Services/ExcelImportService.cs)

### Purpose

The Excel Import Service handles importing dashboard data from Excel files (.xlsx format). It processes multiple sheets containing different types of metrics and provides detailed import statistics.

### Dependencies

- `AppDbContext` - Database context for data persistence
- `EPPlus` - Excel file processing library

### Methods

#### ImportAsync

Imports data from an Excel file containing multiple sheets with dashboard metrics.

**Signature:**
```csharp
Task<ImportStatisticsDto> ImportAsync(IFormFile file)
```

**Parameters:**
- `file` (IFormFile) - Excel file (.xlsx) with dashboard data

**Returns:** `Task<ImportStatisticsDto>` - Statistics about the import operation including rows processed and any errors encountered

**Expected Excel Sheets:**

1. **Keyword Performance**
   - Required columns: `keyword`, `category`
   - Maps to: [`KeywordPerformance`](../SolveoDashboardAssignment.Api/Entities/KeywordPerformance.cs) entity

2. **Channel Performance**
   - Required columns: `channel`
   - Maps to: [`ChannelPerformance`](../SolveoDashboardAssignment.Api/Entities/ChannelPerformance.cs) entity

3. **Monthly Metrics**
   - No required columns (all columns are validated)
   - Maps to: [`MonthlyMetrics`](../SolveoDashboardAssignment.Api/Entities/MonthlyMetrics.cs) entity

4. **Regional Performance**
   - Required columns: `region`, `country`, `city`
   - Maps to: [`RegionalPerformance`](../SolveoDashboardAssignment.Api/Entities/RegionalPerformance.cs) entity

**Features:**
- Multi-sheet processing
- Automatic column mapping
- Data validation
- Error isolation (errors in one sheet don't affect others)
- Detailed import statistics per sheet
- Duplicate detection and handling
- Transaction-based imports for data integrity

**Error Handling:**
- Missing sheets are reported but don't stop processing
- Invalid data rows are skipped and logged
- Missing required columns are reported
- Empty sheets are handled gracefully

---

### Helper Methods

#### ProcessSheet

Internal method for processing individual Excel sheets.

**Signature:**
```csharp
private async Task ProcessSheet<TEntity>(
    ExcelPackage package,
    string sheetName,
    Func<ExcelWorksheet, int, Dictionary<string, int>, TEntity> mapFunction,
    DbSet<TEntity> dbSet,
    string[] requiredColumns,
    ImportStatisticsDto stats)
    where TEntity : class
```

**Features:**
- Generic implementation for any entity type
- Column header validation
- Row-by-row processing with error isolation
- Automatic statistics tracking
- Database transaction management

---

## Metrics Service

**Namespace:** `SolveoDashboardAssignment.Api.Services`  
**Interface:** [`IMetricsService`](../SolveoDashboardAssignment.Api/Interfaces/IMetricsService.cs)  
**Implementation:** [`MetricsService`](../SolveoDashboardAssignment.Api/Services/MetricsService.cs)

### Purpose

The Metrics Service provides comprehensive data retrieval and analysis operations for dashboard metrics including monthly metrics, regional performance, channel performance, and keyword performance.

### Dependencies

- `AppDbContext` - Database context for data access
- `ILogger<MetricsService>` - Logger for error tracking and diagnostics

---

### Monthly Metrics Methods

#### GetLatestMonthlyMetricsAsync

Retrieves the most recent monthly metrics.

**Signature:**
```csharp
Task<MonthlyMetricsDto?> GetLatestMonthlyMetricsAsync()
```

**Returns:** `Task<MonthlyMetricsDto?>` - Latest monthly metrics or null if no data exists

**Calculations:**
- MRR growth percentage (month-over-month)
- Signup to trial conversion rate
- Trial to paid conversion rate

---

#### GetMonthlyMetricsRangeAsync

Retrieves monthly metrics within a specified date range.

**Signature:**
```csharp
Task<List<MonthlyMetricsDto>> GetMonthlyMetricsRangeAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering

**Returns:** `Task<List<MonthlyMetricsDto>>` - List of monthly metrics ordered by month

---

#### GetMrrHistoryAsync

Retrieves MRR (Monthly Recurring Revenue) history for the specified number of months.

**Signature:**
```csharp
Task<List<MonthlyMrrDto>> GetMrrHistoryAsync(int months = 12)
```

**Parameters:**
- `months` (int, default: 12) - Number of months to retrieve

**Returns:** `Task<List<MonthlyMrrDto>>` - List of monthly MRR data ordered by date (most recent first)

---

### Regional Performance Methods

#### GetAllRegionalDataAsync

Retrieves all regional performance records without aggregation.

**Signature:**
```csharp
Task<List<RegionalPerformance>> GetAllRegionalDataAsync(
    DateTime? startDate = null,
    DateTime? endDate = null,
    List<string>? regions = null,
    List<string>? countries = null,
    List<string>? cities = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering
- `regions` (List<string>?, optional) - Filter by specific regions
- `countries` (List<string>?, optional) - Filter by specific countries
- `cities` (List<string>?, optional) - Filter by specific cities

**Returns:** `Task<List<RegionalPerformance>>` - List of all regional performance records

---

#### GetRegionalMetricsAsync

Retrieves aggregated regional performance metrics.

**Signature:**
```csharp
Task<List<RegionalMetricsDto>> GetRegionalMetricsAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null, 
    List<string>? regions = null,
    List<string>? countries = null,
    List<string>? cities = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering
- `regions` (List<string>?, optional) - Filter by specific regions
- `countries` (List<string>?, optional) - Filter by specific countries
- `cities` (List<string>?, optional) - Filter by specific cities

**Returns:** `Task<List<RegionalMetricsDto>>` - List of aggregated regional metrics

**Aggregations:**
- Average trial-to-paid rate
- Traffic trend percentage
- CAC/LTV ratio
- Total traffic and conversions
- Average CAC and LTV
- Month count

---

#### GetAverageTrialToPaidByRegionAsync

Retrieves average trial-to-paid conversion rate by region.

**Signature:**
```csharp
Task<Dictionary<string, decimal>> GetAverageTrialToPaidByRegionAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering

**Returns:** `Task<Dictionary<string, decimal>>` - Dictionary mapping region names to average trial-to-paid rates

---

#### GetTrafficTrendByRegionAsync

Retrieves traffic trends by region.

**Signature:**
```csharp
Task<Dictionary<string, decimal>> GetTrafficTrendByRegionAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering

**Returns:** `Task<Dictionary<string, decimal>>` - Dictionary mapping region names to traffic trend percentages

**Calculation:**
- Compares first and last month traffic
- Returns percentage change

---

#### GetCacLtvRatioByRegionAsync

Retrieves Customer Acquisition Cost to Lifetime Value ratio by region.

**Signature:**
```csharp
Task<Dictionary<string, decimal>> GetCacLtvRatioByRegionAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null, 
    List<string>? regions = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering
- `regions` (List<string>?, optional) - Filter by specific regions

**Returns:** `Task<Dictionary<string, decimal>>` - Dictionary mapping region names to CAC/LTV ratios

**Calculation:**
- Average LTV / Average CAC
- Higher ratios indicate better performance (industry standard: > 3.0)

---

### Channel Performance Methods

#### GetAllChannelPerformanceAsync

Retrieves all channel performance records without aggregation.

**Signature:**
```csharp
Task<List<ChannelMonthlyDto>> GetAllChannelPerformanceAsync(
    DateTime? startDate = null,
    DateTime? endDate = null,
    List<string>? channels = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering
- `channels` (List<string>?, optional) - Filter by specific channels

**Returns:** `Task<List<ChannelMonthlyDto>>` - List of all channel performance records

---

#### GetChannelMetricsAsync

Retrieves aggregated channel performance metrics.

**Signature:**
```csharp
Task<List<ChannelMetricsDto>> GetChannelMetricsAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null, 
    List<string>? channels = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering
- `channels` (List<string>?, optional) - Filter by specific channels

**Returns:** `Task<List<ChannelMetricsDto>>` - List of aggregated channel metrics

**Aggregations:**
- Average conversion rate
- Total sessions and signups
- Average session duration
- Average bounce rate
- Average pages per session
- Month count

---

#### GetConversionRateByChannelAsync

Retrieves conversion rates by channel.

**Signature:**
```csharp
Task<Dictionary<string, decimal>> GetConversionRateByChannelAsync(
    DateTime? startDate = null, 
    DateTime? endDate = null)
```

**Parameters:**
- `startDate` (DateTime?, optional) - Start date for filtering
- `endDate` (DateTime?, optional) - End date for filtering

**Returns:** `Task<Dictionary<string, decimal>>` - Dictionary mapping channel names to conversion rates

---

### Keyword Performance Methods

#### GetKeywordMetricsAsync

Retrieves keyword performance metrics with optional filters.

**Signature:**
```csharp
Task<List<KeywordMetricsDto>> GetKeywordMetricsAsync(
    List<string>? categories = null,
    int? minTraffic = null,
    int? maxTraffic = null)
```

**Parameters:**
- `categories` (List<string>?, optional) - Filter by specific categories
- `minTraffic` (int?, optional) - Minimum traffic threshold
- `maxTraffic` (int?, optional) - Maximum traffic threshold

**Returns:** `Task<List<KeywordMetricsDto>>` - List of keyword metrics

**Data Included:**
- Year-over-year traffic change
- 2024 vs 2025 comparison
- Conversion rates
- Position changes
- AI Overview impact

---

#### GetTrafficChangeYoYAsync

Retrieves year-over-year traffic change for keywords.

**Signature:**
```csharp
Task<List<KeywordMetricsDto>> GetTrafficChangeYoYAsync(
    decimal? minChangePercentage = null,
    List<string>? categories = null)
```

**Parameters:**
- `minChangePercentage` (decimal?, optional) - Minimum change percentage threshold
- `categories` (List<string>?, optional) - Filter by specific categories

**Returns:** `Task<List<KeywordMetricsDto>>` - List of keyword metrics with traffic change data

**Ordering:** Results are ordered by traffic change percentage (descending)

---

## Service Dependencies

### Dependency Injection

All services are registered in [`Program.cs`](../SolveoDashboardAssignment.Api/Program.cs) with scoped lifetime:

```csharp
builder.Services.AddScoped<IExcelImportService, ExcelImportService>();
builder.Services.AddScoped<IMetricsService, MetricsService>();
builder.Services.AddScoped<IAlertService, AlertsService>();
```

### Service Relationships

```
AlertsService
├── Depends on: AppDbContext
├── Depends on: IMetricsService
└── Depends on: ILogger<AlertsService>

ExcelImportService
├── Depends on: AppDbContext
└── Uses: EPPlus library

MetricsService
├── Depends on: AppDbContext
└── Depends on: ILogger<MetricsService>
```

---

## Best Practices

### Error Handling

All services implement comprehensive error handling:
- Try-catch blocks for database operations
- Detailed logging for debugging
- Graceful degradation (e.g., AlertsService continues if one detection method fails)

### Performance Optimization

- Efficient LINQ queries with proper filtering
- Asynchronous operations throughout
- Database query optimization with Entity Framework Core
- Minimal data transfer with DTOs

### Data Validation

- Input parameter validation
- Required field checks
- Date range validation
- Null safety with nullable reference types

### Logging

All services use structured logging:
- Error logging for exceptions
- Information logging for important operations
- Consistent log message formatting
