# Report Issue Form - Implementation Guide

## Overview

The Report Issue functionality is fully implemented end-to-end, from the WebForms UI through to the database.

## File Locations & Code Flow

### 1. **User Interface (HTML Form)**
**File:** `Datwise.WebForms/Pages/ReportIssue.cshtml`

**Location in File:** The entire file contains the form
- Form submits via HTTP POST to the PageModel
- Contains validation error messages for each field
- Fields: Title, Description, Severity, Department, Location, Your Name

### 2. **Form Handler (C# Code-Behind)**
**File:** `Datwise.WebForms/Pages/ReportIssue.cshtml.cs`
**Class:** `ReportIssueModel`
**Main Method:** `OnPostAsync()` (Line 37)

**What it does:**
1. Receives form data with `[FromForm]` attributes
2. Validates all fields using `ValidateForm()` method
3. Creates a JSON payload with the issue data
4. Sends HTTP POST to API: `POST /api/issues`
5. Handles success (redirect to /) and error (show error message)

**Key improvements made:**
- ? Added `IConfiguration` dependency for API URL
- ? Added `[FromForm]` attributes for proper model binding
- ? Enhanced logging at each step (debugging)
- ? Better error handling for HttpRequestException
- ? Detailed error messages shown to user

### 3. **HTTP Client Configuration**
**File:** `Datwise.WebForms/Program.cs` (Lines 11-23)

**Configuration:**
```csharp
builder.Services.AddHttpClient<ReportIssueModel>(client =>
{
    var apiUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:53486";
    client.BaseAddress = new Uri(apiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigureHttpClient(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

**Benefits:**
- ? Typed HttpClient specific to ReportIssueModel
- ? BaseAddress automatically prepended to all requests
- ? 30-second timeout for requests
- ? Accept header set for JSON responses

### 4. **API Endpoint**
**File:** `Datwise.Api/Controllers/IssuesController.cs`
**Method:** `CreateIssue()` (POST /api/issues)

**What it does:**
1. Receives JSON payload from WebForms
2. Validates the issue object
3. Calls `IssueService.CreateIssueAsync()`
4. Returns HTTP 201 (Created) on success

### 5. **Business Logic Layer**
**File:** `Datwise.Services/IssueService.cs`
**Method:** `CreateIssueAsync()`

**What it does:**
1. Validates required fields (Title, Description, ReportedBy)
2. Calls repository to save to database
3. Returns the new issue ID

### 6. **Data Access Layer**
**File:** `Datwise.Data/IssueRepository.cs`
**Method:** `CreateIssueAsync()` (Line 103)

**What it does:**
1. Sets `ReportedDate = DateTime.UtcNow`
2. Adds the issue to the DbContext
3. Calls `SaveChangesAsync()` to commit to database
4. Returns the new issue ID

### 7. **Database**
**File:** `datwise-dev.db` (SQLite)
**Table:** `Issues`

**Stored data:**
- Id (auto-increment)
- Title
- Description
- Severity
- Status (default "Open")
- ReportedBy
- Department
- Location
- ReportedDate (current UTC time)

## Full Request Flow

```
1. User fills form ? Clicks Submit Report button
                    ?
2. HTML Form ? POST to /ReportIssue (ReportIssueModel.OnPostAsync)
                    ?
3. ReportIssueModel ? Validates form data
                    ?
4. If valid ? Creates JSON payload
                    ?
5. HttpClient ? POST to https://localhost:53486/api/issues
                    ?
6. API Controller ? Validates & calls IssueService.CreateIssueAsync()
                    ?
7. IssueService ? Validates & calls IssueRepository.CreateIssueAsync()
                    ?
8. IssueRepository ? Saves to database & commits transaction
                    ?
9. Returns ID ? API returns 201 Created
                    ?
10. WebForms ? Redirect to "/" or show success message
```

## Troubleshooting

### Issue: Form submission fails

**Check:**
1. ? Is the API running on port 53486?
   ```
   ps aux | grep dotnet
   ```

2. ? Is the appsettings.json configured correctly?
   ```json
   "ApiBaseUrl": "https://localhost:53486"
   ```

3. ? Check WebForms console logs for HTTP errors
   - Look for "API Response Status" entries
   - Look for "HTTP Request error" entries

4. ? Test API directly with cURL:
   ```bash
   curl -X POST https://localhost:53486/api/issues \
     -H "Content-Type: application/json" \
     -d '{"title":"Test","description":"Test","severity":"High","status":"Open","reportedBy":"Test User","department":"Test","location":"Test"}'
   ```

### Issue: Form validation shows errors

**Check:**
- All required fields are filled
- Title ? 255 characters
- Description ? 2000 characters
- Department ? 100 characters
- Location ? 255 characters
- Name ? 255 characters
- Severity is one of: Low, Medium, High, Critical

## Testing the Feature

### Test 1: Successful Submission
1. Start API: `cd Datwise.Api && dotnet run`
2. Start WebForms: `cd Datwise.WebForms && dotnet run`
3. Navigate to https://localhost:53485/ReportIssue
4. Fill all fields:
   - Title: "Test Issue"
   - Description: "This is a test issue"
   - Severity: "High"
   - Department: "Testing"
   - Location: "Lab 1"
   - Your Name: "John Doe"
5. Click "Submit Report"
6. ? Should redirect to dashboard with new issue visible

### Test 2: Check Database
```bash
# Open SQLite browser or use command line
sqlite3 Datwise.Api/datwise-dev.db
SELECT * FROM Issues WHERE Title = 'Test Issue';
```

### Test 3: API Logs
Check the API console for:
```
[15:23:45 INF] POST /api/issues HTTP/1.1 [200]
```

## Code Locations Summary

| Component | File | Function |
|-----------|------|----------|
| UI Form | `ReportIssue.cshtml` | N/A (HTML/Razor) |
| Form Handler | `ReportIssue.cshtml.cs` | `OnPostAsync()` |
| Validation | `ReportIssue.cshtml.cs` | `ValidateForm()` |
| HTTP Config | `Program.cs` | DI Container setup |
| API Endpoint | `IssuesController.cs` | `CreateIssue()` POST |
| Business Logic | `IssueService.cs` | `CreateIssueAsync()` |
| Data Access | `IssueRepository.cs` | `CreateIssueAsync()` |
| Database | `datwise-dev.db` | `Issues` table |

## Recent Fixes Applied

? **Fixed form submission issues:**
1. Added explicit `[FromForm]` attributes for parameter binding
2. Added IConfiguration injection to access API URL
3. Added enhanced logging for debugging
4. Improved HttpClient configuration with BaseAddress and timeout
5. Better error messages for HTTP failures
6. Added HttpRequestException handling

All fixes ensure the form submission works end-to-end from UI to database!
