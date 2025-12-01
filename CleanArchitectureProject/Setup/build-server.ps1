# build-server.ps1
# DMS Server Build Script
# Usage: .\build-server.ps1 [-OutputPath <path>] [-Mode <Debug|Release>] [-Archive] [-ArchiveOnly] [-ShowLog] [-Service <service_name1,service_name2,...>]

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
    [string[]]$Service = @()
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
    Framework = "net8.0"
    Verbosity = if ($ShowLog) { "normal" } else { "minimal" }
}

# Service Definitions
$Services = @()

# Add services one by one to avoid array initialization issues
$Services += @{
    Name = "Server.Service.Detector"
    ServiceSubDir = $null
    OutputDir = "dms.detector"
    SpecialHandling = $null
    DisplayName = "Detector"
}

$Services += @{
    Name = "Server.Service.Loader"
    ServiceSubDir = $null
    OutputDir = "dms.loader"
    SpecialHandling = "TIBCO"
    DisplayName = "Loader"
}

$Services += @{
    Name = "Server.Service.Transmitter"
    ProjectSubDir = $null
    OutputDir = "dms.transmitter"
    SpecialHandling = $null
    DisplayName = "Transmitter"
}

$Services += @{
    Name = "Server.Service.Trigger"
    ProjectSubDir = $null
    OutputDir = "dms.trigger"
    SpecialHandling = $null
    DisplayName = "Trigger"
}

$Services += @{
    Name = "Server.Service.Scheduler"
    ProjectSubDir = $null
    OutputDir = "dms.scheduler.eds"
    SpecialHandling = "AutoDelete"
    DisplayName = "Scheduler"
}

$Services += @{
    Name = "Server.Service.LogManager"
    ProjectSubDir = $null
    OutputDir = "dms.logmanager"
    SpecialHandling = $null
    DisplayName = "LogManager"
}

$Services += @{
    Name = "Server.Service.API.AdminTool"
    ProjectSubDir = "API"
    OutputDir = "dms.admintool.api"
    SpecialHandling = $null
    DisplayName = "AdminTool"
}

$Services += @{
    Name = "Server.Service.API.DataManager"
    ProjectSubDir = "API"
    OutputDir = "dms.datamanager.api"
    SpecialHandling = $null
    DisplayName = "DataManager"
}

# Initialize OutputPath
if ([string]::IsNullOrEmpty($OutputPath)) {
    $OutputPath = $ScriptConfig.BaseDir
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

function Build-Service {
    param(
        $ServiceObject,
        [string]$OutputPath
    )
    
    Write-Title -Title "Building $($ServiceObject.DisplayName) Service"

    $projectName = $ServiceObject.Name
    $outputDir = Join-Path $OutputPath $ServiceObject.OutputDir "app"
    $projectPath = Join-Path $ScriptConfig.BuildDir "Server" "Service" $ServiceObject.ProjectSubDir $projectName "$projectName.csproj"
    
    # Clean output directory
    Remove-DirectoryIfExists $outputDir
    
    # Verify project exists
    if (-not (Test-Path $projectPath)) {
        Write-Error "Project not found: $projectPath"
        return $false
    }
    
    # Build project
    $buildArgs = @(
        "publish",
        $projectPath,
        "--configuration", $Mode,
        "--framework", $ScriptConfig.Framework,
        "--output", $outputDir,
        "--verbosity", $ScriptConfig.Verbosity
    )
    
    Write-Info "Running: dotnet $($buildArgs -join ' ')"
    
    & dotnet @buildArgs
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed for $projectName"
        Write-Error "Build process failed"
        Write-Error "Script terminated due to build failure."
        Exit $LASTEXITCODE
    }
    
    Write-Success "Successfully built $projectName"
}

function Invoke-TIBCOFiles {
    param([string]$OutputPath)
    
    $loaderAppDir = Join-Path $OutputPath "dms.loader" "app"
    $rendezvousDir = Join-Path $ScriptConfig.BuildDir "Common" "External" "Rendezvous" "Linux"
    
    Write-Info "Invoking TIBCO Rendezvous files for dms.loader..."
    
    # Check if loader app directory exists
    if (-not (Test-Path $loaderAppDir)) {
        Write-Warning "Loader app directory not found: $loaderAppDir"
        return
    }
    
    # Remove Windows TIBCO DLLs
    Get-ChildItem -Path $loaderAppDir -Filter "tibrv*.dll" | Remove-Item -Force
    
    # Copy Linux TIBCO files
    $linuxFiles = @(
        @{ Source = "libtibrv64.so"; Destination = "libtibrv.so" },
        @{ Source = "TIBCO.Rendezvous.dll"; Destination = "TIBCO.Rendezvous.dll" }
    )
    
    foreach ($file in $linuxFiles) {
        $sourcePath = Join-Path $rendezvousDir $file.Source
        $destPath = Join-Path $loaderAppDir $file.Destination
        
        if (Test-Path $sourcePath) {
            Copy-Item -Path $sourcePath -Destination $destPath -Force
            Write-Info "Copied $($file.Source) to $($file.Destination)"
        } else {
            Write-Warning "Source file not found: $sourcePath"
        }
    }
}

function Copy-SchedulerAutodelete {
    param([string]$OutputPath)
    
    Write-Title -Title "Setting up Scheduler AutoDelete"
    
    $sourceDir = Join-Path $OutputPath "dms.scheduler.eds" "app"
    $destDir = Join-Path $OutputPath "dms.scheduler.autodelete" "app"
    
    Remove-DirectoryIfExists $destDir
    
    Write-Info "Source: $sourceDir"
    Write-Info "Destination: $destDir"
    
    Copy-Item -Recurse -Force -Path $sourceDir -Destination $destDir
    Write-Success "Scheduler AutoDelete configured successfully"
}

function Copy-ConfigFiles {
    param([string]$OutputPath)
    
    Write-Title -Title "Copying Configuration Files"
    
    $configDir = Join-Path $OutputPath "config"
    $settingsSource = Join-Path $ScriptConfig.BuildDir "Server" "Service" "Server.Service.Settings"
    
    Remove-DirectoryIfExists $configDir
    New-Item -ItemType Directory -Path $configDir -Force | Out-Null
    
    if (Test-Path $settingsSource) {
        Copy-Item -Recurse -Force -Path (Join-Path $settingsSource "*.json") -Destination $configDir
        Write-Success "Configuration files copied successfully"
    } else {
        Write-Warning "Settings source directory not found: $settingsSource"
    }
}

function Copy-LinuxIpp {
    param([string]$OutputPath)
    
    Write-Title -Title "Copying Linux IPP Interop Files"
    
    $sourceDir = Join-Path $ScriptConfig.BuildDir "Common\Infra\Common.Infra.Algorithm.IPP.Interop\Linux"
    $destDir = Join-Path $OutputPath "LinuxIpp"
    
    Remove-DirectoryIfExists $destDir
    
    if (Test-Path $sourceDir) {
        Copy-Item -Recurse -Force -Path $sourceDir -Destination $destDir
        Write-Success "Linux IPP files copied successfully"
    } else {
        Write-Warning "Linux IPP source directory not found: $sourceDir"
    }
}

function Compress-Services {
    param([string]$SourceDir)
    
    $archiveFilename = Join-Path $SourceDir "dms-server.zip"
    
    Write-Title -Title "Creating Archive: $archiveFilename"
    
    if (Test-Path $archiveFilename) {
        Remove-Item -Force $archiveFilename
    }
    
    try {
        # Create archive with config directory
        Compress-Archive -Path (Join-Path $SourceDir "config") -DestinationPath $archiveFilename -ErrorAction Stop
        
        # Add all dms.* directories
        Get-ChildItem -Path $SourceDir -Directory -Name "dms.*" | ForEach-Object {
            $dirPath = Join-Path $SourceDir $_
            Compress-Archive -Path $dirPath -DestinationPath $archiveFilename -Update -ErrorAction Stop
        }
        
        # Add LinuxIpp directory if it exists
        $linuxIppPath = Join-Path $SourceDir "LinuxIpp"
        if (Test-Path $linuxIppPath) {
            Compress-Archive -Path $linuxIppPath -DestinationPath $archiveFilename -Update -ErrorAction Stop
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


function Get-ServiceByName {
    param([string]$ServiceName)
    
    Write-Info "Searching for service: '$ServiceName'"
    
    # Try to find by display name first (case-insensitive)
    $service = $Services | Where-Object { $_.DisplayName -eq $ServiceName -or $_.DisplayName.ToLower() -eq $ServiceName.ToLower() } | Select-Object -First 1
    if ($service) {
        Write-Info "Found service by display name: $($service.DisplayName)"
        # Create a new hashtable to ensure proper structure
        return @{
            Name = $service.Name
            ProjectSubDir = $service.ProjectSubDir
            OutputDir = $service.OutputDir
            SpecialHandling = $service.SpecialHandling
            DisplayName = $service.DisplayName
        }
    }
    
    # Try to find by full project name
    $service = $Services | Where-Object { $_.Name -eq $ServiceName -or $_.Name.ToLower() -eq $ServiceName.ToLower() } | Select-Object -First 1
    if ($service) {
        Write-Info "Found service by full name: $($service.Name)"
        # Create a new hashtable to ensure proper structure
        return @{
            Name = $service.Name
            ProjectSubDir = $service.ProjectSubDir
            OutputDir = $service.OutputDir
            SpecialHandling = $service.SpecialHandling
            DisplayName = $service.DisplayName
        }
    }
    
    # Try to find by partial name
    $service = $Services | Where-Object { $_.Name.ToLower().Contains($ServiceName.ToLower()) -or $_.DisplayName.ToLower().Contains($ServiceName.ToLower()) } | Select-Object -First 1
    if ($service) {
        Write-Info "Found service by partial name: $($service.DisplayName)"
        # Create a new hashtable to ensure proper structure
        return @{
            Name = $service.Name
            ProjectSubDir = $service.ProjectSubDir
            OutputDir = $service.OutputDir
            SpecialHandling = $service.SpecialHandling
            DisplayName = $service.DisplayName
        }
    }
    
    Write-Warning "No service found matching: '$ServiceName'"
    return $null
}

function Show-AvailableServices {
    Write-Info "Available services:"
    foreach ($service in $Services) {
        Write-Host "  - $($service.DisplayName) (or '$($service.Name)')"
    }
    Write-Host ""
}

# Main Script Execution
Write-Title -Title "DMS Server Build Script"
Write-Info "Mode: $Mode"
Write-Info "Output Path: $OutputPath"
Write-Info "Archive: $Archive"
Write-Info "Archive Only: $ArchiveOnly"

if ($Service.Count -gt 0) {
    Write-Info "Target Services: $($Service -join ', ')"
}

# Test prerequisites
Test-Prerequisites

# Unit tests are run separately in CI pipeline via run-tests job

# Archive only mode
if ($ArchiveOnly) {
    Compress-Services -SourceDir $OutputPath
    exit 0
}

# Copy configuration files - only when building all services
if ($Service.Count -eq 0) {
    Copy-ConfigFiles -OutputPath $OutputPath
}

# Determine which services to build
if ($Service.Count -eq 0) {
    Write-Title -Title "Building All Services"
} else {
    Write-Title -Title "Building Services: $($Service -join ', ')"
}

# Process services based on whether we're building all or specific services
if ($Service.Count -eq 0) {
    # Build all services - process each service individually
    foreach ($buildService in $Services) {
        Build-Service -ServiceObject $buildService -OutputPath $OutputPath
    
        # Handle special cases only if build was successful
        if ($buildService.SpecialHandling -eq "TIBCO") {
            Invoke-TIBCOFiles -OutputPath $OutputPath
        }

        # Copy scheduler autodelete (special case)
        if ($buildService.SpecialHandling -eq "AutoDelete") {
            Copy-SchedulerAutodelete -OutputPath $OutputPath
        }
    }
} else {
    # Build specific services
    $targetServices = @()
    $notFoundServices = @()
    
    foreach ($serviceName in $Service) {
        $serviceName = $serviceName.Trim()
        $targetService = Get-ServiceByName -ServiceName $serviceName
        
        if ($null -eq $targetService) {
            $notFoundServices += $serviceName
        } else {
            $targetServices += $targetService
        }
    }
    
    if ($notFoundServices.Count -gt 0) {
        Write-Error "Services not found: $($notFoundServices -join ', ')"
        Show-AvailableServices
        exit 1
    }
    
    # Build each target service
    foreach ($targetService in $targetServices) {
        Build-Service -ServiceObject $targetService -OutputPath $OutputPath
        
        # Handle special cases only if build was successful
        if ($targetService.SpecialHandling -eq "TIBCO") {
            Invoke-TIBCOFiles -OutputPath $OutputPath
        }

        # Copy scheduler autodelete (special case)
        if ($targetService.SpecialHandling -eq "AutoDelete") {
            Copy-SchedulerAutodelete -OutputPath $OutputPath
        }
    }
}

# Copy Linux IPP files - only if building all services
if ($Service.Count -eq 0) {
    Copy-LinuxIpp -OutputPath $OutputPath
}

# Create archive if requested
if ($Archive) {
    Compress-Services -SourceDir $OutputPath
}

Write-Title -Title "Build Complete"