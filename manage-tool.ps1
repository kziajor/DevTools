#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet("install", "update", "uninstall", "build")]
    [string]$Action,
    [Parameter(Mandatory = $false)]
    [bool]$Force = $false
)

$ErrorActionPreference = "Stop"

# Configuration
$ProjectPath = "Console\DevTools.Console.csproj"
$PackageDir = "Console\Package"
$ToolName = "DevTools.Console"
$ToolCommand = "dev"

function Write-Header {
    param([string]$Message)
    Write-Host ""
    Write-Host "=============================================" -ForegroundColor Blue
    Write-Host $Message -ForegroundColor Yellow
    Write-Host "=============================================" -ForegroundColor Blue
    Write-Host ""
}

function Test-ToolInstalled {
    try {
        $result = dotnet tool list --global | Select-String $ToolName
        return $null -ne $result
    }
    catch {
        return $false
    }
}

function Build-Project {
    Write-Header "Building DevTools.Console"
    
    # Clean previous package directory
    if (Test-Path $PackageDir) {
        Write-Host "Cleaning previous package directory..." -ForegroundColor Yellow
        Remove-Item $PackageDir -Recurse -Force
    }
    
    # Create package directory
    New-Item -ItemType Directory -Path $PackageDir -Force | Out-Null
    
    # Build and pack the project
    Write-Host "Building project..." -ForegroundColor Green
    dotnet clean $ProjectPath --configuration Release
    dotnet build $ProjectPath --configuration Release
    
    Write-Host "Packing project..." -ForegroundColor Green
    dotnet pack $ProjectPath --configuration Release --output $PackageDir
    
    # Verify package was created
    $packageFile = Get-ChildItem $PackageDir -Filter "*.nupkg" | Select-Object -First 1
    if (-not $packageFile) {
        throw "Package file was not created successfully"
    }
    
    Write-Host "Package created: $($packageFile.Name)" -ForegroundColor Green
    return $packageFile.FullName
}

function Install-Tool {
    Write-Header "Installing DevTools Console Tool"
    
    # Check if already installed
    if (Test-ToolInstalled) {
        Write-Host "Tool is already installed. Use 'update' action to update it." -ForegroundColor Yellow
        return
    }
    
    # Build the project first
    $packagePath = Build-Project
    
    # Install the tool
    Write-Host "Installing tool globally..." -ForegroundColor Green
    dotnet tool install --global --add-source $PackageDir $ToolName
    
    Write-Host ""
    Write-Host "✅ DevTools Console Tool installed successfully!" -ForegroundColor Green
    Write-Host "   Run the tool using: $ToolCommand" -ForegroundColor Cyan
    Write-Host ""
}

function Update-Tool {
    Write-Header "Updating DevTools Console Tool"
    
    # Check if installed
    if (-not (Test-ToolInstalled)) {
        Write-Host "Tool is not installed. Use 'install' action to install it first." -ForegroundColor Yellow
        return
    } else {
        if ($Force) {
            Write-Host "Force update enabled. Uninstalling existing tool..." -ForegroundColor Yellow
            Uninstall-Tool
            Install-Tool
            return
        }
    }
    
    # Build the project first
    $packagePath = Build-Project
    
    # Update the tool
    Write-Host "Updating tool..." -ForegroundColor Green
    dotnet tool update --global --add-source $PackageDir $ToolName
    
    Write-Host ""
    Write-Host "✅ DevTools Console Tool updated successfully!" -ForegroundColor Green
    Write-Host "   Run the tool using: $ToolCommand" -ForegroundColor Cyan
    Write-Host ""
}

function Uninstall-Tool {
    Write-Header "Uninstalling DevTools Console Tool"
    
    # Check if installed
    if (-not (Test-ToolInstalled)) {
        Write-Host "Tool is not installed." -ForegroundColor Yellow
        return
    }
    
    # Uninstall the tool
    Write-Host "Uninstalling tool..." -ForegroundColor Green
    dotnet tool uninstall --global $ToolName
    
    # Clean package directory
    if (Test-Path $PackageDir) {
        Write-Host "Cleaning package directory..." -ForegroundColor Yellow
        Remove-Item $PackageDir -Recurse -Force
    }
    
    Write-Host ""
    Write-Host "✅ DevTools Console Tool uninstalled successfully!" -ForegroundColor Green
    Write-Host ""
}

function Show-Usage {
    Write-Host ""
    Write-Host "DevTools Console Tool Manager" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  .\manage-tool.ps1 install      # Build and install the tool" -ForegroundColor Gray
    Write-Host "  .\manage-tool.ps1 update       # Build and update the tool" -ForegroundColor Gray
    Write-Host "  .\manage-tool.ps1 uninstall    # Uninstall the tool" -ForegroundColor Gray
    Write-Host "  .\manage-tool.ps1 build        # Build the project only" -ForegroundColor Gray
    Write-Host ""
    Write-Host "After installation, run the tool using: $ToolCommand" -ForegroundColor Cyan
    Write-Host ""
}

# Main execution
try {
    # Verify we're in the correct directory
    if (-not (Test-Path $ProjectPath)) {
        throw "Project file not found: $ProjectPath. Make sure you're running this script from the solution root directory."
    }
    
    switch ($Action.ToLower()) {
        "install" { Install-Tool }
        "update" { Update-Tool }
        "uninstall" { Uninstall-Tool }
        "build" { Build-Project | Out-Null; Write-Host "✅ Build completed successfully!" -ForegroundColor Green }
        default { Show-Usage }
    }
}
catch {
    Write-Host ""
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    exit 1
}
