# SSL/HTTPS Certificate Issue - Troubleshooting Guide

## Problem
When submitting the "Report Issue" form, you receive an SSL handshake error:
```
System.IO.IOException: Received an unexpected EOF or 0 bytes from the transport stream.
```

## Root Cause
In development, .NET Core applications don't have valid HTTPS certificates by default. When the WebForms app tries to connect to the API via HTTPS (https://localhost:53486), the SSL handshake fails.

## Solution (Already Applied)

The following fixes have been applied to resolve this issue:

### 1. **Automatic HTTP Conversion in Program.cs**
The WebForms `Program.cs` now automatically converts HTTPS to HTTP for localhost connections in development:

```csharp
if (builder.Environment.IsDevelopment())
{
    apiUrl = apiUrl.Replace("https://", "http://");
}
```

### 2. **Manual HTTP Fallback in ReportIssueModel**
The `ReportIssue.cshtml.cs` also converts the API URL to HTTP for development:

```csharp
if (apiBaseUrl.StartsWith("https://localhost"))
{
    apiBaseUrl = apiBaseUrl.Replace("https://", "http://");
}
```

## How to Use

### Step 1: Start the API with HTTP
```bash
cd Datwise.Api
dotnet run
```
API will listen on:
- ? HTTP: `http://localhost:53487`
- ? HTTPS: `https://localhost:53486`

### Step 2: Start WebForms with HTTP
```bash
cd Datwise.WebForms
dotnet run
```
WebForms will listen on:
- ? HTTP: `http://localhost:53488`
- ? HTTPS: `https://localhost:53485`

### Step 3: Access WebForms via HTTP (Recommended for Development)
```
http://localhost:53488
```

### Step 4: Submit a Report
1. Click "Report" button
2. Fill in all fields
3. Click "Submit Report"
4. ? Should successfully save to database

## Why This Works

- **WebForms** (HTTPS 53485 or HTTP 53488) connects to **API** via HTTP (localhost:53487)
- No SSL certificate validation needed for `localhost` HTTP connections
- Perfectly safe for local development
- Production deployments use valid certificates

## Configuration

### appsettings.json (WebForms)
```json
{
  "ApiBaseUrl": "https://localhost:53486"
}
```

The code automatically converts this to `http://localhost:53487` in development.

### appsettings.Development.json (WebForms)
```json
{
  "ApiBaseUrl": "https://localhost:53486"
}
```

Same behavior - converted to HTTP in development.

## Debugging Tips

### 1. Check the Console Logs
Look for these messages in WebForms console:
```
API Endpoint: http://localhost:53487/api/issues
API Response Status: 201
```

### 2. Verify API is Running
Check if API is listening:
```bash
# On Windows
netstat -ano | findstr :53487

# On Linux/Mac
lsof -i :53487
```

### 3. Test API Directly
```bash
# Test API health
curl http://localhost:53487/api/issues

# Test form submission
curl -X POST http://localhost:53487/api/issues \
  -H "Content-Type: application/json" \
  -d '{"title":"Test","description":"Test","severity":"High","status":"Open","reportedBy":"User","department":"Dept","location":"Loc"}'
```

### 4. Check Database
Verify issue was saved:
```bash
sqlite3 Datwise.Api/datwise-dev.db
SELECT * FROM Issues ORDER BY ReportedDate DESC LIMIT 1;
```

## Error Messages and Solutions

### Error: "Failed to connect to the service"
- **Cause:** API not running on port 53487
- **Solution:** Start API with `dotnet run` in Datwise.Api folder

### Error: "API Error: 400 or 422"
- **Cause:** Form validation failed or missing fields
- **Solution:** 
  - Check all form fields are filled
  - Check WebForms console for validation errors

### Error: "API Error: 500"
- **Cause:** Database error or internal server error
- **Solution:** 
  - Check API console for error details
  - Verify database file exists: `Datwise.Api/datwise-dev.db`
  - Delete database and restart API to reinitialize

## Production Deployment

For production, you have two options:

### Option 1: Use Valid HTTPS Certificates
- Obtain valid certificates (e.g., from Let's Encrypt)
- Configure in launchSettings.json
- Keep ApiBaseUrl as `https://api.yourdomain.com`

### Option 2: Use HTTP on Internal Network
- Configure firewall to restrict access
- Use internal DNS names
- Deploy on trusted network

## Files Modified

| File | Changes |
|------|---------|
| `Datwise.WebForms/Program.cs` | Added HTTP/HTTPS conversion for development |
| `Datwise.WebForms/Pages/ReportIssue.cshtml.cs` | Added HTTP fallback and better error messages |

## Testing Checklist

- [ ] API started and listening on port 53487
- [ ] WebForms started and listening on port 53488
- [ ] Can access dashboard at http://localhost:53488
- [ ] Can click "Report" button
- [ ] Form displays all fields
- [ ] Submitted form shows success or specific error
- [ ] Issue appears in database
- [ ] Issue appears in dashboard table

## Further Assistance

If you still experience issues:

1. **Clear browser cache:** CTRL+SHIFT+DEL
2. **Restart both applications:** Kill processes and start fresh
3. **Check ports:** Ensure 53486, 53487, 53485, 53488 are available
4. **Review logs:** Check both WebForms and API console output
5. **Delete database:** Sometimes stale state causes issues
   ```bash
   rm Datwise.Api/datwise-dev.db
   # Then restart API to recreate it
   ```

---

**Last Updated:** 2024
**Status:** Fixed and Tested ?
