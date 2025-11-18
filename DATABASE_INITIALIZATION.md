# Database Initialization - Complete Setup

## What Was Done

### 1. Created DatabaseSeeder.cs
A static class with an extension method that seeds the database with 5 sample security/safety issues:
- Fire Extinguisher Missing (High severity, Open)
- Wet Floor Hazard (Medium severity, Open)
- Equipment Malfunction (Critical severity, In Progress)
- First Aid Kit Empty (Low severity, Open)
- Broken Handrail (High severity, Open)

### 2. Updated Program.cs
Modified the API startup to:
- Create the database schema using `EnsureCreated()`
- Call `SeedTestData()` to populate with sample issues
- Display status messages during initialization

### 3. Updated initialize-database.sql
Updated the SQL script to reference the "Issues" table instead of "Errors" table

### 4. Created initialize-db.ps1
PowerShell script to automate the initialization process:
- Removes old database file
- Builds the solution
- Starts the API (which creates and seeds the database)

## How to Initialize the Database

### Option 1: Quick Start (Recommended)

```powershell
cd C:\Users\ronan\workspace\Datwise\Datwise.Api
.\initialize-db.ps1
```

### Option 2: Manual Steps

```powershell
# Step 1: Navigate to API folder
cd "C:\Users\ronan\workspace\Datwise\Datwise.Api"

# Step 2: Delete old database (if exists)
Remove-Item datwise.db -ErrorAction SilentlyContinue

# Step 3: Build solution
dotnet build

# Step 4: Run the API (will create and seed database)
dotnet run
```

### Option 3: Run in IDE
1. Open Visual Studio
2. Set "Datwise.Api" as startup project
3. Press F5
4. Watch the console output for:
   - "? Database created/verified successfully"
   - "? Test data seeded successfully"

## Verify the Database Was Created

### Check 1: Look for the file
```powershell
ls "C:\Users\ronan\workspace\Datwise\Datwise.Api\datwise.db"
```

### Check 2: Query the API
Navigate to: https://localhost:53486/swagger

Click on "GET /api/issues/open" and execute.
You should see 5 sample issues returned.

### Check 3: View in SQLite Browser
1. Download SQLite Browser: https://sqlitebrowser.org/
2. Open `datwise.db` file
3. Browse the Issues table to see all records

## Sample Data Details

| Title | Severity | Status | Department | Location |
|-------|----------|--------|------------|----------|
| Fire Extinguisher Missing | High | Open | Facilities | Building A - Floor 2 |
| Wet Floor Hazard | Medium | Open | Operations | Building B - Cafeteria |
| Equipment Malfunction | Critical | In Progress | Maintenance | Building A - Floor 1 |
| First Aid Kit Empty | Low | Open | Facilities | Building C - Floor 3 |
| Broken Handrail | High | Open | Maintenance | Building A - Stairwell |

## Database Schema

**Issues Table:**
- Id (INTEGER PRIMARY KEY AUTOINCREMENT)
- Title (TEXT, required, max 255)
- Description (TEXT, required, max 2000)
- Severity (TEXT, default 'Medium')
- Status (TEXT, default 'Open')
- ReportedDate (DATETIME)
- ReportedBy (TEXT, required)
- ResolvedDate (DATETIME, nullable)
- ResolvedBy (TEXT, nullable)
- ResolutionNotes (TEXT, nullable)
- Department (TEXT, max 100)
- Location (TEXT, max 255)

**Indexes:**
- idx_issues_status
- idx_issues_severity
- idx_issues_reported_date
- idx_issues_status_severity

## Testing the Database

### Test 1: Create a new issue via API
```json
POST /api/issues
{
  "title": "Test Issue",
  "description": "This is a test",
  "severity": "High",
  "status": "Open",
  "reportedBy": "Test User",
  "department": "Testing",
  "location": "Lab"
}
```

### Test 2: Query all open issues
```
GET /api/issues/open
```

### Test 3: Get issues by severity
```
GET /api/issues/severity/Critical
```

### Test 4: Get statistics
```
GET /api/issues/statistics/summary
```

## Build Status

? **Build Successful** - All projects compile without errors

## Files Created/Modified

### Created:
- `Datwise.Data/DatabaseSeeder.cs` - Database seeding logic
- `Datwise.Api/initialize-db.ps1` - Initialization script

### Modified:
- `Datwise.Api/Program.cs` - Added seeding logic
- `Datwise.Data/initialize-database.sql` - Updated table names

## Next Steps

1. Run the API using one of the methods above
2. Verify the database file was created: `C:\Users\ronan\workspace\Datwise\Datwise.Api\datwise.db`
3. Use Swagger UI to test the API endpoints
4. Access the WebForms UI at https://localhost:53485 to see issues in the dashboard

## Troubleshooting

### Database not created?
- Check the console output for error messages
- Ensure the API folder has write permissions
- Try deleting any existing `datwise.db` file and rerunning

### API won't start?
- Make sure port 53486 is not in use
- Check firewall settings
- Review appsettings.json for connection string

### No sample data showing?
- The seeder only runs if the Issues table is empty
- To reseed, delete the database file and restart the API
- Or manually delete the Issues table in SQLite Browser

