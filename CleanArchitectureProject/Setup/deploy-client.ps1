<# 
deploy-client.ps1 — Direct ClickOnce Publish (no stage), no signing
- 바로 네트워크 Target에 Publish (DirectToTarget=ON)
- setup.exe/런타임 폴더는 기본적으로 건드리지 않음(UseBootstrapper=OFF)
- Clean 생략으로 빠르게(NoClean=ON)
#>

[CmdletBinding()]
param(
  [Parameter(Mandatory=$true)] [string]$SolutionRoot,          # C:\Workspace\DMM25
  [Parameter(Mandatory=$true)] [string]$TargetRoot,            # \\192.168.170.43\Publish
  [Parameter(Mandatory=$true)] [string]$ProviderBaseUrl,       # http://192.168.170.43
  [int]$ProviderPort,                                          # 1234 (테스트 시 - PublishTest)
  [string[]]$Apps = @('AdminTool','Client','Configurator'),
  [string]$Version,                                            
  [string]$PublishProfile = 'ClickOnceProfile',
  [ValidateSet('x64','Any CPU','x86')] [string]$Platform = 'x64',
  [switch]$MakeIndexHtml,                                      # 타깃에 index.html 없을 때 생성
  [switch]$UseBootstrapper = $true,                            # setup.exe로 런타임 자동 설치
  [string]$BootstrapperPackagesPath,
  [string]$SmbUser, [string]$SmbPassword,

  # 성능/운영
  [switch]$DirectToTarget = $true,                             # Stage 생략
  [switch]$NoClean = $true                                     # Clean 생략
)

$ErrorActionPreference = 'Stop'
$MSBUILD_FIXED = 'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe'

function Log([string]$m){ Write-Host "[INFO] $m" -ForegroundColor Cyan }
function Warn([string]$m){ Write-Warning $m }
function Fail([string]$m){ Write-Error $m; throw $m }

function Resolve-MSBuild {
  if (Test-Path $MSBUILD_FIXED) { return @{ Type='VS'; Path=$MSBUILD_FIXED } }
  $candidates = @(
    'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe'
  )
  foreach ($p in $candidates) { if (Test-Path $p) { return @{ Type='VS'; Path=$p } } }
  $vswhere = "$Env:ProgramFiles(x86)\Microsoft Visual Studio\Installer\vswhere.exe"
  if (Test-Path $vswhere) {
    $found = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" 2>$null
    if ($LASTEXITCODE -eq 0 -and $found) {
      $msb = ($found | Select-Object -First 1)
      if (Test-Path $msb) { return @{ Type='VS'; Path=$msb } }
    }
  }
  try { $dotnet = (Get-Command dotnet -ErrorAction Stop).Source; if ($dotnet) { return @{ Type='DOTNET'; Path=$dotnet } }} catch {}
  return $null
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
function Invoke-MSBuild([string]$Csproj,[string[]]$MoreArgs,[string]$WorkingDir){
  $info = Resolve-MSBuild
  if (-not $info) { Fail "MSBuild을 찾을 수 없습니다. VS Build Tools 2022 또는 .NET SDK 설치 필요." }
  
  Push-Location $WorkingDir
  try {
    if ($info.Type -eq 'VS') {
      $result = & $info.Path $Csproj @MoreArgs
    } else {
      $result = & $info.Path msbuild $Csproj @MoreArgs
    }
    $exitCode = $LASTEXITCODE
    Log ("MSBuild exit code: {0}" -f $exitCode)
    
    # 오류가 있으면 출력
    if ($exitCode -ne 0) {
      Log ("MSBuild output:")
      $result | ForEach-Object { Log ("  {0}" -f $_) }
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

try {
  $msbuildInfo = Resolve-MSBuild
  Log ("MSBuild = {0} [{1}]" -f $msbuildInfo.Path, $msbuildInfo.Type)

  if ([string]::IsNullOrWhiteSpace($Version)) {
    $now = Get-Date; $Version = '{0}.{1}.{2}.{3}' -f $now.Year,$now.Month,$now.Day,$now.Hour
  }
  if (-not (Test-Path $SolutionRoot)) { Fail ("SolutionRoot not found: {0}" -f $SolutionRoot) }
  if (-not (Test-Path $TargetRoot)) {
    if ($TargetRoot -like "\\*") { Fail ("TargetRoot not found or not accessible: {0}" -f $TargetRoot) }
    else { New-Item -ItemType Directory -Force -Path $TargetRoot | Out-Null }
  }

  # 프로젝트/어셈블리 매핑
  $ProjectMap = @{
    'AdminTool'    = Join-Path $SolutionRoot 'Src\Client\Apps\Client.Apps.AdminTool\Client.Apps.AdminTool.csproj'
    'Client'       = Join-Path $SolutionRoot 'Src\Client\Apps\Client.Apps.Client\Client.Apps.Client.csproj'
    'Configurator' = Join-Path $SolutionRoot 'Src\Client\Apps\Client.Apps.Configurator\Client.Apps.Configurator.csproj'
  }
  $AssemblyMap = @{
    'AdminTool'    = 'Client.Apps.AdminTool'
    'Client'       = 'Client.Apps.Client'
    'Configurator' = 'Client.Apps.Configurator'
  }

  foreach ($app in $Apps) {
    if (-not $ProjectMap.ContainsKey($app)) { Fail ("Project path not mapped: {0}" -f $app) }
    $csproj = $ProjectMap[$app]
    if (-not (Test-Path $csproj)) { Fail ("Project not found: {0}" -f $csproj) }

     $assemblyName = $AssemblyMap[$app]
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
     $tempPublishDir = Join-Path $env:TEMP "ClickOncePublish_$app"
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
     $moreArgs = @(
       $targets,
       "/p:PublishProfile=$PublishProfile",
       "/p:Configuration=Release",
       "/p:Platform=$Platform",
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

    # obj 폴더 정리 후 dotnet restore
    $objDir = Join-Path $projectDir "obj"
    if (Test-Path $objDir) {
      Log ("[{0}] Cleaning obj directory..." -f $app)
      Remove-Item -Recurse -Force $objDir
    }
    
    Log ("[{0}] Running dotnet restore..." -f $app)
    $restoreResult = & dotnet restore $csproj -r win-x64
    if ($LASTEXITCODE -ne 0) { 
      Log ("[{0}] dotnet restore output:" -f $app)
      $restoreResult | ForEach-Object { Log ("  {0}" -f $_) }
      Fail ("[{0}] dotnet restore failed (exit={1})." -f $app, $LASTEXITCODE) 
    }

    # 로컬 임시 폴더에 ClickOnce 게시
    $exit = Invoke-MSBuild -Csproj $csproj -MoreArgs $moreArgs -WorkingDir $projectDir
    if ($exit -ne 0) { Fail ("[{0}] MSBuild publish failed (exit={1})." -f $app,$exit) }

    Log ("[{0}] ClickOnce published to temp directory" -f $app)

    # ClickOnce 결과에서 필요한 파일들만 복사
    Log ("[{0}] Copying ClickOnce files to target..." -f $app)
    
    # 1. Application Files\AppName_Version\ 폴더의 내용을 버전 폴더로 복사
    $tempAppFiles = Join-Path $tempPublishDir "Application Files"
    if (Test-Path $tempAppFiles) {
      $tempVersionDir = Get-ChildItem -Path $tempAppFiles -Directory | Where-Object { $_.Name -like "${assemblyName}_*" } | Select-Object -First 1
      if ($tempVersionDir) {
        Log ("[{0}] Copying version files from: {1}" -f $app, $tempVersionDir.FullName)
        
        # 폴더와 파일을 분리해서 복사
        $items = Get-ChildItem -Path $tempVersionDir.FullName
        foreach ($item in $items) {
          if ($item.PSIsContainer) {
            # 폴더 복사
            $destFolder = Join-Path $versionDir $item.Name
            Copy-Item -Path $item.FullName -Destination $destFolder -Recurse -Force
            Log ("[{0}] Copied folder: {1}" -f $app, $item.Name)
          } else {
            # 파일 복사
            Copy-Item -Path $item.FullName -Destination $versionDir -Force
            Log ("[{0}] Copied file: {1}" -f $app, $item.Name)
          }
        }
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
}
