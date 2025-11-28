# DentalDrill.CRM Setup Script
# This script installs all required packages and prepares the project for building in Visual Studio
# Run this script after cloning the repository for the first time

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "DentalDrill.CRM Setup Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Get the script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcPath = Join-Path $scriptPath "src"
$projectPath = Join-Path $srcPath "DentalDrill.CRM"

# Check prerequisites
Write-Host "Checking prerequisites..." -ForegroundColor Yellow

# Check .NET SDK
try {
    $dotnetVersion = dotnet --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ .NET SDK found: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "✗ .NET SDK not found. Please install .NET 6 SDK from https://dotnet.microsoft.com/download" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ .NET SDK not found. Please install .NET 6 SDK from https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}

# Check Node.js
try {
    $nodeVersion = node --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Node.js found: $nodeVersion" -ForegroundColor Green
    } else {
        Write-Host "✗ Node.js not found. Please install Node.js 18 LTS from https://nodejs.org/" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ Node.js not found. Please install Node.js 18 LTS from https://nodejs.org/" -ForegroundColor Red
    exit 1
}

# Check npm
try {
    $npmVersion = npm --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ npm found: $npmVersion" -ForegroundColor Green
    } else {
        Write-Host "✗ npm not found. Please install Node.js which includes npm" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ npm not found. Please install Node.js which includes npm" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "All prerequisites found!" -ForegroundColor Green
Write-Host ""

# Step 1: Install npm packages in src directory
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Step 1: Installing npm packages in src/" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Push-Location $srcPath
try {
    if (Test-Path "package.json") {
        Write-Host "Installing packages from src/package.json..." -ForegroundColor Yellow
        npm install
        if ($LASTEXITCODE -ne 0) {
            Write-Host "✗ Failed to install npm packages in src/" -ForegroundColor Red
            exit 1
        }
        Write-Host "✓ npm packages installed in src/" -ForegroundColor Green
    } else {
        Write-Host "⚠ package.json not found in src/, skipping..." -ForegroundColor Yellow
    }
} catch {
    Write-Host "✗ Error installing npm packages in src/: $_" -ForegroundColor Red
    exit 1
} finally {
    Pop-Location
}

Write-Host ""

# Step 2: Install npm packages in DentalDrill.CRM directory
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Step 2: Installing npm packages in DentalDrill.CRM/" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Push-Location $projectPath
try {
    if (Test-Path "package.json") {
        Write-Host "Installing packages from DentalDrill.CRM/package.json..." -ForegroundColor Yellow
        Write-Host "This may take a few minutes..." -ForegroundColor Yellow
        npm install
        if ($LASTEXITCODE -ne 0) {
            Write-Host "✗ Failed to install npm packages in DentalDrill.CRM/" -ForegroundColor Red
            exit 1
        }
        Write-Host "✓ npm packages installed in DentalDrill.CRM/" -ForegroundColor Green
    } else {
        Write-Host "✗ package.json not found in DentalDrill.CRM/" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ Error installing npm packages in DentalDrill.CRM/: $_" -ForegroundColor Red
    exit 1
} finally {
    Pop-Location
}

Write-Host ""

# Step 3: Restore NuGet packages
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Step 3: Restoring NuGet packages" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Push-Location $srcPath
try {
    $solutionPath = Join-Path $srcPath "DentalDrill.CRM.sln"
    if (Test-Path $solutionPath) {
        Write-Host "Restoring NuGet packages from solution..." -ForegroundColor Yellow
        dotnet restore $solutionPath
        if ($LASTEXITCODE -ne 0) {
            Write-Host "✗ Failed to restore NuGet packages" -ForegroundColor Red
            exit 1
        }
        Write-Host "✓ NuGet packages restored" -ForegroundColor Green
    } else {
        Write-Host "✗ Solution file not found: $solutionPath" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ Error restoring NuGet packages: $_" -ForegroundColor Red
    exit 1
} finally {
    Pop-Location
}

Write-Host ""

# Step 4: Build the solution to verify everything works
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Step 4: Building solution to verify setup" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Push-Location $srcPath
try {
    Write-Host "Building solution (this may take a few minutes)..." -ForegroundColor Yellow
    dotnet build $solutionPath --no-restore
    if ($LASTEXITCODE -ne 0) {
        Write-Host "✗ Build failed. Please check the errors above." -ForegroundColor Red
        Write-Host "⚠ You may still be able to build in Visual Studio after fixing any issues." -ForegroundColor Yellow
        exit 1
    }
    Write-Host "✓ Build succeeded!" -ForegroundColor Green
} catch {
    Write-Host "✗ Error building solution: $_" -ForegroundColor Red
    Write-Host "⚠ You may still be able to build in Visual Studio after fixing any issues." -ForegroundColor Yellow
    exit 1
} finally {
    Pop-Location
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Setup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Open src/DentalDrill.CRM.sln in Visual Studio 2022" -ForegroundColor White
Write-Host "2. Set DentalDrill.CRM as the startup project" -ForegroundColor White
Write-Host "3. Press F5 to build and run" -ForegroundColor White
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

