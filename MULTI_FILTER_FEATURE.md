# Multi-Filter Feature Documentation

## Overview

The Datwise API now supports comma-separated filtering for both `status` and `severity` parameters. This allows you to query multiple statuses and/or severities in a single request using a "contains" logic.

## API Usage

### Single Value Filtering (Original Functionality)

```http
GET /api/issues?status=Open&severity=High
```

### Multi-Value Filtering (New Feature)

Use comma-separated values to filter by multiple statuses or severities:

```http
GET /api/issues?status=Open,In%20Progress
GET /api/issues?severity=High,Critical
GET /api/issues?status=Open,In%20Progress,Resolved&severity=High,Critical
```

## Filter Parameters

### Status Filter
Accepts comma-separated values from: `Open`, `In Progress`, `Resolved`

**Examples:**
```
status=Open
status=Open,In Progress
status=Open,In Progress,Resolved
```

**URL Encoded Examples:**
```
status=Open%20Progress       (In Progress)
status=Open,In%20Progress
status=Resolved,Open
```

### Severity Filter
Accepts comma-separated values from: `Low`, `Medium`, `High`, `Critical`

**Examples:**
```
severity=High
severity=High,Critical
severity=Low,Medium,High,Critical
```

### Combined Filters with Sorting

```http
GET /api/issues?status=Open,In Progress&severity=High,Critical&sort=-date
GET /api/issues?status=Open&severity=Medium,High,Critical&sort=title
GET /api/issues?severity=Critical&sort=-severity
```

## Implementation Details

### Logic
- **Multiple values in same parameter:** OR logic (contains any)
  - `status=Open,In Progress` returns issues with status Open OR In Progress
  - `severity=High,Critical` returns issues with severity High OR Critical

- **Multiple parameters:** AND logic (intersection)
  - `status=Open,In Progress&severity=High,Critical` returns issues with (Open OR In Progress) AND (High OR Critical)

### Parsing
- Values are split by comma (`,`)
- Whitespace around values is trimmed
- Empty values are ignored
- Case-sensitive matching on the database values

## Example API Calls

### Get All Open and In-Progress Issues
```http
GET /api/issues?status=Open,In%20Progress
```
Response: All issues with status "Open" or "In Progress"

### Get Critical and High Severity Issues
```http
GET /api/issues?severity=Critical,High
```
Response: All issues with severity "Critical" or "High"

### Get Open/In-Progress Critical/High Issues Sorted by Date (Newest First)
```http
GET /api/issues?status=Open,In%20Progress&severity=Critical,High&sort=-date
```
Response: Issues that are (Open OR In Progress) AND (Critical OR High), sorted by date descending

### Get Resolved Issues Only
```http
GET /api/issues?status=Resolved
```
Response: All resolved issues

### Get All Issues Excluding Resolved
```http
GET /api/issues?status=Open,In%20Progress
```
Response: All active issues

## cURL Examples

### Get Open and In-Progress Issues
```bash
curl "https://localhost:53486/api/issues?status=Open,In%20Progress"
```

### Get High and Critical Severity Issues
```bash
curl "https://localhost:53486/api/issues?severity=High,Critical"
```

### Complex Filter: Active High/Critical Issues Sorted by Date
```bash
curl "https://localhost:53486/api/issues?status=Open,In%20Progress&severity=High,Critical&sort=-date"
```

### PowerShell Example
```powershell
$statusFilter = "Open,In Progress"
$severityFilter = "High,Critical"
$url = "https://localhost:53486/api/issues?status=$([System.Web.HttpUtility]::UrlEncode($statusFilter))&severity=$([System.Web.HttpUtility]::UrlEncode($severityFilter))&sort=-date"
Invoke-RestMethod -Uri $url
```

## Testing

Three new unit tests have been added to verify the multi-filter functionality:

1. **GetIssuesAsync_WithMultipleStatuses_ReturnsFilteredIssues**
   - Tests comma-separated status filtering
   - Verifies only matching statuses are returned

2. **GetIssuesAsync_WithMultipleSeverities_ReturnsFilteredIssues**
   - Tests comma-separated severity filtering
   - Verifies only matching severities are returned

3. **GetIssuesAsync_WithMultipleStatusesAndSeverities_ReturnsFilteredIssues**
   - Tests combined multi-status and multi-severity filtering
   - Verifies AND logic between parameters

Run tests with:
```bash
dotnet test Datwise.Tests
```

## Backwards Compatibility

? **Fully backwards compatible** - All existing single-value queries continue to work exactly as before.

**Before:**
```http
GET /api/issues?status=Open&severity=High
```

**After (still works the same):**
```http
GET /api/issues?status=Open&severity=High
```

**New capability:**
```http
GET /api/issues?status=Open,In Progress&severity=High,Critical
```

## Performance Considerations

- Queries using comma-separated values generate SQL with `IN` clauses
- Database indexes on `Status` and `Severity` columns optimize these queries
- No performance degradation compared to multiple individual API calls

## WebForms Integration

The WebForms UI can be enhanced to use this feature. For example, a multi-select checkbox filter could call:

```javascript
// Example: Filter for Open and In Progress issues
const statuses = ["Open", "In Progress"];
const statusFilter = statuses.join(",");
const url = `/api/issues?status=${encodeURIComponent(statusFilter)}&sort=-date`;
```

## Future Enhancements

Potential future improvements:
1. Add pagination support with `$top` and `$skip` parameters
2. Add text search on Title and Description fields
3. Add date range filtering
4. Add department filtering
5. Support for more complex query operators (greater than, less than, etc.)

## Reference

- **API Base URL:** `https://localhost:53486/api/issues`
- **Available Statuses:** Open, In Progress, Resolved
- **Available Severities:** Low, Medium, High, Critical
- **Sortable Fields:** id, title, severity, status, department, location, reportedby, date
- **Sort Prefix:** `-` for descending order

---

Last Updated: 2024
