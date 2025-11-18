#!/usr/bin/env pwsh
# Script to clean up any running processes on Datwise ports

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Datwise Port Cleanup Script" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Ports to check
$ports = @(53486, 53487, 53485, 53488)

Write-Host "Checking for processes using Datwise ports..." -ForegroundColor Yellow
Write-Host ""

$found = $false
foreach ($port in $ports) {
    # Get all processes using this port
    $connections = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue | Where-Object { $_.State -eq 'Listen' }
    
    if ($connections) {
        $found = $true
        foreach ($connection in $connections) {
            $process = Get-Process -Id $connection.OwningProcess -ErrorAction SilentlyContinue
            if ($process) {
                Write-Host "Port $port is in use by: $($process.ProcessName) (PID: $($process.Id))" -ForegroundColor Red
                Write-Host "Killing process $($process.ProcessName)..." -ForegroundColor Yellow
                Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
                Write-Host "? Process killed" -ForegroundColor Green
            }
        }
    } else {
        Write-Host "Port $port: Available" -ForegroundColor Green
    }
}

if (-not $found) {
    Write-Host ""
    Write-Host "? All ports are available" -ForegroundColor Green
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Green
Write-Host "Port cleanup complete!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host ""
Write-Host "You can now start the applications:" -ForegroundColor Cyan
Write-Host "  1. API:       dotnet run (in Datwise.Api folder)" -ForegroundColor White
Write-Host "  2. WebForms:  dotnet run (in Datwise.WebForms folder)" -ForegroundColor White
Write-Host ""
