@echo off
REM Datwise Setup Script for Windows
REM This script helps set up and run the Datwise application

echo ==========================================
echo Datwise Setup and Run Script
echo ==========================================
echo.

REM Check if .NET is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo Error: .NET SDK is not installed
    echo Please install .NET 9.0 SDK from https://dotnet.microsoft.com/en-us/download
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo [OK] .NET SDK found: %DOTNET_VERSION%
echo.

REM Navigate to script directory
cd /d "%~dp0"

echo [INFO] Step 1: Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo [ERROR] Error during restore
    pause
    exit /b 1
)

echo.
echo [INFO] Step 2: Building solution...
dotnet build
if errorlevel 1 (
    echo [ERROR] Error during build
    pause
    exit /b 1
)

echo.
echo [SUCCESS] Build successful!
echo.
echo ==========================================
echo Setup complete! To run the application:
echo ==========================================
echo.
echo Terminal 1 - Start the API:
echo   cd Datwise.Api
echo   dotnet run
echo.
echo Terminal 2 - Start the WebForms:
echo   cd Datwise.WebForms
echo   dotnet run
echo.
echo Access:
echo   - WebForms UI: https://localhost:7290 or http://localhost:5281
echo   - API: https://localhost:7194 or http://localhost:5281
echo   - Swagger Docs: https://localhost:7194/swagger
echo.
pause
