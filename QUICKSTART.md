# Datwise - Quick Start Guide

Welcome to the Datwise Security & Safety Control Panel! This guide will help you get up and running in minutes.

## ?? Prerequisites

- **.NET 9 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Visual Studio 2022** or **Visual Studio Code**
- **Git** (optional, for cloning)
- **SQLite Browser** (optional, for viewing database) - [Download here](https://sqlitebrowser.org/)

## ?? Quick Start (5 minutes)

### Step 1: Open the Solution

```bash
cd C:\Users\ronan\workspace\Datwise
```

Open in Visual Studio:
- Visual Studio ? File ? Open ? Select `Datwise.sln`

Or in VS Code:
```bash
code .
```

### Step 2: Clean Up Any Running Processes

If you previously ran the applications, kill any lingering processes:

```powershell
# In PowerShell (as Administrator)
cd C:\Users\ronan\workspace\Datwise
.\cleanup-ports.ps1
```

### Step 3: Start the API (Terminal 1)

```bash
cd Datwise.Api
dotnet run
```

**Expected Output:**
```
? Database created/verified successfully
? Test data seeded successfully
Now listening on: https://localhost:53486
```

**The database (`datwise-dev.db`) is created automatically with 5 sample issues.**

### Step 4: Start WebForms (Terminal 2)

```bash
cd Datwise.WebForms
dotnet run
```

**Expected Output:**
```
Now listening on: https://localhost:53485
```

### Step 5: Access the Application

Open your browser and navigate to:

**WebForms Dashboard:** https://localhost:53485 or http://localhost:53488

You should see:
- ?? **Statistics Cards** at the top (Total Open Issues, Critical, High Severity, Resolved This Month)
- ?? **Issues Table** at the bottom with 5 sample security/safety issues
- ?? **Report Issue Button** in the header to create new issues

## ?? Sample Data

The database is seeded with 5 realistic security/safety issues:

| Title | Severity | Status | Department |
|-------|----------|--------|------------|
| Fire Extinguisher Missing | High | Open | Facilities |
| Wet Floor Hazard | Medium | Open | Operations |
| Equipment Malfunction | Critical | In Progress | Maintenance |
| First Aid Kit Empty | Low | Open | Facilities |
| Broken Handrail | High | Open | Maintenance |

## ?? Available Endpoints

### WebForms UI
- **Dashboard:** https://localhost:53485/
- **Report Issue:** https://localhost:53485/ReportIssue

### API Endpoints
- **Swagger Docs:** https://localhost:53486/swagger
- **Get Open Issues:** `GET /api/issues/open`
- **Get Issues by Severity:** `GET /api/issues/severity/{severity}`
- **Get Issue by ID:** `GET /api/issues/{id}`
- **Get Statistics:** `GET /api/issues/statistics/summary`
- **Create Issue:** `POST /api/issues`
- **Update Issue:** `PUT /api/issues/{id}`
- **Delete Issue:** `DELETE /api/issues/{id}`

## ?? Testing the Application

### Test 1: View Dashboard
1. Open https://localhost:53485
2. You should see 5 sample issues in the table
3. Statistics cards show counts and summaries

### Test 2: Create a New Issue
1. Click **Report Issue** button
2. Fill in the form:
   - Title: "Broken Glass in Lab"
   - Description: "Safety hazard identified"
   - Severity: "High"
   - Department: "Safety"
   - Location: "Lab 3"
   - Your Name: "Your Name"
3. Click **Submit**
4. You should see a success message and be redirected to the dashboard
5. The new issue should appear in the table

### Test 3: Query the API
1. Open https://localhost:53486/swagger
2. Click **GET /api/issues/open**
3. Click **Try it out** ? **Execute**
4. You should see all open issues in JSON format

### Test 4: Check the Database
1. Download and open [DB Browser for SQLite](https://sqlitebrowser.org/)
2. File ? Open Database
3. Navigate to: `C:\Users\ronan\workspace\Datwise\Datwise.Api\datwise-dev.db`
4. Browse:
   - **Issues** table - contains all issues
   - **Examples** table - for example data
5. View schema, data, and run custom SQL queries

## ?? Project Structure

```
Datwise/
??? Datwise.Api/                 # REST API (Port 53486)
?   ??? Controllers/
?   ?   ??? IssuesController.cs
?   ??? Program.cs
?   ??? appsettings.Development.json
??? Datwise.WebForms/            # Razor Pages UI (Port 53485)
?   ??? Pages/
?   ?   ??? Index.cshtml         # Dashboard
?   ?   ??? ReportIssue.cshtml   # Report form
?   ??? Models/
?   ?   ??? IssueViewModel.cs
?   ??? wwwroot/css/
?   ?   ??? site.css             # Professional styling
?   ??? Program.cs
??? Datwise.Services/            # Business logic layer
?   ??? IIssueService.cs
?   ??? IssueService.cs
??? Datwise.Data/                # Data access layer
?   ??? DatwiseDbContext.cs      # EF Core DbContext
?   ??? IssueRepository.cs       # Data repository
?   ??? DatabaseSeeder.cs        # Sample data seeding
?   ??? datwise-dev.db          # SQLite database
??? Datwise.Models/              # Domain models
?   ??? Issue.cs
??? Datwise.Contracts/           # Interfaces
?   ??? IIssueRepository.cs
?   ??? IssueStatistics.cs
??? Datwise.Tests/               # Unit tests
```

## ?? Architecture

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
?  - Business Logic (Services)             ?
????????????????????????????????????????????
               ? EF Core
               ?
????????????????????????????????????????????
?  Data Layer (EF Core)                    ?
?  - DatwiseDbContext                      ?
?  - IssueRepository                       ?
????????????????????????????????????????????
               ? SQLite
               ?
????????????????????????????????????????????
?  SQLite Database                         ?
?  datwise-dev.db                          ?
????????????????????????????????????????????
```

## ?? UI Features

The dashboard includes:

- **Statistics Cards** - Quick overview of issue metrics
- **Severity Breakdown** - Summary of Medium, Low, and recent issues
- **Professional Table** - All open issues with full details
- **Color-Coded Badges** - Visual severity and status indicators
- **Responsive Design** - Works on desktop, tablet, and mobile
- **Hover Effects** - Smooth animations and transitions
- **Empty State** - User-friendly message when no issues exist

## ?? Configuration

### Database
- **File:** `datwise-dev.db`
- **Location:** `C:\Users\ronan\workspace\Datwise\Datwise.Api\`
- **Type:** SQLite
- **Auto-Created:** Yes, on first API startup

### Ports
| Service | HTTPS | HTTP |
|---------|-------|------|
| API | 53486 | 53487 |
| WebForms | 53485 | 53488 |

### API Base URL
Configure in `Datwise.WebForms/appsettings.json`:
```json
"ApiBaseUrl": "https://localhost:53486"
```

## ?? Troubleshooting

### Port Already in Use
If you get "Failed to bind to address https://127.0.0.1:XXXX: address already in use":

```powershell
# Run the cleanup script
.\cleanup-ports.ps1
```

Or manually:
```powershell
# Find and kill processes on the port
Get-NetTCPConnection -LocalPort 53486 | Where-Object { $_.State -eq 'Listen' } | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
```

### Database Not Creating
1. Delete `datwise-dev.db` if it exists
2. Restart the API
3. Check console for: "? Database created/verified successfully"

### API Not Responding
1. Verify port 53486 is available
2. Check firewall settings
3. Ensure running in Development mode (ASPNETCORE_ENVIRONMENT=Development)

### WebForms Can't Connect to API
1. Verify API is running first
2. Check `appsettings.json` has correct `ApiBaseUrl`
3. Ensure port 53486 is not blocked by firewall

## ?? Technology Stack

- **.NET 9** - Framework
- **ASP.NET Core** - Web framework
- **Razor Pages** - UI framework
- **Entity Framework Core** - ORM
- **SQLite** - Database
- **Bootstrap 5** - CSS framework
- **Bootstrap Icons** - Icon library

## ?? Documentation

For more detailed information, see:
- [DATABASE_INITIALIZATION.md](./DATABASE_INITIALIZATION.md) - Database setup details
- [Datwise.Data/initialize-database.sql](./Datwise.Data/initialize-database.sql) - SQL schema
- [API Documentation](https://localhost:53486/swagger) - Swagger UI

## ?? What to Do Next

1. ? Run the Quick Start (Steps 1-5 above)
2. ? Test creating a new issue via the Report Issue form
3. ? Explore the API via Swagger UI
4. ? View the database in SQLite Browser
5. ?? Customize the styling in `wwwroot/css/site.css`
6. ?? Add more validation rules or business logic
7. ?? Add unit tests to `Datwise.Tests`

## ?? Common Tasks

### Add a New Issue Programmatically
```csharp
POST https://localhost:53486/api/issues
Content-Type: application/json

{
  "title": "Safety Concern",
  "description": "Issue details",
  "severity": "High",
  "status": "Open",
  "reportedBy": "John Doe",
  "department": "Safety",
  "location": "Building A"
}
```

### Get All Critical Issues
```
GET https://localhost:53486/api/issues/severity/Critical
```

### View Dashboard Statistics
```
GET https://localhost:53486/api/issues/statistics/summary
```

### Delete an Issue
```
DELETE https://localhost:53486/api/issues/{id}
```

## ?? Support

For issues or questions:
1. Check the troubleshooting section above
2. Review console output for error messages
3. Check database file exists and has proper permissions
4. Verify ports are available and not blocked

## ?? License

Datwise Security & Safety Control Panel - 2024

---

**Ready to start?** Run the Quick Start steps above and you'll be up and running in 5 minutes! ??
