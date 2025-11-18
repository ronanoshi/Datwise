# Datwise - Security & Safety Control Panel

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-Active%20Development-brightgreen)

A modern, professional Security & Safety Control Panel application built with ASP.NET Core, Razor Pages, and a RESTful API. Manage and track security and safety incidents with an intuitive dashboard, real-time statistics, and powerful filtering and sorting capabilities.

## ?? Features

### Dashboard
- **Real-time Statistics** - Total open issues, critical/high severity counts, monthly resolutions
- **Professional UI** - Modern, responsive design with smooth animations
- **Dark Mode** - Toggle between light and dark themes with persistent settings
- **Full-Screen Optimized** - Fits perfectly in viewport without scrolling

### Data Management
- **Issue Tracking** - Create, read, update, and delete security/safety issues
- **Advanced Filtering** - Filter by status, severity, department, location, and more
- **Multi-Column Sorting** - Click any column header to sort ascending/descending
- **Smart Statistics** - Automatic calculation of issue metrics and trends

### Technical Features
- **RESTful API** - Complete REST API with filtering, sorting, and pagination support
- **SQLite Database** - Lightweight, file-based database with automatic initialization
- **Clean Architecture** - 3-tier architecture with separation of concerns
- **Comprehensive Testing** - Unit tests for repositories and services
- **Dark/Light Mode** - CSS variable-based theming system

## ?? Quick Start

### Prerequisites
- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Visual Studio 2022** or **Visual Studio Code**
- **Git**

### Installation

1. **Clone the Repository**
```bash
git clone https://github.com/ronanoshi/Datwise.git
cd Datwise
```

2. **Clean Up Ports** (if previously run)
```powershell
.\cleanup-ports.ps1
```

3. **Start the API** (Terminal 1)
```bash
cd Datwise.Api
dotnet run
```
Expected output:
```
? Database created/verified successfully
? Test data seeded successfully
Now listening on: https://localhost:53486
```

4. **Start WebForms** (Terminal 2)
```bash
cd Datwise.WebForms
dotnet run
```

5. **Open in Browser**
```
https://localhost:53485
```

## ?? Sample Data

The application comes pre-populated with 5 security/safety issues:

| Title | Severity | Status | Department |
|-------|----------|--------|------------|
| Fire Extinguisher Missing | High | Open | Facilities |
| Wet Floor Hazard | Medium | Open | Operations |
| Equipment Malfunction | Critical | In Progress | Maintenance |
| First Aid Kit Empty | Low | Open | Facilities |
| Broken Handrail | High | Open | Maintenance |

## ?? API Endpoints

### Base URL
```
https://localhost:53486/api/issues
```

### Endpoints

#### Get Issues with Filtering and Sorting
```http
GET /api/issues?status=Open&severity=High&sort=-date
```

**Query Parameters:**
- `status` - Filter by status (Open, In Progress, Resolved)
- `severity` - Filter by severity (Low, Medium, High, Critical)
- `sort` - Sort field with optional `-` prefix for descending
  - Valid fields: `id`, `title`, `severity`, `status`, `department`, `location`, `reportedby`, `date`
  - Examples: `sort=title`, `sort=-date`, `sort=severity`

#### Get Open Issues
```http
GET /api/issues/open?sort=-reporteddate
```

#### Get Issues by Severity
```http
GET /api/issues/severity/Critical
```

#### Get Issue by ID
```http
GET /api/issues/1
```

#### Get Statistics
```http
GET /api/issues/statistics/summary
```

#### Create Issue
```http
POST /api/issues
Content-Type: application/json

{
  "title": "Safety Concern",
  "description": "Detailed description",
  "severity": "High",
  "status": "Open",
  "reportedBy": "John Doe",
  "department": "Facilities",
  "location": "Building A"
}
```

#### Update Issue
```http
PUT /api/issues/1
Content-Type: application/json

{
  "id": 1,
  "title": "Updated Title",
  "description": "Updated description",
  "severity": "Critical",
  "status": "In Progress",
  "reportedBy": "Jane Doe",
  "department": "Facilities",
  "location": "Building A"
}
```

#### Delete Issue
```http
DELETE /api/issues/1
```

## ?? UI Features

### Dashboard
- **Statistics Cards** - Quick overview of key metrics with color coding
- **Sortable Table** - Click column headers to sort data
- **Visual Indicators** - ? for ascending, ? for descending sort
- **Empty States** - Friendly messaging when no data exists
- **Responsive Design** - Works on desktop, tablet, and mobile

### Themes
- **Light Mode** - Clean white interface for daytime use
- **Dark Mode** - Easy on the eyes for extended use

### Accessibility
- Keyboard navigable table sorting
- Color-coded severity and status badges
- ARIA labels for accessibility
- Semantic HTML structure

## ?? Project Structure

```
Datwise/
??? Datwise.Api/                    # ASP.NET Core Web API
?   ??? Controllers/
?   ?   ??? IssuesController.cs    # REST API endpoints
?   ??? Program.cs                 # Configuration
?   ??? appsettings.json           # Settings
?   ??? datwise-dev.db             # SQLite database
?
??? Datwise.WebForms/              # Razor Pages UI
?   ??? Pages/
?   ?   ??? Index.cshtml           # Dashboard
?   ?   ??? ReportIssue.cshtml     # Report form
?   ?   ??? Shared/
?   ?       ??? _Layout.cshtml     # Master layout
?   ??? Models/
?   ?   ??? ControlPanelViewModel.cs
?   ?   ??? IssueViewModel.cs
?   ?   ??? IssueStatisticsViewModel.cs
?   ??? wwwroot/
?   ?   ??? css/
?   ?   ?   ??? site.css           # Theme and styles
?   ?   ??? js/
?   ?       ??? theme-toggle.js    # Dark/light mode
?   ?       ??? table-sorting.js   # Column sorting
?   ??? Program.cs                 # Configuration
?
??? Datwise.Services/              # Business Logic
?   ??? IIssueService.cs           # Service interface
?   ??? IssueService.cs            # Service implementation
?
??? Datwise.Data/                  # Data Access Layer
?   ??? DatwiseDbContext.cs        # EF Core DbContext
?   ??? IssueRepository.cs         # Data repository
?   ??? DatabaseSeeder.cs          # Test data seeding
?   ??? initialize-database.sql    # Database schema
?
??? Datwise.Contracts/             # Interfaces & DTOs
?   ??? IIssueRepository.cs        # Repository interface
?   ??? IssueStatistics.cs         # Statistics DTO
?
??? Datwise.Models/                # Domain Models
?   ??? Issue.cs                   # Issue entity
?
??? Datwise.Tests/                 # Unit Tests
?   ??? IssueRepositoryTests.cs
?   ??? IssueServiceTests.cs
?
??? Documentation/
    ??? README.md                  # This file
    ??? QUICKSTART.md              # Getting started guide
    ??? DATABASE_INITIALIZATION.md # Database setup details
```

## ??? Architecture

```
???????????????????????????????????????????
?     WebForms UI (Razor Pages)           ?
?  https://localhost:53485                ?
?  - Dashboard Display                    ?
?  - Report Issue Form                    ?
???????????????????????????????????????????
               ? HTTP Client (JSON)
               ?
????????????????????????????????????????????
?  REST API (ASP.NET Core)                 ?
?  https://localhost:53486                 ?
?  - IssuesController                      ?
?  - Filtering & Sorting                   ?
????????????????????????????????????????????
               ? EF Core
               ?
????????????????????????????????????????????
?  Business Logic Layer                    ?
?  - IssueService                          ?
?  - Validation & Processing               ?
????????????????????????????????????????????
               ? EF Core
               ?
????????????????????????????????????????????
?  Data Access Layer                       ?
?  - IssueRepository                       ?
?  - Query Building                        ?
????????????????????????????????????????????
               ? SQLite Driver
               ?
????????????????????????????????????????????
?  SQLite Database                         ?
?  datwise-dev.db                          ?
????????????????????????????????????????????
```

## ?? Configuration

### Ports
| Service | HTTPS | HTTP |
|---------|-------|------|
| API | 53486 | 53487 |
| WebForms | 53485 | 53488 |

### Database
- **Type:** SQLite
- **File:** `Datwise.Api/datwise-dev.db`
- **Auto-Created:** Yes, on first API startup

### Connection String
```
Data Source=datwise-dev.db
```

## ?? Testing

### Run Tests
```bash
dotnet test Datwise.Tests
```

### Test Coverage
- **Repository Tests** - 8 tests covering CRUD, filtering, sorting
- **Service Tests** - 13 tests covering business logic and validation
- **API Tests** - Full endpoint coverage

### Running Tests
```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity=detailed

# Run specific test class
dotnet test --filter ClassName=IssueRepositoryTests
```

## ?? Security Considerations

- Input validation on all endpoints
- SQL injection protection via EF Core
- HTTPS enforced in production
- No sensitive data in logs
- Proper error handling without exposing internals

## ?? Performance

- **Database Indexes** - On Status, Severity, and ReportedDate
- **Async/Await** - All database operations are async
- **Efficient Queries** - EF Core query optimization
- **Lazy Loading** - Disabled to prevent N+1 queries

## ?? Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## ?? License

This project is licensed under the MIT License - see the LICENSE file for details.

## ?? Deployment

### Production Checklist
- [ ] Update database connection string for production
- [ ] Configure HTTPS certificates
- [ ] Enable CORS if needed
- [ ] Set up logging and monitoring
- [ ] Configure database backups
- [ ] Review and update security policies
- [ ] Performance testing and optimization

### Docker Support (Coming Soon)
- Containerized API
- Containerized WebForms
- Docker Compose orchestration

## ?? Additional Resources

- [QUICKSTART.md](./QUICKSTART.md) - 5-minute getting started guide
- [DATABASE_INITIALIZATION.md](./DATABASE_INITIALIZATION.md) - Database setup details
- [API Documentation](https://localhost:53486/swagger) - Swagger UI
- [.NET 9 Documentation](https://learn.microsoft.com/dotnet/fundamentals/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [Razor Pages](https://learn.microsoft.com/aspnet/core/razor-pages/)

## ?? Troubleshooting

### Port Already in Use
```powershell
.\cleanup-ports.ps1
```

### Database Not Creating
1. Delete existing `datwise-dev.db` file
2. Restart the API
3. Check console for initialization messages

### API Not Responding
1. Verify API is running on port 53486
2. Check firewall settings
3. Review console output for errors

### WebForms Can't Connect to API
1. Ensure API is running first
2. Check `appsettings.json` for correct `ApiBaseUrl`
3. Verify network connectivity

## ?? Support

For issues and questions:
1. Check [QUICKSTART.md](./QUICKSTART.md)
2. Review console output for error messages
3. Check database file permissions
4. Verify ports are available

## ?? Changelog

See [CHANGELOG.md](./CHANGELOG.md) for version history and changes.

---

**Made with ?? for security and safety professionals**

Last Updated: 2024
