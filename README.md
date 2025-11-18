# Datwise

Datwise is an example ASP.NET WebForms + Web API solution scaffold. This workspace is a starter skeleton to help you build a traditional WebForms site that calls a REST API for server-side operations.

## Projects
- Datwise.WebForms - Classic ASP.NET WebForms app (targeting .NET Framework 4.8). Contains UI pages and client-side logic that consumes the Web API.
- Datwise.Api - Web API (Web API 2) providing REST endpoints consumed by WebForms.
- Datwise.Services - Core application services.
- Datwise.Contracts - DTOs and API contracts.
- Datwise.Models - Domain models and entities.
- Datwise.Data - Data access abstractions and repositories (in-memory stub by default).
- Datwise.Tests - Unit test stubs.

> NOTE: Because classic ASP.NET WebForms and Web API 2 target .NET Framework (4.8), open this project with Visual Studio (Professional/Community) for full feature support (designer, WebForms scaffolding, Publish tools).

## How to use
1. Open `Datwise` folder in Visual Studio (recommended for WebForms and Web API 2). You may need to manually add projects to the solution (or use Visual Studio's "Add Existing Project" to import the `.csproj` files we created here).
2. Restore NuGet packages and build the solution.
	- If you prefer to use the .NET CLI for the API and shared libraries: run `dotnet restore` and `dotnet build` from the root folder.
	- Create a solution and add the projects if you want a single Visual Studio solution, e.g.:

```powershell
cd C:\Users\ronan\workspace\Datwise
dotnet new sln -n Datwise
dotnet sln add Datwise.Contracts\Datwise.Contracts.csproj
dotnet sln add Datwise.Models\Datwise.Models.csproj
dotnet sln add Datwise.Services\Datwise.Services.csproj
dotnet sln add Datwise.Data\Datwise.Data.csproj
dotnet sln add Datwise.Api\Datwise.Api.csproj
dotnet sln add Datwise.WebForms\Datwise.WebForms.csproj
dotnet sln add Datwise.Tests\Datwise.Tests.csproj
dotnet restore
dotnet build
```
3. Start `Datwise.Api` (IIS Express) and `Datwise.WebForms` (IIS Express) in Visual Studio.

### Using the API with the dotnet CLI

1. From an elevated PowerShell terminal or normal terminal where dotnet is available:

```powershell
cd C:\Users\ronan\workspace\Datwise\Datwise.Api
dotnet run
```

2. By default ASP.NET Core will start on a Kestrel listening port (e.g. http://localhost:5000). Update the WebForms `Default.aspx.cs` `CallApi` URL if needed.

## Next steps
- Replace stub services and repository with full implementations.
- Add logging, configuration (appsettings or web.config), and dependency injection.
- Add authentication/authorization for API access.

If you'd like, I can continue by adding working csproj files, Visual Studio solution structure, or automatic CI steps. Tell me which you'd prefer.
