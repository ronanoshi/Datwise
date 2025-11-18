# Datwise - Security & Safety Control Panel

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-Production%20Ready-brightgreen)

A professional Security & Safety Control Panel application built with ASP.NET Core Razor Pages and a RESTful API. Manage and track security and safety incidents with an intuitive dashboard, real-time statistics, and powerful filtering and sorting capabilities.

## ? Key Features

### Dashboard & UI
- **Real-time Statistics** - Total open issues, critical/high severity counts, monthly resolutions
- **Professional Design** - Modern, responsive UI with smooth animations
- **Dark/Light Mode** - Toggle theme with persistent user preference
- **Full-Screen Optimized** - Perfect fit within viewport, no scrolling needed
- **Column Sorting** - Click any table header to sort ascending/descending
- **Multi-Filter Support** - Comma-separated status/severity filtering (e.g., `status=Open,In Progress`)

### Data Management
- **Issue Tracking** - Create, read, update, and delete security/safety incidents
- **Advanced Filtering** - Status, severity, department, location filters with "contains" logic
- **Smart Statistics** - Automatic calculation of metrics and trends
- **SQLite Database** - Lightweight, file-based, auto-initialized on startup

### Technical Architecture
- **3-Tier Architecture** - Clean separation: UI ? API ? Data
- **RESTful API** - Complete REST endpoints with OpenAPI/Swagger documentation
- **Entity Framework Core** - ORM with migrations and automatic database creation
- **Comprehensive Tests** - 35 unit tests covering repositories and services
- **Async/Await Throughout** - All I/O operations are non-blocking

## ?? Quick Start

### Prerequisites
- .NET 9 SDK
- Visual Studio 2022 or VS Code

### Setup (5 minutes)

1. **Clone & Navigate**
```bash
git clone https://github.com/ronanoshi/Datwise.git
cd Datwise
```

2. **Start API** (Terminal 1)
```bash
cd Datwise.Api
dotnet run
# Listens on: http://localhost:53487 (dev) or https://localhost:53486
```

3. **Start WebForms** (Terminal 2)
```bash
cd Datwise.WebForms
dotnet run
# Listens on: http://localhost:53488 (dev) or https://localhost:53485
```

4. **Open Browser**
```
http://localhost:53488
```

## ?? API Endpoints

### Base URL
`http://localhost:53487` (development) or `https://localhost:53486` (production)

### Main Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/issues` | Get all issues with optional filtering & sorting |
| GET | `/api/issues/{id}` | Get issue by ID |
| GET | `/api/issues/statistics/summary` | Get dashboard statistics |
| POST | `/api/issues` | Create new issue |
| PUT | `/api/issues/{id}` | Update issue |
| DELETE | `/api/issues/{id}` | Delete issue |

### Query Parameters

**Status Filter** (comma-separated)
```
GET /api/issues?status=Open,In Progress
GET /api/issues?status=Open,In Progress,Resolved
```

**Severity Filter** (comma-separated)
```
GET /api/issues?severity=High,Critical
GET /api/issues?severity=Low,Medium,High,Critical
```

**Combined with Sorting**
```
GET /api/issues?status=Open,In Progress&severity=High,Critical&sort=-date
GET /api/issues?status=Open&sort=title
```

**Valid Sort Fields**: `id`, `title`, `severity`, `status`, `department`, `location`, `reportedby`, `date`
Use `-` prefix for descending: `sort=-date`

## ??? Architecture

### Project Structure
```
Datwise/
??? Datwise.Api/              # REST API (ASP.NET Core Web API)
??? Datwise.WebForms/         # UI (Razor Pages)
??? Datwise.Services/         # Business logic (.NET Standard 2.0)
??? Datwise.Data/             # Data access (EF Core)
??? Datwise.Contracts/        # Interfaces & DTOs
??? Datwise.Models/           # Domain models
??? Datwise.Tests/            # Unit tests (xUnit + Moq)
```

### Data Flow
```
User Form (Razor Pages)
    ?
ReportIssueModel (HTTP POST)
    ?
IssuesController (REST API)
    ?
IssueService (Business Logic)
    ?
IssueRepository (EF Core)
    ?
SQLite Database
```

## ?? UI/UX Features

### Dark/Light Mode
- Located in navbar toggle switch
- Persisted in browser localStorage
- Smooth CSS variable transitions
- Works across all pages

### Table Sorting
- Click column headers to sort
- Visual indicators (? ?) show direction
- URL persists sort state: `/?sort=-date`
- Supports all issue fields

### Statistics Dashboard
- Real-time metrics cards
- Color-coded severity indicators
- Open issues count (Open + In Progress)
- Critical/High/Medium/Low breakdowns
- Monthly resolution tracking

## ?? Testing

### Run All Tests
```bash
dotnet test Datwise.Tests
```

### Test Coverage
- **35 total tests** - All passing
- **17 repository tests** - CRUD, filtering, sorting
- **18 service tests** - Business logic, validation

### Test Framework
- **xUnit** - Modern test framework
- **Moq** - Mocking library for dependencies
- **In-Memory Database** - Isolated test data

## ?? Sample Data

Application comes pre-seeded with 5 issues:

| Issue | Severity | Status | Department |
|-------|----------|--------|-----------|
| Fire Extinguisher Missing | High | Open | Facilities |
| Wet Floor Hazard | Medium | Open | Operations |
| Equipment Malfunction | Critical | In Progress | Maintenance |
| First Aid Kit Empty | Low | Open | Facilities |
| Broken Handrail | High | Open | Maintenance |

## ?? Configuration

### Database
- **Type**: SQLite
- **File**: `Datwise.Api/datwise-dev.db`
- **Auto-Initialize**: Yes, on API startup
- **Schema**: `datwise-dev.db` created with Issues table

### API Configuration
```json
// appsettings.Development.json
{
  "ApiBaseUrl": "http://localhost:53487"
}
```

### WebForms Configuration
```json
// appsettings.Development.json
{
  "ApiBaseUrl": "http://localhost:53487"
}
```
*Note: Uses HTTP for development, HTTPS for production*

## ?? Security & Performance

### Security
- Input validation on all fields
- SQL injection protection via EF Core
- HTTPS in production
- Proper error handling

### Performance
- Database indexes on Status, Severity, ReportedDate
- Async/await for all I/O
- EF Core query optimization
- Lazy loading disabled to prevent N+1 queries

## ??? Development Workflow

### Adding a New Feature
1. Create DTO in `Datwise.Contracts`
2. Add repository method in `Datwise.Data`
3. Add service method in `Datwise.Services`
4. Expose via `IssuesController` endpoint
5. Add unit tests in `Datwise.Tests`
6. Update UI in `Datwise.WebForms`

### Modifying API Behavior
1. Update `IIssueService` interface
2. Implement in `IssueService`
3. Update `IIssueRepository` if needed
4. Update controller endpoint
5. Add/update tests

## ?? Deployment

### Development
```bash
dotnet run              # Uses launchSettings.json
http://localhost:53488  # WebForms
http://localhost:53487  # API
```

### Production
- Set `ASPNETCORE_ENVIRONMENT=Production`
- Configure valid HTTPS certificates
- Use production database (SQL Server recommended)
- Update `ApiBaseUrl` in appsettings
- Enable CORS if needed
- Configure logging and monitoring

## ?? Troubleshooting

### Port Already in Use
```powershell
.\cleanup-ports.ps1
```

### Database Not Creating
1. Delete `Datwise.Api/datwise-dev.db`
2. Restart API
3. Check console for initialization messages

### SSL Certificate Issues (Development)
- API automatically uses HTTP for localhost in dev
- WebForms uses HTTP in development
- Use `http://localhost:53488` in browser

### Form Submission Fails
1. Verify API is running on port 53487
2. Check appsettings.json ApiBaseUrl
3. Review console logs for HTTP errors
4. Test API with cURL: `curl http://localhost:53487/api/issues`

## ?? Code Examples

### Get Issues with Filtering
```csharp
var response = await _httpClient.GetAsync(
    "http://localhost:53487/api/issues?status=Open,In Progress&sort=-date");
var issues = JsonSerializer.Deserialize<List<IssueViewModel>>(
    await response.Content.ReadAsStringAsync());
```

### Create New Issue
```json
POST http://localhost:53487/api/issues
Content-Type: application/json

{
  "title": "Safety Concern",
  "description": "Issue details",
  "severity": "High",
  "status": "Open",
  "reportedBy": "John Doe",
  "department": "Operations",
  "location": "Building A"
}
```

### Filter Multiple Statuses
```bash
curl "http://localhost:53487/api/issues?status=Open,In%20Progress&severity=High,Critical"
```

## ?? Support

For issues:
1. Check console output for detailed error messages
2. Verify API and WebForms are both running
3. Ensure database file exists: `Datwise.Api/datwise-dev.db`
4. Review test files for usage examples

## ?? License

MIT License - See LICENSE file for details

---

**Technology Stack**
- .NET 9 (API, WebForms)
- .NET Standard 2.0 (Services, Contracts)
- Entity Framework Core
- SQLite
- Bootstrap 5
- xUnit & Moq

**Status**: ? Production Ready | **Tests**: ? 35/35 Passing | **Build**: ? Successful
