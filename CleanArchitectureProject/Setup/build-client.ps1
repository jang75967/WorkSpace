# build-client.ps1
# DMS Client Build Script
# Usage: .\build-client.ps1 [-OutputPath <path>] [-Mode <Debug|Release>] [-Archive] [-ArchiveOnly] [-ShowLog] [-App <app_name1,app_name2,...>]

[CmdletBinding()]
param (
    [Parameter(Position = 0)]
    [string]$OutputPath,
    
    [Parameter(Position = 1)]
    [ValidateSet("Debug", "Release")]
    [string]$Mode = "Release",
    
    [switch]$Archive = $false,
    [switch]$ArchiveOnly = $false,
    [switch]$ShowLog = $false,
    [string[]]$App = @()
)

# ANSI Color Codes
$Colors = @{
    Reset = "`e[0m"
    Bold = "`e[1m"
    Red = "`e[91m"
    Yellow = "`e[93m"
    Blue = "`e[94m"
    Cyan = "`e[96m"
    Green = "`e[92m"
}

# Script Configuration
$ScriptConfig = @{
    BaseDir = (Get-Location).Path
    BuildDir = Join-Path (Get-Location).Path "..\Src"
    Framework = "net8.0-windows"
    Verbosity = if ($ShowLog) { "normal" } else { "minimal" }
}

# Client App Definitions
$ClientApps = @()

# Add client apps one by one
$ClientApps += @{
    Name = "Client.Apps.AdminTool"
    ProjectSubDir = "Apps"
    OutputDir = ""
    DisplayName = "AdminTool"
}

$ClientApps += @{
    Name = "Client.Apps.Client"
    ProjectSubDir = "Apps"
    OutputDir = ""
    DisplayName = "Client"
}

$ClientApps += @{
    Name = "Client.Apps.Configurator"
    ProjectSubDir = "Apps"
    OutputDir = ""
    DisplayName = "Configurator"
}

$ClientApps += @{
    Name = "Client.Apps.ManualTrackOutSimulator"
    ProjectSubDir = "Apps"
    OutputDir = ""
    DisplayName = "ManualTrackOutSimulator"
}

# Initialize OutputPath
if ([string]::IsNullOrEmpty($OutputPath)) {
    $OutputPath = Join-Path $ScriptConfig.BaseDir "DMS25-Client"
}

# Functions
function Write-Title {
    param([string]$Title)
    Write-Host " "
    Write-Host "$($Colors.Blue)$($Colors.Bold)=== $Title ===$($Colors.Reset)"
}

function Write-Info {
    param([string]$Message)
    Write-Host "$($Colors.Cyan)$Message$($Colors.Reset)"
}

function Write-Success {
    param([string]$Message)
    Write-Host "$($Colors.Green)$Message$($Colors.Reset)"
}

function Write-Warning {
    param([string]$Message)
    Write-Host "$($Colors.Yellow)$Message$($Colors.Reset)"
}

function Write-Error {
    param([string]$Message)
    Write-Host "$($Colors.Red)$Message$($Colors.Reset)"
}

function Test-Command {
    param([string]$Command)
    try {
        Get-Command $Command -ErrorAction Stop | Out-Null
        return $true
    }
    catch {
        return $false
    }
}

function Remove-DirectoryIfExists {
    param([string]$Path)
    if (Test-Path $Path) {
        Write-Info "Removing directory: $Path"
        Remove-Item -Recurse -Force $Path -ErrorAction Stop
    }
}

function Build-ClientApp {
    param(
        $AppObject,
        [string]$OutputPath
    )
    
    Write-Title -Title "Building $($AppObject.DisplayName) App"

    $projectName = $AppObject.Name
    $outputDir = $OutputPath  # 모든 앱의 빌드 산출물을 루트에 배치
    $projectPath = "$($ScriptConfig.BuildDir)\Client\$($AppObject.ProjectSubDir)\$projectName\$projectName.csproj"

    # Output directory는 이미 메인에서 정리되었으므로 여기서는 정리하지 않음
    
    # Verify project exists
    if (-not (Test-Path $projectPath)) {
        Write-Error "Project not found: $projectPath"
        return $false
    }
    
    # Publish project to get complete package
    $publishArgs = @(
    "publish",
    $projectPath,
    "--configuration", $Mode,
    "--framework", $ScriptConfig.Framework,
    "--output", $outputDir,

    # 로그 최소화
    "-v:quiet",                   # minimal 보다 더 조용 (필요시 minimal 로 낮춰도 OK)
    "-nologo",
    "-clp:ErrorsOnly;NoSummary;NoItemAndPropertyList"  # 콘솔 로거: 에러만, 요약/아이템 목록 숨김

    # Windows 전용 SDK 경고 방지(비 Windows 환경에서 WPF/WinForms 타겟 시 자주 뜨는 경고)
    "/p:EnableWindowsTargeting=true"
    )
    
    Write-Info "Running: dotnet $($publishArgs -join ' ')"
    
    & dotnet @publishArgs
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Publish failed for $projectName"
        Write-Error "Publish process failed"
        Write-Error "Script terminated due to publish failure."
        Exit $LASTEXITCODE
    }
    
    Write-Success "Successfully built $projectName"
}


function Compress-ClientApps {
    param([string]$SourceDir)
    
    $archiveFilename = Join-Path $SourceDir "dms-client.zip"
    
    Write-Title -Title "Creating Archive: $archiveFilename"
    
    if (Test-Path $archiveFilename) {
        Remove-Item -Force $archiveFilename
    }
    
    try {
        # Create archive with all files in the source directory
        $filesToArchive = Get-ChildItem -Path $SourceDir -File
        if ($filesToArchive.Count -gt 0) {
            Compress-Archive -Path $filesToArchive.FullName -DestinationPath $archiveFilename -ErrorAction Stop
        }
        
        Write-Success "Archive created successfully: $archiveFilename"
    }
    catch {
        Write-Error "Failed to create archive: $($_.Exception.Message)"
        return $false
    }
    
    return $true
}

function Test-Prerequisites {
    if (-not (Test-Command "dotnet")) {
        Write-Error ".NET SDK is not installed or not in PATH"
        exit 1
    }
    
    if (-not (Test-Path $ScriptConfig.BuildDir)) {
        Write-Error "Build directory not found: $($ScriptConfig.BuildDir)"
        exit 1
    }
}

function Get-AppByName {
    param([string]$AppName)
    
    Write-Info "Searching for app: '$AppName'"
    
    # Try to find by display name first (case-insensitive)
    $app = $ClientApps | Where-Object { $_.DisplayName -eq $AppName -or $_.DisplayName.ToLower() -eq $AppName.ToLower() } | Select-Object -First 1
    if ($app) {
        Write-Info "Found app by display name: $($app.DisplayName)"
        return @{
            Name = $app.Name
            ProjectSubDir = $app.ProjectSubDir
            OutputDir = $app.OutputDir
            DisplayName = $app.DisplayName
        }
    }
    
    # Try to find by full project name
    $app = $ClientApps | Where-Object { $_.Name -eq $AppName -or $_.Name.ToLower() -eq $AppName.ToLower() } | Select-Object -First 1
    if ($app) {
        Write-Info "Found app by full name: $($app.Name)"
        return @{
            Name = $app.Name
            ProjectSubDir = $app.ProjectSubDir
            OutputDir = $app.OutputDir
            DisplayName = $app.DisplayName
        }
    }
    
    # Try to find by partial name
    $app = $ClientApps | Where-Object { $_.Name.ToLower().Contains($AppName.ToLower()) -or $_.DisplayName.ToLower().Contains($AppName.ToLower()) } | Select-Object -First 1
    if ($app) {
        Write-Info "Found app by partial name: $($app.DisplayName)"
        return @{
            Name = $app.Name
            ProjectSubDir = $app.ProjectSubDir
            OutputDir = $app.OutputDir
            DisplayName = $app.DisplayName
        }
    }
    
    Write-Warning "No app found matching: '$AppName'"
    return $null
}

function Show-AvailableApps {
    Write-Info "Available client apps:"
    foreach ($app in $ClientApps) {
        Write-Host "  - $($app.DisplayName) (or '$($app.Name)')"
    }
    Write-Host ""
}

# Main Script Execution
Write-Title -Title "DMS Client Build Script"
Write-Info "Mode: $Mode"
Write-Info "Output Path: $OutputPath"
Write-Info "Archive: $Archive"
Write-Info "Archive Only: $ArchiveOnly"

if ($App.Count -gt 0) {
    Write-Info "Target Apps: $($App -join ', ')"
}

# Test prerequisites
Test-Prerequisites

# Archive only mode
if ($ArchiveOnly) {
    Compress-ClientApps -SourceDir $OutputPath
    exit 0
}

# Determine which apps to build
if ($App.Count -eq 0) {
    Write-Title -Title "Building All Client Apps"
} else {
    Write-Title -Title "Building Apps: $($App -join ', ')"
}

# Clean output directory (only once at the beginning)
Remove-DirectoryIfExists $OutputPath
New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null

# Process apps based on whether we're building all or specific apps
if ($App.Count -eq 0) {
    # Build all apps
    foreach ($buildApp in $ClientApps) {
        Build-ClientApp -AppObject $buildApp -OutputPath $OutputPath
    }
} else {
    # Build specific apps
    $targetApps = @()
    $notFoundApps = @()
    
    foreach ($appName in $App) {
        $appName = $appName.Trim()
        $targetApp = Get-AppByName -AppName $appName
        
        if ($null -eq $targetApp) {
            $notFoundApps += $appName
        } else {
            $targetApps += $targetApp
        }
    }
    
    if ($notFoundApps.Count -gt 0) {
        Write-Error "Apps not found: $($notFoundApps -join ', ')"
        Show-AvailableApps
        exit 1
    }
    
    # Build each target app
    foreach ($targetApp in $targetApps) {
        Build-ClientApp -AppObject $targetApp -OutputPath $OutputPath
    }
}

# Create archive if requested
if ($Archive) {
    Compress-ClientApps -SourceDir $OutputPath
}

Write-Title -Title "Build Complete"