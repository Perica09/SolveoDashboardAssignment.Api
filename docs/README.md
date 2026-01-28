# Solveo Dashboard API - Technical Documentation

## Overview

The Solveo Dashboard API is a comprehensive ASP.NET Core backend application designed to manage and analyze dashboard metrics for marketing performance, keyword tracking, channel analytics, and regional performance. The API provides robust data import capabilities, real-time performance alerts, and detailed analytics endpoints.

**Version:** 1.0  
**Framework:** .NET 10.0  
**Database:** PostgreSQL  
**Architecture:** RESTful API with MVC pattern

---

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Documentation](#documentation)
- [Project Structure](#project-structure)
- [Technology Stack](#technology-stack)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)
- [Development](#development)
- [Deployment](#deployment)
- [Contributing](#contributing)

---

## Features

### 📊 Data Import
- **Excel Import** - Import dashboard data from Excel files (.xlsx)
- **Multi-Sheet Processing** - Handles Keyword Performance, Channel Performance, Monthly Metrics, and Regional Performance
- **Validation & Error Reporting** - Comprehensive validation with detailed error messages
- **Batch Processing** - Efficient bulk data import with transaction support

### 🚨 Performance Alerts
- **High Traffic Low Conversion Detection** - Identifies keywords with optimization opportunities
- **AI Overview Cannibalization** - Detects keywords affected by AI Overview
- **Regional Underperformance** - Flags regions with poor CAC/LTV ratios
- **Seasonal Dips** - Identifies significant month-over-month declines
- **Channel Waste Detection** - Finds channels with high spend but low conversion
- **Severity-Based Prioritization** - Alerts sorted by Critical → High → Medium → Low

### 📈 Analytics & Metrics
- **Monthly Metrics** - MRR tracking, growth rates, conversion funnels
- **Regional Performance** - Geographic analysis with CAC/LTV ratios
- **Channel Analytics** - Marketing channel performance and ROI
- **Keyword Tracking** - Year-over-year keyword performance comparison
- **Trend Analysis** - Traffic trends, conversion rates, and growth patterns

### 🔧 Developer Features
- **RESTful API Design** - Clean, intuitive endpoint structure
- **Swagger Documentation** - Interactive API documentation
- **CORS Support** - Configured for Angular frontend integration
- **Comprehensive Logging** - Built-in error tracking and diagnostics
- **Entity Framework Core** - Modern ORM with migrations support

---

## Architecture

### Design Pattern
The application follows the **MVC (Model-View-Controller)** pattern with a clear separation of concerns:

```
┌─────────────────┐
│   Controllers   │  ← HTTP Request Handling
└────────┬────────┘
         │
┌────────▼────────┐
│    Services     │  ← Business Logic
└────────┬────────┘
         │
┌────────▼────────┐
│   Data Layer    │  ← Database Access (EF Core)
└────────┬────────┘
         │
┌────────▼────────┐
│   PostgreSQL    │  ← Data Storage
└─────────────────┘
```

### Layer Responsibilities

#### Controllers Layer
- HTTP request/response handling
- Input validation
- Route mapping
- Error handling and status codes

**Controllers:**
- [`AlertsController`](../SolveoDashboardAssignment.Api/Controllers/AlertsController.cs) - Performance alerts
- [`ExcelImportController`](../SolveoDashboardAssignment.Api/Controllers/ImportController.cs) - Data import
- [`MetricsController`](../SolveoDashboardAssignment.Api/Controllers/MetricsController.cs) - Analytics and metrics

#### Services Layer
- Business logic implementation
- Data aggregation and calculations
- Alert detection algorithms
- Data transformation

**Services:**
- [`AlertsService`](../SolveoDashboardAssignment.Api/Services/AlertsService.cs) - Alert detection and management
- [`ExcelImportService`](../SolveoDashboardAssignment.Api/Services/ExcelImportService.cs) - Excel file processing
- [`MetricsService`](../SolveoDashboardAssignment.Api/Services/MetricsService.cs) - Metrics retrieval and analysis

#### Data Layer
- Entity definitions
- Database context
- Migrations
- Data access operations

**Entities:**
- [`ChannelPerformance`](../SolveoDashboardAssignment.Api/Entities/ChannelPerformance.cs)
- [`KeywordPerformance`](../SolveoDashboardAssignment.Api/Entities/KeywordPerformance.cs)
- [`MonthlyMetrics`](../SolveoDashboardAssignment.Api/Entities/MonthlyMetrics.cs)
- [`RegionalPerformance`](../SolveoDashboardAssignment.Api/Entities/RegionalPerformance.cs)

---

## Getting Started

### Prerequisites

- **.NET 10.0 SDK** or later
- **PostgreSQL 12+** database server
- **Visual Studio 2022** or **VS Code** (recommended)
- **Git** for version control

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd SolveoDashboardAssignment
   ```

2. **Configure the database connection**
   
   Update the connection string in [`appsettings.json`](../SolveoDashboardAssignment.Api/appsettings.json):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=solveo_dashboard;Username=your_username;Password=your_password"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   cd SolveoDashboardAssignment.Api
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the API**
   - API Base URL: `http://localhost:5000/api`
   - Swagger UI: `http://localhost:5000/`

### Quick Start Example

**Import data from Excel:**
```bash
curl -X POST "http://localhost:5000/api/excelimport/import" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@dashboard_data.xlsx"
```

**Get all alerts:**
```bash
curl -X GET "http://localhost:5000/api/alerts"
```

**Get latest monthly metrics:**
```bash
curl -X GET "http://localhost:5000/api/metrics/monthly/latest"
```

---

## Documentation

### Complete Documentation

| Document | Description |
|----------|-------------|
| [**API Reference**](API-REFERENCE.md) | Complete API endpoint documentation with request/response examples |
| [**Services Documentation**](SERVICES.md) | Detailed service layer documentation with method signatures |
| [**Models & Data Structures**](MODELS.md) | Entity, DTO, and enum documentation |

### Quick Links

- **Controllers:** [AlertsController](../SolveoDashboardAssignment.Api/Controllers/AlertsController.cs), [ExcelImportController](../SolveoDashboardAssignment.Api/Controllers/ImportController.cs), [MetricsController](../SolveoDashboardAssignment.Api/Controllers/MetricsController.cs)
- **Services:** [AlertsService](../SolveoDashboardAssignment.Api/Services/AlertsService.cs), [ExcelImportService](../SolveoDashboardAssignment.Api/Services/ExcelImportService.cs), [MetricsService](../SolveoDashboardAssignment.Api/Services/MetricsService.cs)
- **Entities:** [ChannelPerformance](../SolveoDashboardAssignment.Api/Entities/ChannelPerformance.cs), [KeywordPerformance](../SolveoDashboardAssignment.Api/Entities/KeywordPerformance.cs), [MonthlyMetrics](../SolveoDashboardAssignment.Api/Entities/MonthlyMetrics.cs), [RegionalPerformance](../SolveoDashboardAssignment.Api/Entities/RegionalPerformance.cs)

---

## Project Structure

```
SolveoDashboardAssignment.Api/
├── Controllers/              # API Controllers
│   ├── AlertsController.cs
│   ├── ImportController.cs
│   └── MetricsController.cs
├── Services/                 # Business Logic Services
│   ├── AlertsService.cs
│   ├── ExcelImportService.cs
│   └── MetricsService.cs
├── Interfaces/               # Service Interfaces
│   ├── IAlertService.cs
│   ├── IExcelImportService.cs
│   └── IMetricsService.cs
├── Entities/                 # Database Entities
│   ├── ChannelPerformance.cs
│   ├── KeywordPerformance.cs
│   ├── MonthlyMetrics.cs
│   └── RegionalPerformance.cs
├── Dtos/                     # Data Transfer Objects
│   ├── AlertDto.cs
│   ├── ChannelMetricsDto.cs
│   ├── ImportStatisticsDto.cs
│   ├── KeywordMetricsDto.cs
│   ├── MonthlyMetricsDto.cs
│   └── RegionalMetricsDto.cs
├── Enums/                    # Enumerations
│   ├── AlertSeverity.cs
│   └── AlertType.cs
├── Data/                     # Database Context
│   └── AppDbContext.cs
├── Helpers/                  # Helper Classes
│   └── ImportServiceHelpers.cs
├── Migrations/               # EF Core Migrations
├── Program.cs               # Application Entry Point
└── appsettings.json         # Configuration
```

---

## Technology Stack

### Backend Framework
- **ASP.NET Core 10.0** - Modern web framework
- **C# 13** - Latest language features
- **Entity Framework Core** - ORM for database access

### Database
- **PostgreSQL** - Relational database
- **Npgsql** - PostgreSQL provider for EF Core

### Libraries & Packages
- **EPPlus** - Excel file processing
- **Swashbuckle** - Swagger/OpenAPI documentation
- **Microsoft.AspNetCore.OpenApi** - OpenAPI support

### Development Tools
- **Visual Studio 2022** / **VS Code**
- **Entity Framework Core Tools** - Migrations and scaffolding
- **Swagger UI** - Interactive API testing

---

## API Endpoints

### Alerts Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/alerts` | Get all alerts sorted by severity |
| GET | `/api/alerts/high-traffic-low-conversion` | Detect high traffic, low conversion keywords |
| GET | `/api/alerts/ai-overview-cannibalization` | Detect AI Overview cannibalization |
| GET | `/api/alerts/regional-underperformance` | Detect underperforming regions |
| GET | `/api/alerts/seasonal-dips` | Detect seasonal performance dips |
| GET | `/api/alerts/channel-waste` | Detect wasteful channel spending |

### Import Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/excelimport/import` | Import data from Excel file |

### Metrics Endpoints

#### Monthly Metrics
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/metrics/monthly/latest` | Get latest monthly metrics |
| GET | `/api/metrics/monthly` | Get monthly metrics range |
| GET | `/api/metrics/mrr-history` | Get MRR history |

#### Regional Performance
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/metrics/regional/all` | Get all regional data |
| GET | `/api/metrics/regional` | Get aggregated regional metrics |
| GET | `/api/metrics/regional/trial-to-paid` | Get trial-to-paid rates by region |
| GET | `/api/metrics/regional/traffic-trends` | Get traffic trends by region |
| GET | `/api/metrics/regional/cac-ltv` | Get CAC/LTV ratios by region |

#### Channel Performance
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/metrics/channels/all` | Get all channel performance data |
| GET | `/api/metrics/channels` | Get aggregated channel metrics |
| GET | `/api/metrics/channels/conversion-rates` | Get conversion rates by channel |

#### Keyword Performance
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/metrics/keywords` | Get keyword metrics |
| GET | `/api/metrics/keywords/traffic-change` | Get year-over-year traffic change |

**For detailed endpoint documentation, see [API Reference](API-REFERENCE.md)**

---

## Database Schema

### Tables

#### ChannelPerformances
Stores monthly performance metrics for marketing channels.

**Key Columns:** `Id`, `Month`, `Channel`, `Sessions`, `Signups`, `ConversionRate`

#### KeywordPerformances
Stores keyword performance metrics comparing 2024 and 2025 data.

**Key Columns:** `Id`, `Keyword`, `Category`, `Traffic2024`, `Traffic2025`, `AiOverviewTriggered`

#### MonthlyMetrics
Stores monthly business metrics including traffic, conversions, and MRR.

**Key Columns:** `Id`, `Month`, `MrrUsd`, `WebsiteTraffic`, `UniqueSignups`, `TrialsStarted`

#### RegionalPerformances
Stores monthly performance metrics by geographic region.

**Key Columns:** `Id`, `Region`, `Country`, `City`, `Month`, `TotalTraffic`, `CacUsd`, `LtvUsd`

### Relationships

```
MonthlyMetrics (1:N) ← Month → (N:1) ChannelPerformances
MonthlyMetrics (1:N) ← Month → (N:1) RegionalPerformances
```

**For detailed schema documentation, see [Models & Data Structures](MODELS.md)**

---

## Development

### Running in Development Mode

```bash
cd SolveoDashboardAssignment.Api
dotnet run --environment Development
```

**Development Features:**
- Swagger UI enabled at root URL
- Detailed error pages
- CORS enabled for `http://localhost:4200`

### Creating Migrations

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Running Tests

```bash
dotnet test
```

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and single-purpose

---

## Deployment

### Production Configuration

1. **Update connection string** in `appsettings.json`
2. **Set environment** to Production
3. **Apply migrations** to production database
4. **Configure CORS** for production frontend URL
5. **Enable HTTPS** redirection
6. **Set up logging** and monitoring

### Environment Variables

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<production-connection-string>
```

### Docker Deployment (Optional)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["SolveoDashboardAssignment.Api/SolveoDashboardAssignment.Api.csproj", "SolveoDashboardAssignment.Api/"]
RUN dotnet restore "SolveoDashboardAssignment.Api/SolveoDashboardAssignment.Api.csproj"
COPY . .
WORKDIR "/src/SolveoDashboardAssignment.Api"
RUN dotnet build "SolveoDashboardAssignment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolveoDashboardAssignment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolveoDashboardAssignment.Api.dll"]
```

---

## Contributing

### Development Workflow

1. **Fork the repository**
2. **Create a feature branch** (`git checkout -b feature/amazing-feature`)
3. **Commit your changes** (`git commit -m 'Add amazing feature'`)
4. **Push to the branch** (`git push origin feature/amazing-feature`)
5. **Open a Pull Request**

### Coding Standards

- Follow Microsoft C# coding conventions
- Write unit tests for new features
- Update documentation for API changes
- Ensure all tests pass before submitting PR

---

## Support & Resources

### Documentation
- [API Reference](API-REFERENCE.md) - Complete API documentation
- [Services Documentation](SERVICES.md) - Service layer details
- [Models & Data Structures](MODELS.md) - Entity and DTO documentation

### External Resources
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

---

## License

This project is proprietary software developed for Solveo.

---

## Contact

For questions or support, please contact the development team.

---

**Last Updated:** January 2025  
**Version:** 1.0
