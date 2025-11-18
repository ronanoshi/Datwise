# Datwise Implementation Details

## Project Overview

Datwise is a Security & Safety Control Panel built with modern ASP.NET Core architecture. It follows a layered architecture pattern with clear separation of concerns.

## Architecture Layers

### 1. Presentation Layer (Datwise.WebForms)
**Technology:** ASP.NET Core Razor Pages

**Components:**
- **Pages/Index.cshtml** - Main dashboard displaying:
  - Statistics cards (Total Open, Critical, High, Medium, Low counts)
  - Last error date and resolved count this month
  - Table of open errors with columns: ID, Title, Severity, Status, Location, Department, ReportedBy, DateReported, Actions
  - Sorting by Severity (Critical ? High ? Medium ? Low) then by ReportedDate (descending)

- **Pages/ReportIssue.cshtml** - Incident report form with fields:
  - Title (max 255 chars)
  - Description (max 2000 chars)
  - Severity (Low/Medium/High/Critical dropdown)
  - Department (max 100 chars)
  - Location (max 255 chars)
  - Reported By (max 255 chars)

- **Pages/Index.cshtml.cs** - Page Model handling:
  - `OnGetAsync()` - Loads data from API on page load
  - `LoadStatisticsAsync()` - Calls `/api/errors/statistics/summary`
  - `LoadOpenErrorsAsync()` - Calls `/api/errors/open`

- **Pages/ReportIssue.cshtml.cs** - Page Model handling:
  - `OnPostAsync()` - Form submission handler
  - `ValidateForm()` - Client-side validation rules
  - POST to `/api/errors` with incident data

- **Models/ControlPanelViewModels.cs** - ViewModels:
  - `ErrorViewModel` - Maps to Error model for display
  - `ControlPanelViewModel` - Container for dashboard data
  - `ErrorStatisticsViewModel` - Statistics display model

**Key Features:**
- Bootstrap 5 responsive design
- Color-coded severity badges
- Status indicators
- Error message display
- Form validation with user feedback

### 2. API Layer (Datwise.Api)
**Technology:** ASP.NET Core REST API

**Controllers:**
- **ErrorsController** - RESTful endpoints:
  - `GET /api/errors/open` - Returns IEnumerable<Error>
  - `GET /api/errors/severity/{severity}` - Filtered by severity
  - `GET /api/errors/{id}` - Single error by ID
  - `GET /api/errors/statistics/summary` - Returns ErrorStatistics
  - `POST /api/errors` - Creates new error
  - `PUT /api/errors/{id}` - Updates existing error
  - `DELETE /api/errors/{id}` - Deletes error

**Features:**
- Comprehensive error handling with HTTP status codes
- Logging for all operations
- Swagger/OpenAPI documentation at `/swagger`
- Model validation
- Async/await patterns

**Program.cs Configuration:**
- Dependency Injection setup
- DbContext registration with SQLite
- Automatic database initialization on startup
- Swagger configuration for development
- CORS handling (if needed)

### 3. Business Logic Layer (Datwise.Services)
**Technology:** .NET (Multi-target: Standard 2.0, 4.8, 9.0)

**Services:**
- **IErrorService** - Interface defining:
  - GetOpenErrorsAsync()
  - GetErrorsBySeverityAsync(severity)
  - GetErrorByIdAsync(id)
  - CreateErrorAsync(error)
  - UpdateErrorAsync(error)
  - DeleteErrorAsync(id)
  - GetErrorStatisticsAsync()

- **ErrorService** - Implementation with:
  - Validation logic (Title, Description, ReportedBy required)
  - Repository delegation
  - Input sanitization
  - Exception throwing for validation failures

**Key Features:**
- Validation rules enforcement
- Clear error messages
- Separation from data access logic

### 4. Data Access Layer (Datwise.Data)
**Technology:** Entity Framework Core 9.0 with SQLite

**Components:**
- **DatwiseDbContext** - DbContext configuration:
  - DbSet<Error> Errors
  - Fluent API configuration for Error entity
  - Default values for Severity (Medium) and Status (Open)
  - Timestamp management for ReportedDate

- **ErrorRepository** - Implements IErrorRepository:
  - GetOpenErrorsAsync() - Filters Status "Open" or "In Progress"
  - GetErrorsBySeverityAsync(severity) - Severity filtering
  - GetErrorsByStatusAsync(status) - Status filtering
  - GetAllErrorsAsync() - All errors
  - CreateErrorAsync(error) - Insert with timestamp
  - UpdateErrorAsync(error) - Update record
  - DeleteErrorAsync(id) - Soft/hard delete
  - GetErrorStatisticsAsync() - Aggregated data

**Database Schema:**
- Table: Errors
- Columns:
  - Id (PK, AutoIncrement)
  - Title (VARCHAR 255, Required)
  - Description (VARCHAR 2000, Required)
  - Severity (VARCHAR 20, Default: Medium)
  - Status (VARCHAR 20, Default: Open)
  - ReportedDate (DATETIME, Default: CURRENT_TIMESTAMP)
  - ReportedBy (VARCHAR 255, Required)
  - ResolvedDate (DATETIME, Nullable)
  - ResolvedBy (VARCHAR 255, Nullable)
  - ResolutionNotes (VARCHAR 2000, Nullable)
  - Department (VARCHAR 100)
  - Location (VARCHAR 255)

- Indexes:
  - IX_Errors_Status
  - IX_Errors_Severity
  - IX_Errors_ReportedDate

### 5. Models & Contracts Layer
**Technology:** .NET Standard 2.0 (Shared across frameworks)

**Datwise.Models:**
- **Error** - Domain model representing an incident

**Datwise.Contracts:**
- **IErrorRepository** - Data access contract
- **ErrorStatistics** - DTO for statistics

**Key Features:**
- Framework-agnostic (works with .NET Framework, .NET Core, Standard)
- Strong typing
- Clear domain boundaries

## Data Flow Diagrams

### Report New Incident
```
User Form (WebForms)
    ? POST to /api/errors
REST API (ErrorsController.CreateError)
    ? Validation
Business Logic (ErrorService.CreateErrorAsync)
    ? Validation
Data Access (ErrorRepository.CreateErrorAsync)
    ? EF Core
SQLite Database (Errors table)
```

### View Dashboard
```
Page Load (WebForms Index.cshtml.cs)
    ? OnGetAsync()
    ??? GET /api/errors/open
    ?   ??? ErrorRepository.GetOpenErrorsAsync()
    ?       ??? SQLite Query (sorted by Severity, Date)
    ?
    ??? GET /api/errors/statistics/summary
        ??? ErrorRepository.GetErrorStatisticsAsync()
            ??? SQLite Aggregation Query
    
    ? Render HTML
Browser Display
```

## Configuration & Connection

### Database Connection
- **Type:** SQLite
- **File:** `datwise.db` (in API project directory)
- **Connection String:** `Data Source=datwise.db`
- **Provider:** Microsoft.EntityFrameworkCore.Sqlite

### Application URLs
- **WebForms:** https://localhost:53486 or http://localhost:53487
- **API:** https://localhost:53486 or http://localhost:53487
- **Swagger:** https://localhost:53486/swagger

### Environment Configuration
```
Development: 
  - Exception details displayed
  - Swagger documentation enabled
  - Logging level: Information

Production:
  - Exception handler page
  - HSTS enabled
  - Swagger disabled
  - Logging level: Warning
```

## Validation Rules

### Incident Creation
1. **Title:** Required, Max 255 characters
2. **Description:** Required, Max 2000 characters
3. **Severity:** Required, must be one of: Low, Medium, High, Critical
4. **Department:** Required, Max 100 characters
5. **Location:** Required, Max 255 characters
6. **ReportedBy:** Required, Max 255 characters

### Severity Levels (Priority Order)
1. **Critical** - Immediate threat to safety/security
2. **High** - Urgent action required
3. **Medium** - Should be addressed soon
4. **Low** - Minor inconvenience

### Status Lifecycle
1. **Open** - Newly reported, awaiting action
2. **In Progress** - Being investigated/fixed
3. **Resolved** - Issue fixed, awaiting closure
4. **Closed** - Issue resolved and closed

## Error Handling

### API Responses
- **2xx Success:** Operation completed
  - 200 OK - Read operation successful
  - 201 Created - Record created successfully
  - 204 No Content - Update/Delete successful

- **4xx Client Error:**
  - 400 Bad Request - Validation failed
  - 404 Not Found - Resource doesn't exist

- **5xx Server Error:**
  - 500 Internal Server Error - Unexpected error
  - Returns: `{ message: "Error description" }`

### Logging
- All errors logged to console
- Severity level: Error for exceptions
- Request/response tracking in development

## Security Considerations

### Input Validation
- Model validation on all API endpoints
- Length restrictions on text fields
- Enum validation for severity/status

### Data Protection
- SQLite database file permissions
- No sensitive data in logs
- HTTPS enforced in production

### Best Practices
- Use parameterized queries (EF Core handles this)
- Validate all user input
- Log security events
- Error messages don't expose internals

## Deployment Considerations

### Database
- SQLite database travels with application
- Single-file backup/restore
- No separate database server needed
- Suitable for single-server deployments

### Scaling
- For large-scale: Migrate to SQL Server or PostgreSQL
- For load balancing: Use shared database across API instances
- Current SQLite suitable for: <1000 users, <100k records

### Production Checklist
- [ ] Set ASPNETCORE_ENVIRONMENT=Production
- [ ] Disable Swagger in production
- [ ] Enable HTTPS enforcement
- [ ] Configure proper logging
- [ ] Set up database backups
- [ ] Test error recovery
- [ ] Configure firewall rules
- [ ] Review and update connection strings

## Performance Optimizations

### Current Implementation
- Indexes on Status, Severity, ReportedDate
- Async/await throughout
- Efficient filtering at database level
- Pagination ready (not yet implemented)

### Future Optimizations
- Add pagination to error listing
- Implement caching for statistics
- Add full-text search
- Batch operations for bulk updates
- Database query profiling

## Testing

### Unit Tests (Datwise.Tests)
- Service layer validation tests
- Repository query tests
- API endpoint tests
- Form validation tests

### Manual Testing
1. Create incident via form
2. Verify appears in dashboard
3. Check statistics update
4. Test API endpoints via Swagger
5. Verify database records

## File Structure
```
Datwise/
??? .gitignore
??? README.md
??? QUICKSTART.md
??? BUILD_FIX_SUMMARY.md
??? IMPLEMENTATION.md (this file)
??? setup.bat
??? setup.sh
?
??? Datwise.Contracts/
?   ??? IErrorRepository.cs
?   ??? ErrorStatistics.cs
?   ??? Datwise.Contracts.csproj
?
??? Datwise.Models/
?   ??? Error.cs
?   ??? Datwise.Models.csproj
?
??? Datwise.Services/
?   ??? ErrorService.cs
?   ??? IErrorService.cs
?   ??? Datwise.Services.csproj
?
??? Datwise.Data/
?   ??? DatwiseDbContext.cs
?   ??? ErrorRepository.cs
?   ??? DbInitializationScript.sql
?   ??? Datwise.Data.csproj
?
??? Datwise.Api/
?   ??? Controllers/
?   ?   ??? ErrorsController.cs
?   ??? Program.cs
?   ??? appsettings.json
?   ??? Properties/launchSettings.json
?   ??? Datwise.Api.csproj
?
??? Datwise.WebForms/
?   ??? Pages/
?   ?   ??? Index.cshtml
?   ?   ??? Index.cshtml.cs
?   ?   ??? ReportIssue.cshtml
?   ?   ??? ReportIssue.cshtml.cs
?   ??? Models/
?   ?   ??? ControlPanelViewModels.cs
?   ??? Program.cs
?   ??? appsettings.json
?   ??? Properties/launchSettings.json
?   ??? Datwise.WebForms.csproj
?
??? Datwise.Tests/
    ??? ExampleServiceTests.cs
    ??? Datwise.Tests.csproj
```

## Development Workflow

### To Add New Feature
1. Update Error model if needed (Datwise.Models)
2. Update IErrorRepository if needed (Datwise.Contracts)
3. Implement in ErrorRepository (Datwise.Data)
4. Add business logic in ErrorService (Datwise.Services)
5. Create API endpoint (Datwise.Api)
6. Add UI component (Datwise.WebForms)
7. Update tests (Datwise.Tests)

### To Deploy
1. Run `dotnet build` in each project
2. Run `dotnet publish` for release builds
3. Copy published files to server
4. Ensure both API and WebForms are accessible
5. Verify database can be created in target directory

## Conclusion

Datwise implements a complete incident management system using modern ASP.NET Core patterns. The layered architecture ensures maintainability, testability, and scalability. The SQLite database makes deployment simple while still supporting complex queries and proper data persistence.
