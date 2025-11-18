# Copilot Chat Context — Datwise

This file captures the Copilot chat summary, current workspace state, and next steps so you can continue work from Visual Studio Community.

## Summary (short)
- Scaffolding created for Datwise: `Datwise.WebForms`, `Datwise.Api`, `Datwise.Services`, `Datwise.Contracts`, `Datwise.Models`, `Datwise.Data`, `Datwise.Shared`, `Datwise.Tests`.
- `Datwise.Api` migrated to ASP.NET Core targeting modern .NET (currently set to net9.0 on this machine).
- Shared libraries updated to `netstandard2.0` or multi-target to allow compatibility with both WebForms (.NET 4.8) and modern .NET.
- SQLite local dev support: EF Core DbContext scaffolded and `schema.sql` added.
- Git repo initialized and code pushed to `https://github.com/ronanoshi/Datwise`.
- API verified running locally (tested `GET /api/values/1` returning `{ "id":1, "name":"Sample" }`).

## Where things live (important files)
- Solution: `Datwise.sln`
- API: `Datwise.Api/Program.cs`, `Datwise.Api/Controllers/ValuesController.cs`, `Datwise.Api/appsettings.Development.json`
- WebForms: `Datwise.WebForms/Default.aspx`, `Datwise.WebForms/Default.aspx.cs`
- Data: `Datwise.Data/DatwiseDbContext.cs`, `Datwise.Data/schema.sql`
- Services: `Datwise.Services/ExampleService.cs`, `Datwise.Services/IExampleService.cs`
- Contracts/Models: `Datwise.Contracts/ExampleContract.cs`, `Datwise.Models/ExampleModel.cs`
- Tests: `Datwise.Tests/ExampleServiceTests.cs`

## How to open in Visual Studio (recommended for WebForms)
1. Install Visual Studio Community with workloads: "ASP.NET and web development" and ".NET desktop development". Ensure .NET 9 SDK and .NET Framework 4.8 targeting pack installed.
2. Open `C:\Users\ronan\workspace\Datwise\Datwise.sln` in Visual Studio.
3. Restore NuGet packages when prompted and build.
4. If `Datwise.WebForms` has missing designer members, right-click project or `Default.aspx` → "Convert to Web Application" in Solution Explorer to generate `*.designer.cs`.
5. Set startup projects: Solution properties → Startup Project → Multiple startup projects → Start `Datwise.WebForms` and `Datwise.Api`.
6. Press F5 to run WebForms + API.

## How to run API from CLI (works in VS Code / terminal)
```powershell
cd C:\Users\ronan\workspace\Datwise\Datwise.Api
dotnet run --urls http://localhost:5050
# then in another terminal
curl http://localhost:5050/api/values/1
```

## Local DB (SQLite)
- Connection string: `Data Source=datwise-dev.db` declared in `Datwise.Api/appsettings.Development.json`.
- Create DB from schema: use SQLite CLI or run the SQL in `Datwise.Data/schema.sql`.
- EF Core: `Datwise.Data/DatwiseDbContext.cs` added; if you want migrations, use the EF tools in the Package Manager Console or `dotnet ef`.

## Outstanding tasks / Next steps
- [ ] Open solution in Visual Studio and generate designer for WebForms (or I can add `Default.aspx.designer.cs` if you want quicker CLI builds).
- [ ] Add CRUD endpoints in `Datwise.Api` and corresponding service/repository logic.
- [ ] Create EF Core migrations and seed sample data (SQLite).
- [ ] Add CI (GitHub Actions) to build API and run tests on push.

## Quick GitHub pointers
- Repo: `https://github.com/ronanoshi/Datwise`
- Current branch: `main`

---

If you'd like, I can also create a small Visual Studio `.sln.user` or notes file, or add the `Default.aspx.designer.cs` stub now. Reply with which you'd prefer.
