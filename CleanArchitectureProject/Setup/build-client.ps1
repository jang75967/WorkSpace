# build-client.ps1
# DMS Client Build Script
# Usage: .\build-client.ps1 [-Mode <Debug|Release>] [-Flavor <Memory|Foundry>] [-ShowLog] [-App <app_name1,app_name2,...>]

[CmdletBinding()]
param (
    [Parameter(Position = 0)]
    [ValidateSet("Debug", "Release")]
    [string]$Mode = "Release",

    [Parameter(Position = 1)]
    [ValidateSet("Memory", "Foundry")]
    [string]$Flavor = "Memory",

    [switch]$ShowLog = $false,

    # DisplayName 또는 csproj명 일부로 필터링하고 싶을 때 사용 (선택)
    [string[]]$App = @()
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ANSI Color Codes
$Colors = @{
    Reset  = "`e[0m"
    Bold   = "`e[1m"
    Red    = "`e[91m"
    Yellow = "`e[93m"
    Blue   = "`e[94m"
    Cyan   = "`e[96m"
    Green  = "`e[92m"
}

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

function Write-Warn {
    param([string]$Message)
    Write-Host "$($Colors.Yellow)$Message$($Colors.Reset)"
}

function Write-Fail {
    param([string]$Message)
    Write-Host "$($Colors.Red)$Message$($Colors.Reset)"
}

function Test-Command {
    param([string]$Command)
    try {
        Get-Command $Command -ErrorAction Stop | Out-Null
        return $true
    } catch {
        return $false
    }
}

# Script paths
$srcPath = Join-Path $PSScriptRoot "..\Src"
if (-not (Test-Path $srcPath)) {
    throw "Src directory not found: $srcPath"
}

$ScriptConfig = @{
    BuildDir  = (Resolve-Path $srcPath).Path   # repo\Src
    Framework = "net8.0-windows"
    Verbosity = if ($ShowLog) { "normal" } else { "minimal" }
}

# Client Apps 정의
$ClientApps = @()
$ClientApps += @{ Name="Mirero.DMS.Client.Apps.AdminTool"; ProjectSubDir="Apps"; OutputDir=""; DisplayName="AdminTool" }
$ClientApps += @{ Name="Mirero.DMS.Client.Apps.Client";                 ProjectSubDir="Apps"; DisplayName="Client" }
$ClientApps += @{ Name="Mirero.DMS.Client.Apps.Configurator";           ProjectSubDir="Apps"; DisplayName="Configurator" }
$ClientApps += @{ Name="Mirero.DMS.Client.Apps.ManualTrackOutSimulator";ProjectSubDir="Apps"; DisplayName="ManualTrackOutSimulator" }

if ($Flavor -eq "Foundry") {
  $ClientApps += @{ Name="Mirero.DMS.Client.Apps.AutoBBT";               ProjectSubDir="Apps"; DisplayName="AutoBBT" }
  $ClientApps += @{ Name="Mirero.DMS.Client.Apps.LotStatusBoard";        ProjectSubDir="Apps"; DisplayName="LotStatusBoard" }
  $ClientApps += @{ Name="Mirero.DMS.Client.Apps.AutoTrackOutSimulator"; ProjectSubDir="Apps"; DisplayName="AutoTrackOutSimulator" }
}

function Resolve-TargetApps {
    param([hashtable[]]$AllApps, [string[]]$Filter)

    if (-not $Filter -or $Filter.Count -eq 0) { return $AllApps }

    $filterLower = $Filter | ForEach-Object { $_.ToLower() }

    $targets = $AllApps | Where-Object {
        $dn = $_.DisplayName.ToLower()
        $nm = $_.Name.ToLower()
        ($filterLower | Where-Object { $dn -eq $_ -or $nm -eq $_ -or $dn.Contains($_) -or $nm.Contains($_) }).Count -gt 0
    }

    return $targets
}

function Build-ClientApp {
    param(
        [hashtable]$AppObject
    )

    $projectName = $AppObject.Name
    $projectPath = Join-Path $ScriptConfig.BuildDir ("Client\{0}\{1}\{1}.csproj" -f $AppObject.ProjectSubDir, $projectName)

    Write-Title "Building $($AppObject.DisplayName)"

    if (-not (Test-Path $projectPath)) {
        throw "Project not found: $projectPath"
    }

    # dotnet build args
    $buildArgs = @(
        "build",
        $projectPath,
        "--configuration", $Mode,
        "--framework", $ScriptConfig.Framework,
        "--nologo",
        "--verbosity", $ScriptConfig.Verbosity,
        "/p:EnableWindowsTargeting=true"
    )

    Write-Info "Running: dotnet $($buildArgs -join ' ')"

    $buildOutput = & dotnet @buildArgs 2>&1
    $exit = $LASTEXITCODE

    if ($exit -ne 0) {
        # 실패 로그 저장
        $logDir = Join-Path "D:\Client_ci_logs" $Flavor
        New-Item -ItemType Directory -Force -Path $logDir | Out-Null
        $logFile = Join-Path $logDir ("build_{0}_{1}.log" -f $AppObject.DisplayName, (Get-Date -Format "yyyyMMdd_HHmmss"))
        $buildOutput | Out-File -FilePath $logFile -Encoding utf8

        Write-Host ""
        Write-Fail "================ dotnet build FAILED ================"
        Write-Host "Project : $projectName" -ForegroundColor Yellow
        Write-Host "Path    : $projectPath" -ForegroundColor Yellow
        Write-Host "Exit    : $exit" -ForegroundColor Yellow
        Write-Host "LogFile : $logFile" -ForegroundColor Yellow
        Write-Fail "====================================================="

        Write-Host ""
        Write-Host "---- Error lines ----" -ForegroundColor Cyan
        $buildOutput |
          Select-String -Pattern ":\s*error\s+", "MSB\d{4}", "NETSDK\d{4}", "NU\d{4}", "fatal", "failed" -CaseSensitive:$false |
          Select-Object -First 120 |
          ForEach-Object { Write-Host $_.Line }

        throw "Build failed for $projectName (exit=$exit)"
    }

    Write-Success "Successfully built $projectName"
}

# ---- Main ----
Write-Title "DMS Client Build Script"
Write-Info "Mode   : $Mode"
Write-Info "Flavor : $Flavor"
if ($App.Count -gt 0) { Write-Info "Target : $($App -join ', ')" }

if (-not (Test-Command "dotnet")) {
    throw ".NET SDK is not installed or not in PATH"
}

$targets = Resolve-TargetApps -AllApps $ClientApps -Filter $App

if (-not $targets -or $targets.Count -eq 0) {
    Write-Warn "No matching apps to build."
    exit 0
}

foreach ($t in $targets) {
    Build-ClientApp -AppObject $t
}

Write-Title "Build Complete"
exit 0