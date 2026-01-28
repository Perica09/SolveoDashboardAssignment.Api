# Models and Data Structures

## Overview

This document provides comprehensive documentation for all data models, entities, DTOs (Data Transfer Objects), and enums used in the Solveo Dashboard API.

---

## Table of Contents

- [Entities](#entities)
  - [ChannelPerformance](#channelperformance)
  - [KeywordPerformance](#keywordperformance)
  - [MonthlyMetrics](#monthlymetrics)
  - [RegionalPerformance](#regionalperformance)
- [Data Transfer Objects (DTOs)](#data-transfer-objects-dtos)
  - [AlertDto](#alertdto)
  - [ChannelMetricsDto](#channelmetricsdto)
  - [ChannelMonthlyDto](#channelmonthlydto)
  - [ImportStatisticsDto](#importstatisticsdto)
  - [KeywordMetricsDto](#keywordmetricsdto)
  - [MonthlyMetricsDto](#monthlymetricsdto)
  - [MonthlyMrrDto](#monthlymrrdto)
  - [RegionalMetricsDto](#regionalmetricsdto)
- [Enumerations](#enumerations)
  - [AlertSeverity](#alertseverity)
  - [AlertType](#alerttype)

---

## Entities

Entities represent database tables and are mapped using Entity Framework Core.

### ChannelPerformance

**Namespace:** `SolveoDashboardAssignment.Api.Entities`  
**File:** [`ChannelPerformance.cs`](../SolveoDashboardAssignment.Api/Entities/ChannelPerformance.cs)

Represents monthly performance metrics for marketing channels.

#### Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Id` | int | Yes | Primary key (auto-generated) |
| `Month` | DateTime | Yes | Month of the performance data |
| `Channel` | string | Yes | Marketing channel name (e.g., "Organic Search", "Paid Search") |
| `Sessions` | int | Yes | Total number of sessions for the channel |
| `Signups` | int | Yes | Total number of signups from the channel |
| `ConversionRate` | decimal | Yes | Conversion rate percentage (signups/sessions * 100) |
| `AvgSessionDurationSec` | int | Yes | Average session duration in seconds |
| `BounceRate` | decimal | Yes | Bounce rate percentage |
| `PagesPerSession` | decimal | Yes | Average number of pages viewed per session |

#### Example

```json
{
  "id": 1,
  "month": "2025-01-01T00:00:00Z",
  "channel": "Organic Search",
  "sessions": 25000,
  "signups": 850,
  "conversionRate": 3.4,
  "avgSessionDurationSec": 180,
  "bounceRate": 42.5,
  "pagesPerSession": 3.2
}
```

#### Database Table

**Table Name:** `ChannelPerformances`

**Indexes:**
- Primary key on `Id`
- Recommended: Index on `Month` and `Channel` for query performance

---

### KeywordPerformance

**Namespace:** `SolveoDashboardAssignment.Api.Entities`  
**File:** [`KeywordPerformance.cs`](../SolveoDashboardAssignment.Api/Entities/KeywordPerformance.cs)

Represents keyword performance metrics comparing 2024 and 2025 data.

#### Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Id` | int | Yes | Primary key (auto-generated) |
| `Keyword` | string | Yes | Keyword phrase |
| `Category` | string | Yes | Keyword category (e.g., "Product", "Feature", "Comparison") |
| `Traffic2024` | int | Yes | Traffic volume in 2024 |
| `Traffic2025` | int | Yes | Traffic volume in 2025 |
| `TrafficChangePct` | decimal | Yes | Year-over-year traffic change percentage |
| `Position2024` | int | Yes | Search engine ranking position in 2024 |
| `Position2025` | int | Yes | Search engine ranking position in 2025 |
| `PositionChange` | int | Yes | Change in position (positive = improvement) |
| `Signups2024` | int | Yes | Number of signups in 2024 |
| `Signups2025` | int | Yes | Number of signups in 2025 |
| `ConversionRate2024` | decimal | Yes | Conversion rate in 2024 (%) |
| `ConversionRate2025` | decimal | Yes | Conversion rate in 2025 (%) |
| `AiOverviewTriggered` | bool | Yes | Whether AI Overview is triggered for this keyword |
| `DifficultyScore` | int | Yes | SEO difficulty score (0-100) |
| `CpcUsd` | decimal | Yes | Cost per click in USD |

#### Example

```json
{
  "id": 1,
  "keyword": "project management software",
  "category": "Product",
  "traffic2024": 4500,
  "traffic2025": 5200,
  "trafficChangePct": 15.5,
  "position2024": 5,
  "position2025": 3,
  "positionChange": 2,
  "signups2024": 126,
  "signups2025": 166,
  "conversionRate2024": 2.8,
  "conversionRate2025": 3.2,
  "aiOverviewTriggered": false,
  "difficultyScore": 75,
  "cpcUsd": 12.50
}
```

#### Database Table

**Table Name:** `KeywordPerformances`

**Indexes:**
- Primary key on `Id`
- Recommended: Index on `Keyword` and `Category` for query performance

---

### MonthlyMetrics

**Namespace:** `SolveoDashboardAssignment.Api.Entities`  
**File:** [`MonthlyMetrics.cs`](../SolveoDashboardAssignment.Api/Entities/MonthlyMetrics.cs)

Represents monthly business metrics including traffic, conversions, and MRR.

#### Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Id` | int | Yes | Primary key (auto-generated) |
| `Month` | DateTime | Yes | Month of the metrics |
| `WebsiteTraffic` | int | Yes | Total website traffic for the month |
| `UniqueSignups` | int | Yes | Number of unique signups |
| `TrialsStarted` | int | Yes | Number of trials started |
| `PaidConversions` | int | Yes | Number of paid conversions |
| `MrrUsd` | decimal | Yes | Monthly Recurring Revenue in USD |
| `ChurnRate` | decimal | Yes | Customer churn rate percentage |
| `SignupToTrialRate` | decimal | Yes | Signup to trial conversion rate (%) |
| `TrialToPaidRate` | decimal | Yes | Trial to paid conversion rate (%) |
| `NetNewMrr` | decimal | Yes | Net new MRR for the month |
| `ExpansionMrr` | decimal | Yes | MRR from expansions/upgrades |
| `ChurnedMrr` | decimal | Yes | MRR lost to churn |

#### Example

```json
{
  "id": 1,
  "month": "2025-01-01T00:00:00Z",
  "websiteTraffic": 50000,
  "uniqueSignups": 1200,
  "trialsStarted": 542,
  "paidConversions": 122,
  "mrrUsd": 125000.00,
  "churnRate": 3.5,
  "signupToTrialRate": 45.2,
  "trialToPaidRate": 22.5,
  "netNewMrr": 10000.00,
  "expansionMrr": 5000.00,
  "churnedMrr": 4000.00
}
```

#### Database Table

**Table Name:** `MonthlyMetrics`

**Indexes:**
- Primary key on `Id`
- Recommended: Unique index on `Month` for data integrity

---

### RegionalPerformance

**Namespace:** `SolveoDashboardAssignment.Api.Entities`  
**File:** [`RegionalPerformance.cs`](../SolveoDashboardAssignment.Api/Entities/RegionalPerformance.cs)

Represents monthly performance metrics by geographic region.

#### Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Id` | int | Yes | Primary key (auto-generated) |
| `Region` | string | Yes | Geographic region (e.g., "North America", "Europe") |
| `Country` | string | Yes | Country name |
| `City` | string | Yes | City name |
| `Month` | DateTime | Yes | Month of the performance data |
| `OrganicTraffic` | int | Yes | Organic traffic volume |
| `PaidTraffic` | int | Yes | Paid traffic volume |
| `TotalTraffic` | int | Yes | Total traffic (organic + paid) |
| `TrialsStarted` | int | Yes | Number of trials started |
| `PaidConversions` | int | Yes | Number of paid conversions |
| `TrialToPaidRate` | decimal | Yes | Trial to paid conversion rate (%) |
| `MrrUsd` | decimal | Yes | Monthly Recurring Revenue in USD |
| `CacUsd` | decimal | Yes | Customer Acquisition Cost in USD |
| `LtvUsd` | decimal | Yes | Customer Lifetime Value in USD |

#### Example

```json
{
  "id": 1,
  "region": "North America",
  "country": "USA",
  "city": "New York",
  "month": "2025-01-01T00:00:00Z",
  "organicTraffic": 15000,
  "paidTraffic": 8000,
  "totalTraffic": 23000,
  "trialsStarted": 150,
  "paidConversions": 35,
  "trialToPaidRate": 23.33,
  "mrrUsd": 8500.00,
  "cacUsd": 120.00,
  "ltvUsd": 450.00
}
```

#### Database Table

**Table Name:** `RegionalPerformances`

**Indexes:**
- Primary key on `Id`
- Recommended: Index on `Region`, `Country`, `City`, and `Month` for query performance

---

## Data Transfer Objects (DTOs)

DTOs are used to transfer data between the API and clients, providing a clean separation from database entities.

### AlertDto

**Namespace:** `SolveoDashboardAssignment.Api.Dtos`  
**File:** [`AlertDto.cs`](../SolveoDashboardAssignment.Api/Dtos/AlertDto.cs)

Represents a performance alert detected by the system.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `AlertType` | string | Type of alert (see [AlertType](#alerttype) enum) |
| `Severity` | string | Severity level (see [AlertSeverity](#alertseverity) enum) |
| `Message` | string | Descriptive message about the alert |
| `Entity` | string | Entity that triggered the alert (keyword, channel, region, etc.) |
| `Value` | decimal | Current value of the metric |
| `Threshold` | decimal | Threshold value that was exceeded or not met |
| `RecommendedAction` | string | Recommended action to address the alert |
| `DetectedAt` | DateTime? | Timestamp when the alert was detected |

#### Example

```json
{
  "alertType": "HighTrafficLowConversion",
  "severity": "High",
  "message": "Keyword 'project management software' has high traffic (5000) but low conversion rate (1.2%)",
  "entity": "project management software",
  "value": 1.2,
  "threshold": 1.5,
  "recommendedAction": "Review landing page content and optimize conversion funnel",
  "detectedAt": "2025-01-28T10:30:00Z"
}
```

---

### ChannelMetricsDto

**Namespace:** `SolveoDashboardAssignment.Api.Dtos`  
**File:** [`ChannelMetricsDto.cs`](../SolveoDashboardAssignment.Api/Dtos/ChannelMetricsDto.cs)

Represents aggregated channel performance metrics.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Channel` | string | Marketing channel name |
| `ConversionRate` | decimal | Average conversion rate (%) |
| `TotalSessions` | int | Total sessions across all months |
| `TotalSignups` | int | Total signups across all months |
| `AverageSessionDuration` | int | Average session duration in seconds |
| `BounceRate` | decimal | Average bounce rate (%) |
| `PagesPerSession` | decimal | Average pages per session |
| `MonthCount` | int | Number of months included in aggregation |

#### Example

```json
{
  "channel": "Organic Search",
  "conversionRate": 3.4,
  "totalSessions": 300000,
  "totalSignups": 10200,
  "averageSessionDuration": 180,
  "bounceRate": 42.5,
  "pagesPerSession": 3.2,
  "monthCount": 12
}
```

---

### ChannelMonthlyDto

**Namespace:** `SolveoDashboardAssignment.Api.Dtos`  
**File:** [`ChannelMonthlyDto.cs`](../SolveoDashboardAssignment.Api/Dtos/ChannelMonthlyDto.cs)

Represents monthly channel performance data without aggregation.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Month` | string | Month in string format |
| `Channel` | string | Marketing channel name |
| `Sessions` | int | Number of sessions |
| `Signups` | int | Number of signups |
| `ConversionRate` | decimal | Conversion rate (%) |
| `AvgSessionDurationSec` | int | Average session duration in seconds |
| `BounceRate` | decimal | Bounce rate (%) |
| `PagesPerSession` | decimal | Pages per session |

#### Example

```json
{
  "month": "2025-01",
  "channel": "Organic Search",
  "sessions": 25000,
  "signups": 850,
  "conversionRate": 3.4,
  "avgSessionDurationSec": 180,
  "bounceRate": 42.5,
  "pagesPerSession": 3.2
}
```

---

### ImportStatisticsDto

**Namespace:** `SolveoDashboardAssignment.Api.Dtos`  
**File:** [`ImportStatisticsDto.cs`](../SolveoDashboardAssignment.Api/Dtos/ImportStatisticsDto.cs)

Represents statistics about an Excel import operation.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `SheetStats` | Dictionary<string, SheetStatistics> | Statistics for each sheet |
| `TotalRowsAllSheets` | int | Total rows across all sheets |
| `TotalImportedAllSheets` | int | Total imported rows across all sheets |
| `TotalSkippedAllSheets` | int | Total skipped rows across all sheets |
| `GlobalErrors` | List<string> | Global errors not specific to any sheet |

#### SheetStatistics Properties

| Property | Type | Description |
|----------|------|-------------|
| `SheetName` | string | Name of the Excel sheet |
| `TotalRows` | int | Total rows in the sheet |
| `ImportedRows` | int | Successfully imported rows |
| `SkippedRows` | int | Skipped rows (due to errors) |
| `Errors` | List<string> | List of error messages |

#### Example

```json
{
  "sheetStats": {
    "Keyword Performance": {
      "sheetName": "Keyword Performance",
      "totalRows": 100,
      "importedRows": 95,
      "skippedRows": 5,
      "errors": [
        "Row 10: Missing required column 'keyword'"
      ]
    }
  },
  "totalRowsAllSheets": 362,
  "totalImportedAllSheets": 355,
  "totalSkippedAllSheets": 7,
  "globalErrors": []
}
```

---

### KeywordMetricsDto

**Namespace:** `SolveoDashboardAssignment.Api.Dtos`  
**File:** [`KeywordMetricsDto.cs`](../SolveoDashboardAssignment.Api/Dtos/KeywordMetricsDto.cs)

Represents keyword performance metrics with year-over-year comparison.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Keyword` | string | Keyword phrase |
| `Category` | string | Keyword category |
| `TrafficChangeYoY` | decimal | Year-over-year traffic change (%) |
| `Traffic2024` | int | Traffic in 2024 |
| `Traffic2025` | int | Traffic in 2025 |
| `ConversionRate2024` | decimal | Conversion rate in 2024 (%) |
| `ConversionRate2025` | decimal | Conversion rate in 2025 (%) |
| `Position2024` | int | Search position in 2024 |
| `Position2025` | int | Search position in 2025 |
| `PositionChange` | int | Position change (positive = improvement) |
| `AiOverviewTriggered` | string | "Yes" or "No" |

#### Example

```json
{
  "keyword": "project management software",
  "category": "Product",
  "trafficChangeYoY": 15.5,
  "traffic2024": 4500,
  "traffic2025": 5200,
  "conversionRate2024": 2.8,
  "conversionRate2025": 3.2,
  "position2024": 5,
  "position2025": 3,
  "positionChange": 2,
  "aiOverviewTriggered": "No"
}
```

---

### MonthlyMetricsDto

**Namespace:** `SolveoDashboardAssignment.Api.Dtos`  
**File:** [`MonthlyMetricsDto.cs`](../SolveoDashboardAssignment.Api/Dtos/MonthlyMetricsDto.cs)

Represents monthly business metrics with calculated growth rates.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `LatestMrr` | decimal | Current month's MRR |
| `GrowthPercentageMoM` | decimal | Month-over-month growth percentage |
| `SignupToTrialPercentage` | decimal | Signup to trial conversion rate (%) |
| `TrialToPaidPercentage` | decimal | Trial to paid conversion rate (%) |
| `Month` | string | Month in string format |
| `PreviousMonthMrr` | decimal? | Previous month's MRR (nullable) |
| `WebsiteTraffic` | int | Total website traffic |
| `UniqueSignups` | int | Number of unique signups |
| `TrialsStarted` | int | Number of trials started |
| `PaidConversions` | int | Number of paid conversions |
| `ChurnRate` | decimal | Customer churn rate (%) |

#### Example

```json
{
  "latestMrr": 125000.00,
  "growthPercentageMoM": 8.5,
  "signupToTrialPercentage": 45.2,
  "trialToPaidPercentage": 22.5,
  "month": "2025-01",
  "previousMonthMrr": 115000.00,
  "websiteTraffic": 50000,
  "uniqueSignups": 1200,
  "trialsStarted": 542,
  "paidConversions": 122,
  "churnRate": 3.5
}
```

---

### MonthlyMrrDto

**Namespace:** `SolveoDashboardAssignment.Api.Dtos`  
**File:** [`MonthlyMrrDto.cs`](../SolveoDashboardAssignment.Api/Dtos/MonthlyMrrDto.cs)

Represents MRR data for a specific month.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Month` | int | Month number (1-12) |
| `Year` | int | Year |
| `MrrUsd` | decimal | Monthly Recurring Revenue in USD |

#### Example

```json
{
  "month": 1,
  "year": 2025,
  "mrrUsd": 125000.00
}
```

---

### RegionalMetricsDto

**Namespace:** `SolveoDashboardAssignment.Api.Dtos`  
**File:** [`RegionalMetricsDto.cs`](../SolveoDashboardAssignment.Api/Dtos/RegionalMetricsDto.cs)

Represents aggregated regional performance metrics.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Region` | string | Geographic region |
| `Country` | string | Country name |
| `City` | string | City name |
| `AverageTrialToPaidRate` | decimal | Average trial to paid conversion rate (%) |
| `TrafficTrendPercentage` | decimal | Traffic trend percentage |
| `CacLtvRatio` | decimal | CAC/LTV ratio |
| `TotalTraffic` | int | Total traffic across all months |
| `TotalConversions` | int | Total conversions across all months |
| `AverageCac` | decimal | Average Customer Acquisition Cost |
| `AverageLtv` | decimal | Average Lifetime Value |
| `MonthCount` | int | Number of months included in aggregation |

#### Example

```json
{
  "region": "North America",
  "country": "USA",
  "city": "New York",
  "averageTrialToPaidRate": 23.5,
  "trafficTrendPercentage": 12.3,
  "cacLtvRatio": 3.75,
  "totalTraffic": 276000,
  "totalConversions": 420,
  "averageCac": 120.00,
  "averageLtv": 450.00,
  "monthCount": 12
}
```

---

## Enumerations

### AlertSeverity

**Namespace:** `SolveoDashboardAssignment.Api.Enums`  
**File:** [`AlertSeverity.cs`](../SolveoDashboardAssignment.Api/Enums/AlertSeverity.cs)

Defines severity levels for performance alerts.

#### Values

| Value | Description |
|-------|-------------|
| `Low` | Informational alert - no immediate action required |
| `Medium` | Requires attention - should be reviewed soon |
| `High` | Requires immediate attention - action needed |
| `Critical` | Requires urgent action - significant impact on business |

#### Usage

```csharp
var severity = AlertSeverity.High;
```

---

### AlertType

**Namespace:** `SolveoDashboardAssignment.Api.Enums`  
**File:** [`AlertType.cs`](../SolveoDashboardAssignment.Api/Enums/AlertType.cs)

Defines types of performance alerts that can be detected.

#### Values

| Value | Description |
|-------|-------------|
| `HighTrafficLowConversion` | Keywords with high traffic but low conversion rates |
| `AiOverviewCannibalization` | Keywords affected by AI Overview cannibalization |
| `RegionalUnderperformance` | Regions with poor performance metrics |
| `SeasonalDip` | Seasonal dips in performance |
| `ChannelWaste` | Channels with wasteful spending (high sessions, low conversion) |
| `PoorCacLtvRatio` | Poor Customer Acquisition Cost to Lifetime Value ratio |
| `MrrDecline` | Monthly Recurring Revenue decline |

#### Usage

```csharp
var alertType = AlertType.HighTrafficLowConversion;
```

---

## Data Relationships

### Entity Relationships

```
MonthlyMetrics (1:N) ← Month → (N:1) ChannelPerformance
MonthlyMetrics (1:N) ← Month → (N:1) RegionalPerformance
```

### DTO to Entity Mapping

| DTO | Source Entity/Entities |
|-----|------------------------|
| `AlertDto` | Calculated from multiple entities |
| `ChannelMetricsDto` | Aggregated from `ChannelPerformance` |
| `ChannelMonthlyDto` | Mapped from `ChannelPerformance` |
| `KeywordMetricsDto` | Mapped from `KeywordPerformance` |
| `MonthlyMetricsDto` | Mapped from `MonthlyMetrics` |
| `MonthlyMrrDto` | Extracted from `MonthlyMetrics` |
| `RegionalMetricsDto` | Aggregated from `RegionalPerformance` |

---

## Validation Rules

### Common Validation

- All required string properties must not be null or empty
- All numeric properties must be >= 0
- Percentage values must be between 0 and 100
- DateTime values must be valid dates

### Entity-Specific Validation

#### ChannelPerformance
- `ConversionRate` should be between 0 and 100
- `BounceRate` should be between 0 and 100
- `PagesPerSession` should be > 0

#### KeywordPerformance
- `DifficultyScore` must be between 0 and 100
- `CpcUsd` must be >= 0
- Position values must be > 0

#### MonthlyMetrics
- All rate percentages must be between 0 and 100
- MRR values must be >= 0

#### RegionalPerformance
- `TrialToPaidRate` must be between 0 and 100
- `CacUsd` and `LtvUsd` must be > 0
- `TotalTraffic` should equal `OrganicTraffic + PaidTraffic`

---

## Best Practices

### Working with Entities

1. **Always use required properties** - Ensure all required fields are populated
2. **Use UTC for DateTime** - All DateTime values should be in UTC to avoid timezone issues
3. **Validate before saving** - Implement validation logic before persisting to database
4. **Use transactions** - Wrap multiple entity operations in transactions for data integrity

### Working with DTOs

1. **Map carefully** - Ensure proper mapping between entities and DTOs
2. **Include only necessary data** - DTOs should only contain data needed by the client
3. **Use meaningful names** - Property names should be clear and self-documenting
4. **Document calculations** - Clearly document any calculated fields in DTOs

### Performance Considerations

1. **Use projections** - Select only needed columns when querying
2. **Avoid N+1 queries** - Use eager loading with `.Include()` when needed
3. **Index frequently queried fields** - Add database indexes for better performance
4. **Cache aggregated data** - Consider caching for expensive aggregations
