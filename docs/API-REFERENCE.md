# API Reference

## Overview

The Solveo Dashboard API provides endpoints for managing dashboard metrics, importing data from Excel files, and detecting performance alerts. The API is built with ASP.NET Core and uses PostgreSQL as the database.

**Base URL:** `http://localhost:5000/api` (Development)

**API Version:** 1.0

---

## Table of Contents

- [Authentication](#authentication)
- [Controllers](#controllers)
  - [Alerts Controller](#alerts-controller)
  - [Excel Import Controller](#excel-import-controller)
  - [Metrics Controller](#metrics-controller)
- [Response Formats](#response-formats)
- [Error Handling](#error-handling)

---

## Authentication

Currently, the API does not require authentication. All endpoints are publicly accessible.

---

## Controllers

### Alerts Controller

**Base Route:** `/api/alerts`

The Alerts Controller provides endpoints for detecting and retrieving performance alerts across various metrics including keywords, channels, regions, and seasonal trends.

#### Get All Alerts

Retrieves all alerts from all detection methods sorted by severity (Critical → High → Medium → Low).

**Endpoint:** `GET /api/alerts`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Filter alerts from this date onwards |
| `endDate` | DateTime | No | null | Filter alerts up to this date |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

```json
"An error occurred while retrieving alerts"
```

---

#### Get High Traffic Low Conversion Alerts

Detects keywords with high traffic but low conversion rates, indicating potential optimization opportunities.

**Endpoint:** `GET /api/alerts/high-traffic-low-conversion`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `minTraffic` | int | No | 2000 | Minimum traffic threshold |
| `maxConversion` | decimal | No | 1.5 | Maximum conversion rate threshold (%) |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

---

#### Get AI Overview Cannibalization Alerts

Detects keywords affected by AI Overview cannibalization (traffic decline with AI Overview triggered).

**Endpoint:** `GET /api/alerts/ai-overview-cannibalization`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `minDeclinePercentage` | decimal | No | 10.0 | Minimum traffic decline percentage threshold |

**Response:** `200 OK`

```json
[
  {
    "alertType": "AiOverviewCannibalization",
    "severity": "Critical",
    "message": "Keyword 'best CRM tools' affected by AI Overview with -25% traffic decline",
    "entity": "best CRM tools",
    "value": -25.0,
    "threshold": -10.0,
    "recommendedAction": "Optimize content for AI Overview or target alternative keywords",
    "detectedAt": "2025-01-28T10:30:00Z"
  }
]
```

**Error Response:** `500 Internal Server Error`

---

#### Get Regional Underperformance Alerts

Detects underperforming regions based on CAC/LTV ratio.

**Endpoint:** `GET /api/alerts/regional-underperformance`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Filter from this date onwards |
| `endDate` | DateTime | No | null | Filter up to this date |

**Response:** `200 OK`

```json
[
  {
    "alertType": "RegionalUnderperformance",
    "severity": "High",
    "message": "Region 'North America - USA - New York' has poor CAC/LTV ratio of 0.85",
    "entity": "North America - USA - New York",
    "value": 0.85,
    "threshold": 3.0,
    "recommendedAction": "Review marketing spend and customer acquisition strategies for this region",
    "detectedAt": "2025-01-28T10:30:00Z"
  }
]
```

**Error Response:** `500 Internal Server Error`

---

#### Get Seasonal Dips Alerts

Detects seasonal dips in performance (significant month-over-month declines).

**Endpoint:** `GET /api/alerts/seasonal-dips`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Filter from this date onwards |
| `endDate` | DateTime | No | null | Filter up to this date |

**Response:** `200 OK`

```json
[
  {
    "alertType": "SeasonalDip",
    "severity": "Medium",
    "message": "MRR declined by 15% in December 2024",
    "entity": "December 2024",
    "value": -15.0,
    "threshold": -10.0,
    "recommendedAction": "Investigate seasonal factors and plan promotional campaigns",
    "detectedAt": "2025-01-28T10:30:00Z"
  }
]
```

**Error Response:** `500 Internal Server Error`

---

#### Get Channel Waste Alerts

Detects channels with wasteful spending (high sessions but low conversion rates).

**Endpoint:** `GET /api/alerts/channel-waste`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `maxConversion` | decimal | No | 2.0 | Maximum conversion rate threshold (%) |
| `minSessions` | int | No | 10000 | Minimum sessions threshold |
| `channelsToCheck` | List<string> | No | null | Specific channels to check |
| `startDate` | DateTime | No | null | Filter from this date onwards |
| `endDate` | DateTime | No | null | Filter up to this date |

**Response:** `200 OK`

```json
[
  {
    "alertType": "ChannelWaste",
    "severity": "High",
    "message": "Channel 'Paid Search' has high sessions (50000) but low conversion rate (1.5%)",
    "entity": "Paid Search",
    "value": 1.5,
    "threshold": 2.0,
    "recommendedAction": "Review ad targeting and landing page optimization for this channel",
    "detectedAt": "2025-01-28T10:30:00Z"
  }
]
```

**Error Response:** `500 Internal Server Error`

---

### Excel Import Controller

**Base Route:** `/api/excelimport`

The Excel Import Controller handles importing dashboard data from Excel files (.xlsx format).

#### Import Excel File

Imports data from an Excel file containing multiple sheets with dashboard metrics.

**Endpoint:** `POST /api/excelimport/import`

**Content-Type:** `multipart/form-data`

**Request Body:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `file` | IFormFile | Yes | Excel file (.xlsx) containing dashboard data |

**Expected Excel Sheets:**

1. **Keyword Performance** - Keyword metrics comparing 2024 and 2025 data
2. **Channel Performance** - Monthly performance metrics for marketing channels
3. **Monthly Metrics** - Monthly business metrics including traffic, conversions, and MRR
4. **Regional Performance** - Monthly performance metrics by geographic region

**Response:** `200 OK`

```json
{
  "sheetStats": {
    "Keyword Performance": {
      "sheetName": "Keyword Performance",
      "totalRows": 100,
      "importedRows": 95,
      "skippedRows": 5,
      "errors": [
        "Row 10: Missing required column 'keyword'",
        "Row 25: Invalid data format for 'traffic2024'"
      ]
    },
    "Channel Performance": {
      "sheetName": "Channel Performance",
      "totalRows": 50,
      "importedRows": 50,
      "skippedRows": 0,
      "errors": []
    },
    "Monthly Metrics": {
      "sheetName": "Monthly Metrics",
      "totalRows": 12,
      "importedRows": 12,
      "skippedRows": 0,
      "errors": []
    },
    "Regional Performance": {
      "sheetName": "Regional Performance",
      "totalRows": 200,
      "importedRows": 198,
      "skippedRows": 2,
      "errors": [
        "Row 45: Missing required column 'region'"
      ]
    }
  },
  "totalRowsAllSheets": 362,
  "totalImportedAllSheets": 355,
  "totalSkippedAllSheets": 7,
  "globalErrors": []
}
```

**Error Responses:**

- `400 Bad Request` - No file uploaded
```json
"No file uploaded"
```

- `400 Bad Request` - Invalid file type
```json
"Only .xlsx files are supported"
```

- `400 Bad Request` - File too large
```json
"File size exceeds 10MB limit"
```

---

### Metrics Controller

**Base Route:** `/api/metrics`

The Metrics Controller provides endpoints for retrieving dashboard metrics and analytics across monthly metrics, regional performance, channel performance, and keyword performance.

---

#### Monthly Metrics Endpoints

##### Get Latest Monthly Metrics

Retrieves the most recent monthly metrics.

**Endpoint:** `GET /api/metrics/monthly/latest`

**Response:** `200 OK`

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

**Error Responses:**

- `404 Not Found` - No data available
```json
"No monthly metrics found"
```

- `500 Internal Server Error`

---

##### Get Monthly Metrics Range

Retrieves monthly metrics within a specified date range.

**Endpoint:** `GET /api/metrics/monthly`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

---

##### Get MRR History

Retrieves MRR (Monthly Recurring Revenue) history for the specified number of months.

**Endpoint:** `GET /api/metrics/mrr-history`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `months` | int | No | 12 | Number of months to retrieve |

**Response:** `200 OK`

```json
[
  {
    "month": 1,
    "year": 2025,
    "mrrUsd": 125000.00
  },
  {
    "month": 12,
    "year": 2024,
    "mrrUsd": 115000.00
  }
]
```

**Error Response:** `500 Internal Server Error`

---

#### Regional Performance Endpoints

##### Get All Regional Data

Retrieves all regional performance records without aggregation.

**Endpoint:** `GET /api/metrics/regional/all`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |
| `regions` | List<string> | No | null | Filter by specific regions |
| `countries` | List<string> | No | null | Filter by specific countries |
| `cities` | List<string> | No | null | Filter by specific cities |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

---

##### Get Regional Metrics

Retrieves aggregated regional performance metrics.

**Endpoint:** `GET /api/metrics/regional`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |
| `regions` | List<string> | No | null | Filter by specific regions |
| `countries` | List<string> | No | null | Filter by specific countries |
| `cities` | List<string> | No | null | Filter by specific cities |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

---

##### Get Average Trial-to-Paid by Region

Retrieves average trial-to-paid conversion rate by region.

**Endpoint:** `GET /api/metrics/regional/trial-to-paid`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |

**Response:** `200 OK`

```json
{
  "North America": 23.5,
  "Europe": 21.8,
  "Asia": 19.2
}
```

**Error Response:** `500 Internal Server Error`

---

##### Get Traffic Trends by Region

Retrieves traffic trends by region.

**Endpoint:** `GET /api/metrics/regional/traffic-trends`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |

**Response:** `200 OK`

```json
{
  "North America": 12.3,
  "Europe": 8.7,
  "Asia": 15.2
}
```

**Error Response:** `500 Internal Server Error`

---

##### Get CAC/LTV Ratio by Region

Retrieves Customer Acquisition Cost to Lifetime Value ratio by region.

**Endpoint:** `GET /api/metrics/regional/cac-ltv`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |
| `regions` | List<string> | No | null | Filter by specific regions |

**Response:** `200 OK`

```json
{
  "North America": 3.75,
  "Europe": 3.25,
  "Asia": 2.85
}
```

**Error Response:** `500 Internal Server Error`

---

#### Channel Performance Endpoints

##### Get All Channel Performance

Retrieves all channel performance records (monthly, no aggregation).

**Endpoint:** `GET /api/metrics/channels/all`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |
| `channels` | List<string> | No | null | Filter by specific channels |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

---

##### Get Channel Metrics

Retrieves aggregated channel performance metrics.

**Endpoint:** `GET /api/metrics/channels`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |
| `channels` | List<string> | No | null | Filter by specific channels |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

---

##### Get Conversion Rates by Channel

Retrieves conversion rates by channel.

**Endpoint:** `GET /api/metrics/channels/conversion-rates`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `startDate` | DateTime | No | null | Start date for filtering |
| `endDate` | DateTime | No | null | End date for filtering |

**Response:** `200 OK`

```json
{
  "Organic Search": 3.4,
  "Paid Search": 2.8,
  "Social Media": 2.1,
  "Email": 4.5
}
```

**Error Response:** `500 Internal Server Error`

---

#### Keyword Performance Endpoints

##### Get Keyword Metrics

Retrieves keyword performance metrics with optional filters.

**Endpoint:** `GET /api/metrics/keywords`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `categories` | List<string> | No | null | Filter by specific categories |
| `minTraffic` | int | No | null | Minimum traffic threshold |
| `maxTraffic` | int | No | null | Maximum traffic threshold |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

---

##### Get Traffic Change Year-over-Year

Retrieves year-over-year traffic change for keywords.

**Endpoint:** `GET /api/metrics/keywords/traffic-change`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `minChangePercentage` | decimal | No | null | Minimum change percentage threshold |
| `categories` | List<string> | No | null | Filter by specific categories |

**Response:** `200 OK`

```json
[
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
]
```

**Error Response:** `500 Internal Server Error`

---

## Response Formats

### Success Response

All successful responses return a `200 OK` status code with a JSON body containing the requested data.

### Error Response

Error responses include an appropriate HTTP status code and a descriptive error message:

- `400 Bad Request` - Invalid request parameters or missing required fields
- `404 Not Found` - Requested resource not found
- `500 Internal Server Error` - Server-side error occurred

---

## Error Handling

All endpoints implement comprehensive error handling:

1. **Validation Errors** - Return `400 Bad Request` with descriptive error messages
2. **Not Found Errors** - Return `404 Not Found` when resources don't exist
3. **Server Errors** - Return `500 Internal Server Error` with generic error messages (detailed errors are logged server-side)

All errors are logged using the built-in ASP.NET Core logging framework for debugging and monitoring purposes.

---

## CORS Configuration

The API is configured to allow cross-origin requests from:
- `http://localhost:4200` (Angular development server)

All headers and HTTP methods are allowed for the configured origin.

---

## Swagger Documentation

Interactive API documentation is available via Swagger UI in development mode:

**URL:** `http://localhost:5000/` (when running in development)

The Swagger UI provides:
- Interactive API testing
- Request/response examples
- Schema definitions
- Parameter descriptions
