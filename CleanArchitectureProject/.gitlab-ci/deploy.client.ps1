[CmdletBinding()]
param(
  [Parameter(Mandatory=$true)] [string]$SolutionRoot,
  [Parameter(Mandatory=$true)] [string]$TargetRoot,
  [Parameter(Mandatory=$true)] [string]$ProviderBaseUrl,
  [int]$ProviderPort,
  [string]$Flavor = "Memory",
  [string[]]$Apps = @(),                                       # Apps 기본값은 Flavor 기반으로 런타임에서 할당
  [string]$Version,                                            
  [string]$PublishProfile = 'ClickOnceProfile',
  [ValidateSet('x64','Any CPU','x86')] [string]$Platform = 'x64',
  [switch]$MakeIndexHtml,                                      # 타깃에 index.html 없을 때 생성
  [switch]$UseBootstrapper = $true,                            # setup.exe로 런타임 자동 설치
  [string]$BootstrapperPackagesPath,
  [string]$SmbUser, [string]$SmbPassword,
  [switch]$DirectToTarget = $true,                             # Stage 생략
  [switch]$NoClean = $true,                                    # Clean 생략

  # 게시 전용 임시 루트
  [string]$TempRoot = "D:\ClickOnce_Temp"
)

# Flavor 검증
if ([string]::IsNullOrWhiteSpace($Flavor)) { $Flavor = "Memory" }
if ($Flavor -ne "Memory" -and $Flavor -ne "Foundry") { throw "Invalid Flavor: $Flavor (expected Memory|Foundry)" }

# 앱 카탈로그: 키 → 지원 Flavor 목록 (단일 source of truth)
# csproj 경로/어셈블리명은 'Mirero.DMS.Client.Apps.<Key>' 규칙으로 유도됨
$AppCatalog = [ordered]@{
  'AdminTool'      = @('Memory','Foundry')
  'Client'         = @('Memory','Foundry')
  'Configurator'   = @('Memory','Foundry')
  'LotStatusBoard' = @('Memory','Foundry')
  'AutoBBT'        = @('Foundry')
}

# Apps 미지정 시 현재 Flavor에 해당하는 앱을 기본값으로
if (-not $Apps -or $Apps.Count -eq 0) {
  $Apps = @($AppCatalog.Keys | Where-Object { $AppCatalog[$_] -contains $Flavor })
}

# TempRoot 준비
if (-not (Test-Path $TempRoot)) { New-Item -ItemType Directory -Force -Path $TempRoot | Out-Null }

$ErrorActionPreference = 'Stop'
$MSBUILD_FIXED = 'C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe'

function Log([string]$m){ Write-Host "[INFO] $m" -ForegroundColor Cyan }
function Warn([string]$m){ Write-Warning $m }
function Fail([string]$m){ Write-Error $m; throw $m }

function Resolve-MSBuild {
  if (-not (Test-Path $MSBUILD_FIXED)) { return $null }
  return $MSBUILD_FIXED
}

function New-InstallUrl([string]$Base,[int]$Port,[string]$AppFolder){
  try { $b = [System.UriBuilder]$Base } catch { Fail ("Invalid ProviderBaseUrl: {0}" -f $Base) }
  if ($Port) { $b.Port = $Port }
  $b.Path = ($b.Path.TrimEnd('/') + '/' + $AppFolder + '/')
  return $b.Uri.AbsoluteUri
}

function Write-AppIndexHtml([string]$OutDir,[string]$AppName,[string]$Ver){
@"
<!doctype html>
<html lang="ko"><head><meta charset="utf-8" /><title>$AppName</title>
<style>body{font-family:Tahoma,Arial,sans-serif;margin:40px;color:#222}
.card{max-width:720px;border:1px solid #e5e7eb;border-radius:12px;box-shadow:0 4px 12px rgba(0,0,0,.06)}
.hd{background:#1c5280;color:#fff;padding:18px 22px;border-radius:12px 12px 0 0}.hd small{display:block;color:#cbd5e1}
.bd{padding:22px}.row{margin:8px 0}.label{display:inline-block;width:80px;color:#475569}
a.btn{display:inline-block;margin-top:12px;padding:10px 18px;background:#0078d7;color:#fff;text-decoration:none;border-radius:8px}
a.btn:hover{background:#005ea0}</style></head>
<body><div class="card"><div class="hd"><div style="font-size:22px;font-weight:700">$AppName</div><small>mirero</small></div>
<div class="bd"><div class="row"><span class="label">Version</span> <span>$Ver</span></div>
<a class="btn" href="setup.exe">Install / Update</a></div></div></body></html>
"@ | Set-Content -Encoding UTF8 -Path (Join-Path $OutDir 'index.html')
}

# MSBuild 실행 함수
function Invoke-MSBuild([string]$MSBuild,[string]$Csproj,[string]$AppName,[string[]]$MoreArgs,[string]$WorkingDir){
  Push-Location $WorkingDir
  try {
    $result = & $MSBuild $Csproj @MoreArgs 2>&1
    $exitCode = $LASTEXITCODE
    Log ("MSBuild exit code: {0}" -f $exitCode)

    if ($exitCode -ne 0) {
      # 핵심 에러 키워드 추출해서 출력
      $keyErrors = $result | Where-Object {
        $_ -match '(?i)\berror\b|MSB\d{4}|NETSDK\d{4}|NU\d{4}|GenerateBootstrapper|Bootstrapper|ClickOnce|setup\.exe|cannot be found|not found|Access is denied|denied|failed'
      }

      if ($keyErrors -and $keyErrors.Count -gt 0) {
        Log ("---- KEY ERRORS (last 200 lines) ----")
        $keyErrors | Select-Object -Last 200 | ForEach-Object {
          Log ("  {0}" -f $_)
        }
        Log ("---- END KEY ERRORS ----")
      }
      else {
        Log ("No key error lines matched. Showing last 200 lines of MSBuild output.")
        $result | Select-Object -Last 200 | ForEach-Object {
          Log ("  {0}" -f $_)
        }
      }
    }

    return $exitCode
  }
  finally { Pop-Location }
}

# --------------------------- 메인 ---------------------------

# SMB 연결(옵션)
$didNetUse = $false
if ($SmbUser -and $SmbPassword) {
  Log ("Opening SMB session to {0} ..." -f $TargetRoot)
  cmd /c ('net use "{0}" /user:{1} "{2}" /persistent:no' -f $TargetRoot,$SmbUser,$SmbPassword) | Out-Null
  $didNetUse = $true
}

# SUBST 변수 초기화
$didSubst = $false
$substDrive = if ($Flavor -eq "Memory") { "M:" } else { "F:" }

try {
  $env:NUGET_PACKAGES = "C:\Users\mirero\.nuget\packages"
  Log ("NUGET_PACKAGES = {0}" -f $env:NUGET_PACKAGES)

  $msbuildPath = Resolve-MSBuild
  if (-not $msbuildPath) { Fail "MSBuild not found: $MSBUILD_FIXED (VS Build Tools 2022 required)" }
  Log ("MSBuild = {0}" -f $msbuildPath)

  if ([string]::IsNullOrWhiteSpace($Version)) {
    $now = Get-Date; $Version = '{0}.{1}.{2}.{3}' -f $now.Year,$now.Month,$now.Day,$now.Hour
  }
  if (-not (Test-Path $SolutionRoot)) { Fail ("SolutionRoot not found: {0}" -f $SolutionRoot) }

   # LONG PATH 문제: SUBST로 SolutionRoot를 짧게 (ProjectMap 생성 전에 적용)
  $srcRoot = (Resolve-Path $SolutionRoot).Path
  Log ("[SUBST] Original SolutionRoot: {0}" -f $srcRoot)
  
  # 기존 SUBST 제거 (혹시 있을 수 있음)
  cmd /c "subst $substDrive /d" 2>&1 | Out-Null
  
  # SUBST 생성
  $substResult = cmd /c "subst $substDrive `"$srcRoot`"" 2>&1
  if ($LASTEXITCODE -eq 0) {
    $didSubst = $true
    $SolutionRoot = "$substDrive\"
    Log ("[SUBST] SolutionRoot changed to: {0} (original: {1})" -f $SolutionRoot, $srcRoot)
  } else {
    Warn ("[SUBST] Failed to create SUBST drive X:: {0}" -f ($substResult -join "`n"))
    Log ("[SUBST] Continuing with original path (may hit long path issues)")
  }

  if (-not (Test-Path $TargetRoot)) {
    if ($TargetRoot -like "\\*") { Fail ("TargetRoot not found or not accessible: {0}" -f $TargetRoot) }
    else { New-Item -ItemType Directory -Force -Path $TargetRoot | Out-Null }
  }

  foreach ($app in $Apps) {
    if (-not $AppCatalog.Contains($app)) { Fail ("Unknown app: {0}" -f $app) }

    $assemblyName = "Mirero.DMS.Client.Apps.$app"
    $csproj = Join-Path $SolutionRoot ('Src\Client\Apps\{0}\{0}.csproj' -f $assemblyName)
    if (-not (Test-Path $csproj)) { Fail ("Project not found: {0}" -f $csproj) }

     $outDir = Join-Path $TargetRoot $app
     if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Force -Path $outDir | Out-Null }

     # ClickOnce 구조: Application Files\AssemblyName_Version\ 형태로 생성
     $appFilesDir = Join-Path $outDir "Application Files"
     $versionDir = Join-Path $appFilesDir "${assemblyName}_$($Version -replace '\.','_')"
     New-Item -ItemType Directory -Force -Path $appFilesDir | Out-Null
     
     # 현재 버전 폴더만 제거 (다른 버전은 보존) - 최적화된 처리
     if (Test-Path $versionDir) {
       Log ("[{0}] Removing current version directory: {1}" -f $app, $versionDir)
       try {
         Remove-Item -Recurse -Force $versionDir -ErrorAction Stop
         Log ("[{0}] Successfully removed version directory" -f $app)
       } catch {
         Log ("[{0}] Warning: Could not remove version directory: {1}" -f $app, $_.Exception.Message)
         # 계속 진행 (새 폴더가 덮어쓰기됨)
       }
     }
     
     # 기존 버전들 확인 및 보존
     $existingVersions = Get-ChildItem -Path $appFilesDir -Directory | Where-Object { $_.Name -like "${assemblyName}_*" }
     if ($existingVersions) {
       Log ("[{0}] Existing versions preserved: {1}" -f $app, ($existingVersions.Name -join ", "))
     }
     
     New-Item -ItemType Directory -Force -Path $versionDir | Out-Null

     # 로컬 임시 폴더에 ClickOnce 게시
     $projectDir = Split-Path $csproj -Parent
     $unique = $env:CI_PIPELINE_ID
     
     if (-not $unique) { $unique = [guid]::NewGuid().ToString("N") }
     $tempPublishDir = Join-Path $TempRoot ("ClickOncePublish_{0}_{1}_{2}" -f $Flavor, $app, $unique)

     if (Test-Path $tempPublishDir) {
       Remove-Item -Recurse -Force $tempPublishDir
     }
     New-Item -ItemType Directory -Force -Path $tempPublishDir | Out-Null

    $installUrl = New-InstallUrl -Base $ProviderBaseUrl -Port $ProviderPort -AppFolder $app

    Log ("[{0}] ProjectDir : {1}" -f $app, $projectDir)
    Log ("[{0}] TempPublishDir : {1}" -f $app, $tempPublishDir)
    Log ("[{0}] InstallUrl : {1}" -f $app, $installUrl)
    Log ("[{0}] Version    : {1}, Platform: {2}, Bootstrapper: {3}" -f $app, $Version, $Platform, $UseBootstrapper)

    # MSBuild 인자
    $targets = if ($NoClean) { '/t:Publish' } else { '/t:Clean;Publish' }

    Log ("[{0}] Running dotnet restore..." -f $app)
    $devExpressSource = "D:\Client.BuildAssets\DevExpress24.2.3\packages"

    if (Test-Path $devExpressSource) {
      Log ("[{0}] Using DevExpress local source: {1}" -f $app, $devExpressSource)
      $restoreResult = & dotnet restore $csproj -r win-x64 --packages $env:NUGET_PACKAGES --source "https://api.nuget.org/v3/index.json" --source $devExpressSource 2>&1
    } else {
      Log ("[{0}] Warning: DevExpress local source not found" -f $app)
      $restoreResult = & dotnet restore $csproj -r win-x64 --packages $env:NUGET_PACKAGES 2>&1
    }

    if ($LASTEXITCODE -ne 0) { 
      Log ("[{0}] dotnet restore output:" -f $app)
      $restoreResult | ForEach-Object { Log ("  {0}" -f $_) }
      Fail ("[{0}] dotnet restore failed (exit={1})." -f $app, $LASTEXITCODE) 
    }

     $moreArgs = @(
       $targets,
       "/p:PublishProfile=$PublishProfile",
       "/p:Configuration=Release",
       "/p:Platform=$Platform",
       "/p:RestoreDuringBuild=false",
       "/p:RestoreOnBuild=false",
       "/p:NoRestore=true",
       "/p:PublishProtocol=ClickOnce",
       "/p:PublishDir=$tempPublishDir",
       "/p:InstallUrl=$installUrl",
       "/p:ApplicationVersion=$Version",
       "/p:SignManifests=false",
       "/p:GenerateManifests=true",
       "/p:UpdateEnabled=true",
       "/p:UpdateMode=Foreground",
       "/v:m","/nologo"
     )

    if ($UseBootstrapper) {
      # 원하실 때만: setup.exe/런타임 생성/갱신
      $moreArgs += "/p:BootstrapperEnabled=true","/p:BootstrapperComponentsLocation=Relative"
      if ($BootstrapperPackagesPath) { $moreArgs += "/p:BootstrapperPackagesPath=`"$BootstrapperPackagesPath`"" }
    } else {
      # 기본: setup.exe/런타임 폴더는 건드리지 않음
      $moreArgs += "/p:BootstrapperEnabled=false","/p:GenerateBootstrapper=false"
    }

    # 로컬 임시 폴더에 ClickOnce 게시
    $exit = Invoke-MSBuild -MSBuild $msbuildPath -Csproj $csproj -AppName $app -MoreArgs $moreArgs -WorkingDir $projectDir
    if ($exit -ne 0) { Fail ("[{0}] MSBuild publish failed (exit={1})." -f $app,$exit) }

    Log ("[{0}] ClickOnce published to temp directory" -f $app)

    # ClickOnce 결과에서 필요한 파일들만 복사
    Log ("[{0}] Copying ClickOnce files to target..." -f $app)
    
    # 1. Application Files\AppName_Version\ 폴더의 내용을 버전 폴더로 복사
    $tempAppFiles = Join-Path $tempPublishDir "Application Files"
    if (Test-Path $tempAppFiles) {
      $tempVersionDir = Get-ChildItem -Path $tempAppFiles -Directory | Where-Object { $_.Name -like "${assemblyName}_*" } | Sort-Object LastWriteTime -Descending | Select-Object -First 1
      if ($tempVersionDir) {
        Log ("[{0}] Copying version files from: {1}" -f $app, $tempVersionDir.FullName)
        
        # robocopy /MT 멀티스레드로 버전 폴더 전체 복사 (SMB 대량 소파일 가속). Copy-Item 단일 스레드 대비 대폭 단축.
        # robocopy 종료코드: 0~7 정상(1=복사됨 등), 8 이상만 실패. EAP=Stop 에서 비0 종료가 throw 되지 않도록 Continue 로 감쌈.
        $prevEAP = $ErrorActionPreference
        $ErrorActionPreference = 'Continue'
        robocopy $tempVersionDir.FullName $versionDir /E /MT:16 /R:2 /W:2 /NFL /NDL /NP
        $rc = $LASTEXITCODE
        $ErrorActionPreference = $prevEAP
        $global:LASTEXITCODE = 0
        if ($rc -ge 8) { Fail ("[{0}] robocopy failed (exit={1}) copying version files." -f $app, $rc) }
        Log ("[{0}] Copied version files via robocopy (rc={1})" -f $app, $rc)
      }
    }
    
    # 2. 루트 파일들만 복사 (.application, setup.exe)
    $rootFiles = @("$assemblyName.application", "setup.exe")
    foreach ($file in $rootFiles) {
      $sourceFile = Join-Path $tempPublishDir $file
      if (Test-Path $sourceFile) {
        $destFile = Join-Path $outDir $file
        Copy-Item -Path $sourceFile -Destination $destFile -Force
        Log ("[{0}] Copied: {1}" -f $app, $file)
      }
    }
    
    # 3. 임시 폴더 정리
    Remove-Item -Recurse -Force $tempPublishDir
    Log ("[{0}] Cleaned up temp directory" -f $app)

     # index.html 업데이트
     if ($MakeIndexHtml) {
       Write-AppIndexHtml -OutDir $outDir -AppName $assemblyName -Ver $Version
       Log ("[{0}] index.html updated." -f $app)
     }

     # 매니페스트 파일 확인
     $manifestFile = Join-Path $outDir "$assemblyName.application"
     if (Test-Path $manifestFile) {
       Log ("[{0}] Manifest file updated: {1}" -f $app, $manifestFile)
     } else {
       Warn ("[{0}] Manifest file not found: {1}" -f $app, $manifestFile)
     }

    Log ("[{0}] Publish done." -f $app)
  }

  Log "All apps published successfully."
}
finally {
  if ($didNetUse) {
    Log "Closing SMB session ..."
    cmd /c ('net use "{0}" /delete /y' -f $TargetRoot) | Out-Null
  }
  if ($didSubst -and $substDrive) {
    Log "Removing SUBST drive ..."
    cmd /c "subst $substDrive /d" 2>&1 | Out-Null
  }
}
