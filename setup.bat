@echo off
REM DentalDrill.CRM Setup Script Launcher
REM Double-click this file to run the setup script

echo Starting DentalDrill.CRM Setup...
echo.

REM Get the directory where this batch file is located
set "SCRIPT_DIR=%~dp0"

REM Check if PowerShell is available
powershell -Command "Get-Host" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Error: PowerShell is not available on this system.
    echo Please install PowerShell or run setup.ps1 manually.
    pause
    exit /b 1
)

REM Run the PowerShell script with execution policy bypass
powershell -ExecutionPolicy Bypass -File "%SCRIPT_DIR%setup.ps1"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Setup completed with errors. Please review the output above.
    pause
    exit /b %ERRORLEVEL%
)

exit /b 0

