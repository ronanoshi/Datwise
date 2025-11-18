#!/usr/bin/env pwsh
# Script to initialize Datwise database with test data

$apiPath = "C:\Users\ronan\workspace\Datwise\Datwise.Api"
$dbFile = Join-Path $apiPath "datwise.db"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Datwise Database Initialization Script" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Clean up old database
Write-Host "Step 1: Checking for existing database..." -ForegroundColor Yellow
if (Test-Path $dbFile) {
    Write-Host "Removing existing database file..." -ForegroundColor Yellow
    Remove-Item $dbFile
    Write-Host "? Database file removed" -ForegroundColor Green
} else {
    Write-Host "No existing database found" -ForegroundColor Green
}

Write-Host ""
Write-Host "Step 2: Building the solution..." -ForegroundColor Yellow
cd $apiPath
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "? Build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "? Build succeeded" -ForegroundColor Green

Write-Host ""
Write-Host "Step 3: Starting API (will initialize database)..." -ForegroundColor Yellow
Write-Host "The API will start and automatically create the database with test data." -ForegroundColor Cyan
Write-Host "Press Ctrl+C to stop the API after initialization." -ForegroundColor Cyan
Write-Host ""

# Run the API - it will create and seed the database
dotnet run

Write-Host ""
Write-Host "================================================" -ForegroundColor Green
Write-Host "Database initialization complete!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host ""
Write-Host "Database file location: $dbFile" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Access the API at: https://localhost:53486" -ForegroundColor White
Write-Host "2. View Swagger docs at: https://localhost:53486/swagger" -ForegroundColor White
Write-Host "3. Try the GET /api/issues/open endpoint to see the seeded data" -ForegroundColor White
Write-Host ""
