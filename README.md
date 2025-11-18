# Datwise - Security & Safety Control Panel

Datwise is a comprehensive security and safety incident management system built with modern ASP.NET Core. It features a Razor Pages web UI, a REST API, and SQLite database integration.

## Projects
- **Datwise.WebForms** - Razor Pages web application (ASP.NET Core 9.0) with Control Panel UI
- **Datwise.Api** - REST API (ASP.NET Core 9.0) providing incident management endpoints
- **Datwise.Services** - Business logic (Multi-target: .NET Standard 2.0, .NET 4.8, .NET 9.0)
- **Datwise.Contracts** - Service interfaces and DTOs (.NET Standard 2.0)
- **Datwise.Models** - Domain models and entities (.NET Standard 2.0)
- **Datwise.Data** - Entity Framework Core data access layer (ASP.NET Core 9.0)
- **Datwise.Tests** - Unit tests (xUnit, .NET 9.0)

## Features

### Control Panel (WebForms)
- Dashboard with incident statistics
- Table of open incidents sorted by severity and date
- Report Issue form for new incident submissions
- Form validation and error handling
- Responsive Bootstrap 5 UI

### API Endpoints
- `GET /api/errors/open` - Get all open incidents
- `GET /api/errors/severity/{severity}` - Get incidents by severity
- `GET /api/errors/{id}` - Get incident by ID
- `GET /api/errors/statistics/summary` - Get statistics
- `POST /api/errors` - Create new incident report
- `PUT /api/errors/{id}` - Update incident
- `DELETE /api/errors/{id}` - Delete incident
- Swagger/OpenAPI documentation at `/swagger`

## Database
- **Type**: SQLite
- **File**: `datwise.db`
- **Initialization**: Automatic on first API run
- **Tables**: Errors table with incident data

## How to Use

### Quick Start
1. Clone the repository
2. Run from the solution root:
   ```bash
   dotnet restore
   dotnet build
   ```

### Running the Application

**Start the API:**
```bash
cd Datwise.Api
dotnet run
```
API available at: `https://localhost:7194` or `http://localhost:5281`

**Start the WebForms (in a new terminal):**
```bash
cd Datwise.WebForms
dotnet run
```
WebForms available at: `https://localhost:7290` or `http://localhost:5281`

### Visual Studio
1. Open the solution
2. Set **Datwise.Api** as startup project
3. Press F5 to run the API
4. Open a new terminal and run WebForms as shown above

## Configuration

**API** (`Datwise.Api/appsettings.json`):
- ConnectionString: SQLite database path

**WebForms** (`Datwise.WebForms/appsettings.json`):
- ApiBaseUrl: URL where API is running

## Database Setup
The database is automatically created and initialized on the first API run. The schema includes:
- Errors table with proper indexes
- Support for incident severity levels (Low, Medium, High, Critical)
- Support for incident status tracking (Open, In Progress, Resolved, Closed)

## Development
- Built with ASP.NET Core 9.0
- Entity Framework Core for data access
- Bootstrap 5 for UI
- RESTful API design
- Multi-target .NET support for shared libraries

## Deployment
The SQLite database makes this easy to deploy:
1. Build and publish both projects
2. Ensure both have access to the same directory for `datwise.db`
3. Configure connection strings and API URLs as needed
4. Run both services

For more details, see the individual project README files or documentation.
