#requires -Version 7.0
# deploy.server.ps1
# DMS Server Deploy Script (CI deploy stage — 버전별 아티팩트 생성 + staging 까지만)
#   - dotnet publish  -> Setup/Services/dms.<svc>/app  (Dockerfile 의 ADD ./app 대상)
#   - TIBCO Linux 파일 교체 (해당 서비스)
#   - appsetting.*.json -> Setup/config , Linux IPP -> Setup/LinuxIpp
#   - 위 결과를 $ReleaseRoot\$Version 으로 staging
# 원격 배포서버로의 실제 전송은 현재 수동(scp/build_and_push/kubectl)이므로 이 스크립트는 staging 까지만 한다.
# Usage:
#   .\deploy.server.ps1 -SolutionRoot <repoRoot> [-Version <ver>] [-Flavor Memory|Foundry] [-ReleaseRoot D:\DeployServer] [-Mode Release] [-Service a,b] [-NoStage]
#   [원격복사] ... -Copy -RemoteHost <ip> [-RemoteUser mireroadmin] [-SshKey <key> | -RemotePassword <pw>] [-RemotePort 22] [-RemoteBasePath /appdata/dms/volumes/setup] -RemoteServiceDir <docker-service-dmm|docker-service-dmf|docker-service>

[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)] [string]$SolutionRoot,
    [string]$Version,
    [string]$ReleaseRoot = "D:\DeployServer",
    [ValidateSet("Memory", "Foundry")] [string]$Flavor = "Memory",
    [ValidateSet("Debug", "Release")] [string]$Mode = "Release",
    [string[]]$Service = @(),
    [switch]$Archive = $false,
    [switch]$NoStage = $false,

    # ---- 리눅스 서버 복사(copy) 단계 — 모든 접속/경로 정보를 파라미터화 (정보 변경 시 호출부 한 곳만 수정) ----
    [switch]$Copy = $false,
    [string]$RemoteHost,
    [string]$RemoteUser = "mireroadmin",
    [string]$RemotePassword,
    [string]$SshKey,
    [int]$RemotePort = 22,
    [string]$RemoteBasePath = "/appdata/dms/volumes/setup",
    [string]$RemoteServiceDir
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$Colors = @{
    Reset = "`e[0m"; Bold = "`e[1m"; Red = "`e[91m"; Yellow = "`e[93m"; Blue = "`e[94m"; Cyan = "`e[96m"; Green = "`e[92m"
}
function Write-Title { param([string]$Title) Write-Host " "; Write-Host "$($Colors.Blue)$($Colors.Bold)=== $Title ===$($Colors.Reset)" }
function Write-Info  { param([string]$Message) Write-Host "$($Colors.Cyan)$Message$($Colors.Reset)" }
function Write-Success { param([string]$Message) Write-Host "$($Colors.Green)$Message$($Colors.Reset)" }
function Write-Warn  { param([string]$Message) Write-Host "$($Colors.Yellow)$Message$($Colors.Reset)" }
function Write-Fail  { param([string]$Message) Write-Host "$($Colors.Red)$Message$($Colors.Reset)" }

function Remove-DirectoryIfExists {
    param([string]$Path)
    if (Test-Path $Path) { Write-Info "Removing directory: $Path"; Remove-Item -Recurse -Force $Path -ErrorAction Stop }
}

if (-not (Test-Path $SolutionRoot)) { throw "SolutionRoot not found: $SolutionRoot" }
$SolutionRoot = (Resolve-Path $SolutionRoot).Path

if ([string]::IsNullOrWhiteSpace($Version)) {
    $now = Get-Date; $Version = '{0}.{1:D2}.{2:D2}.{3:HHmm}' -f $now.Year, $now.Month, $now.Day, $now
}

$BuildDir   = Join-Path $SolutionRoot "Src"
$SetupDir   = Join-Path $SolutionRoot "Setup"
$ServicesDir = Join-Path $SetupDir "Services"
$Framework  = "net8.0"
$Verbosity  = "minimal"

# Server Services 정의
$ServerServices = @()
$ServerServices += @{ Name="Mirero.DMS.Server.Service.API.AdminTool";       ProjectSubDir="API";  OutputDir="dms.admintool.api";       SpecialHandling=$null;   DisplayName="AdminTool" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.API.DataManager";       ProjectSubDir="API";  OutputDir="dms.datamanager.api";       SpecialHandling=$null;   DisplayName="DataManager" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.AutoDelete";          ProjectSubDir=$null;  OutputDir="dms.autodelete";          SpecialHandling=$null;   DisplayName="AutoDelete" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.Detector";            ProjectSubDir=$null;  OutputDir="dms.detector";            SpecialHandling=$null;   DisplayName="Detector" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.Eds";                 ProjectSubDir=$null;  OutputDir="dms.eds";                 SpecialHandling=$null;   DisplayName="Eds" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.Loader";              ProjectSubDir=$null;  OutputDir="dms.loader";              SpecialHandling="TIBCO"; DisplayName="Loader" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.LogManager";          ProjectSubDir=$null;  OutputDir="dms.logmanager";          SpecialHandling=$null;   DisplayName="LogManager" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.LotTracker";          ProjectSubDir=$null;  OutputDir="dms.lottracker";          SpecialHandling=$null;   DisplayName="LotTracker" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.SendFabExecutor";     ProjectSubDir=$null;  OutputDir="dms.sendfabexecutor";     SpecialHandling="TIBCO"; DisplayName="SendFabExecutor" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.SendFabListener";     ProjectSubDir=$null;  OutputDir="dms.sendfablistener";     SpecialHandling="TIBCO"; DisplayName="SendFabListener" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.TrackOutDistributor"; ProjectSubDir=$null;  OutputDir="dms.trackoutdistributor"; SpecialHandling="TIBCO"; DisplayName="TrackOutDistributor" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.TrackOutExecutor";    ProjectSubDir=$null;  OutputDir="dms.trackoutexecutor";    SpecialHandling="TIBCO"; DisplayName="TrackOutExecutor" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.TransDistributor";    ProjectSubDir=$null;  OutputDir="dms.transdistributor";    SpecialHandling=$null;   DisplayName="TransDistributor" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.Transmitter";         ProjectSubDir=$null;  OutputDir="dms.transmitter";         SpecialHandling=$null;   DisplayName="Transmitter" }
$ServerServices += @{ Name="Mirero.DMS.Server.Service.Trigger";             ProjectSubDir=$null;  OutputDir="dms.trigger";             SpecialHandling=$null;   DisplayName="Trigger" }

function Get-ProjectPath {
    param([hashtable]$ServiceObject)
    $root = Join-Path $BuildDir "Server" "Service"
    if ($ServiceObject.ProjectSubDir) { $root = Join-Path $root $ServiceObject.ProjectSubDir }
    return Join-Path $root $ServiceObject.Name "$($ServiceObject.Name).csproj"
}

function Resolve-TargetServices {
    param([hashtable[]]$AllServices, [string[]]$Filter)
    if (-not $Filter -or $Filter.Count -eq 0) { return $AllServices }
    $filterLower = $Filter | ForEach-Object { $_.ToLower() }
    return $AllServices | Where-Object {
        $dn = $_.DisplayName.ToLower(); $nm = $_.Name.ToLower()
        ($filterLower | Where-Object { $dn -eq $_ -or $nm -eq $_ -or $dn.Contains($_) -or $nm.Contains($_) }).Count -gt 0
    }
}

function Publish-ServerService {
    param([hashtable]$ServiceObject)
    $projectName = $ServiceObject.Name
    $projectPath = Get-ProjectPath -ServiceObject $ServiceObject
    $outputDir   = Join-Path $ServicesDir $ServiceObject.OutputDir "app"

    Write-Title "Publishing $($ServiceObject.DisplayName)"
    if (-not (Test-Path $projectPath)) { throw "Project not found: $projectPath" }

    Remove-DirectoryIfExists $outputDir

    $publishArgs = @(
        "publish", $projectPath,
        "--configuration", $Mode,
        "--framework", $Framework,
        "--output", $outputDir,
        "--nologo",
        "--verbosity", $Verbosity
    )
    Write-Info "Running: dotnet $($publishArgs -join ' ')"
    # build-server 와 동일: 출력은 변수로 캡처(warning 콘솔 노출 억제), 실패 시에만 에러만 출력
    $publishOutput = & dotnet @publishArgs 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Fail "================ dotnet publish FAILED ================"
        Write-Host "Project : $projectName" -ForegroundColor Yellow
        Write-Host "Path    : $projectPath" -ForegroundColor Yellow
        Write-Host "Exit    : $LASTEXITCODE" -ForegroundColor Yellow
        Write-Fail "======================================================"
        Write-Host ""
        Write-Host "---- Error lines ----" -ForegroundColor Cyan
        $publishOutput |
          Select-String -Pattern ":\s*error\s+", "MSB\d{4}", "NETSDK\d{4}", "NU\d{4}", "fatal", "failed" -CaseSensitive:$false |
          Select-Object -First 120 | ForEach-Object { Write-Host $_.Line }
        throw "Publish failed for $projectName (exit=$LASTEXITCODE)"
    }

    if ($ServiceObject.SpecialHandling -eq "TIBCO") {
        Invoke-TIBCOFiles -AppDir $outputDir
    }
    Write-Success "Published $projectName"
}

function Invoke-TIBCOFiles {
    param([string]$AppDir)
    $rendezvousDir = Join-Path $BuildDir "Common" "External" "Rendezvous" "Linux"
    Write-Info "Applying TIBCO Rendezvous Linux files: $AppDir"
    if (-not (Test-Path $AppDir)) { Write-Warn "App directory not found: $AppDir"; return }

    Get-ChildItem -Path $AppDir -Filter "tibrv*.dll" -ErrorAction SilentlyContinue | Remove-Item -Force
    $linuxFiles = @(
        @{ Source = "libtibrv64.so";        Destination = "libtibrv.so" },
        @{ Source = "TIBCO.Rendezvous.dll"; Destination = "TIBCO.Rendezvous.dll" }
    )
    foreach ($file in $linuxFiles) {
        $sourcePath = Join-Path $rendezvousDir $file.Source
        $destPath   = Join-Path $AppDir $file.Destination
        if (Test-Path $sourcePath) { Copy-Item -Path $sourcePath -Destination $destPath -Force; Write-Info "Copied $($file.Source) -> $($file.Destination)" }
        else { Write-Warn "Source file not found: $sourcePath" }
    }
}

function Copy-ConfigFiles {
    Write-Title "Copying Configuration Files"
    $configDir = Join-Path $SetupDir "config"
    $settingsSource = Join-Path $BuildDir "Server" "Service" "Mirero.DMS.Server.Service.Settings"
    Remove-DirectoryIfExists $configDir
    New-Item -ItemType Directory -Path $configDir -Force | Out-Null
    if (Test-Path $settingsSource) {
        Copy-Item -Recurse -Force -Path (Join-Path $settingsSource "*.json") -Destination $configDir
        Write-Success "Configuration files copied"
    } else { Write-Warn "Settings source not found: $settingsSource" }
}

function Copy-LinuxIpp {
    Write-Title "Copying Linux IPP Interop Files"
    $sourceDir = Join-Path $BuildDir "Common\Infra\Mirero.DMS.Common.Infra.Algorithm.IPP.Interop\Linux"
    $destDir   = Join-Path $SetupDir "LinuxIpp"
    Remove-DirectoryIfExists $destDir
    if (Test-Path $sourceDir) { Copy-Item -Recurse -Force -Path $sourceDir -Destination $destDir; Write-Success "Linux IPP files copied" }
    else { Write-Warn "Linux IPP source not found: $sourceDir" }
}

function Publish-Staging {
    param([string]$ReleaseDir)
    Write-Title "Staging release to $ReleaseDir"
    Remove-DirectoryIfExists $ReleaseDir
    New-Item -ItemType Directory -Path $ReleaseDir -Force | Out-Null
    Copy-Item -Recurse -Force -Path $ServicesDir -Destination $ReleaseDir
    Copy-Item -Recurse -Force -Path (Join-Path $SetupDir "config") -Destination $ReleaseDir
    $linuxIpp = Join-Path $SetupDir "LinuxIpp"
    if (Test-Path $linuxIpp) { Copy-Item -Recurse -Force -Path $linuxIpp -Destination $ReleaseDir }
    Write-Success "Release staged: $ReleaseDir"
}

function Copy-ToRemote {
    param([hashtable[]]$Targets, [string]$SourceDir)
    if ([string]::IsNullOrWhiteSpace($RemoteHost))       { throw "-Copy 사용 시 -RemoteHost 필수" }
    if ([string]::IsNullOrWhiteSpace($RemoteServiceDir)) { throw "-Copy 사용 시 -RemoteServiceDir 필수 (예: docker-service-dmm / docker-service-dmf / docker-service)" }

    $remoteDir = "$RemoteBasePath/$RemoteServiceDir"
    $dest = "$RemoteUser@${RemoteHost}:$remoteDir/"
    Write-Title "Copy to remote  ->  $dest  (port $RemotePort)"

    # 전송 방식: SshKey(키, 자동화 권장) > pscp(-pw, PuTTY) > scp(대화형 비번 입력)
    $pscp = Get-Command pscp -ErrorAction SilentlyContinue
    if ($SshKey)                        { $mode = "key" }
    elseif ($RemotePassword -and $pscp) { $mode = "pscp" }
    else                                { $mode = "scp" }
    Write-Info "Transfer mode: $mode  (services: $($Targets.Count) 개, 폴더 전체 덮어쓰기 / 그 외 원격 항목 보존)"
    if ($mode -eq "scp" -and $RemotePassword) {
        Write-Warn "pscp(PuTTY) 미발견 -> OpenSSH scp 사용: 비밀번호를 대화형으로 입력해야 합니다. (CI 자동화엔 -SshKey 권장)"
    }

    # keepalive/타임아웃 — 혼잡 시 idle 연결이 reset 되는 것 방지. 서비스당 재시도(최대 $maxRetry)로 일시적 끊김 흡수.
    $sshOpts = @("-o","StrictHostKeyChecking=accept-new","-o","ServerAliveInterval=15","-o","ServerAliveCountMax=4","-o","ConnectTimeout=30")
    $maxRetry = 3
    $throttle = 4   # 동시 전송 개수 (FileZilla 병렬 전송처럼 — 과하면 reset 우려라 보수적으로 4)

    # 전송 대상: 로컬 폴더(dms.<svc>)가 실제 존재하는 것만
    $names = @()
    foreach ($t in $Targets) {
        $n = $t.OutputDir   # dms.<svc>
        if (Test-Path (Join-Path $SourceDir $n)) { $names += $n } else { Write-Warn "Skip (로컬 폴더 없음): $n" }
    }
    Write-Info "병렬 전송 시작 (동시 $throttle 개, 서비스 $($names.Count) 개)"

    # ForEach-Object -Parallel: 각 runspace 가 동시에 콘솔로 찍으면 로그가 인터리브됨 →
    #   라이브 출력/scp 배너를 2>&1 로 캡처(콘솔 오염 방지)하고, 결과만 객체로 반환.
    #   완료 후 메인에서 서비스별 요약을 한 번에 출력하고, '실패한 서비스만' 캡처 로그를 덤프.
    # Set-Location 으로 staging 폴더 이동 후 '상대경로(dms.<svc>)'로 전송 — Windows 'C:\' 의 콜론이 scp 원격경로로 오인되는 것 방지
    $results = $names | ForEach-Object -ThrottleLimit $throttle -Parallel {
        $ErrorActionPreference = 'Continue'   # 비0 종료가 throw 되지 않게 (수동으로 $LASTEXITCODE 판정)
        $name = $_
        $mode = $using:mode; $sshKey = $using:SshKey; $remotePwd = $using:RemotePassword
        $port = $using:RemotePort; $opts = $using:sshOpts; $dest = $using:dest
        $srcDir = $using:SourceDir; $maxRetry = $using:maxRetry

        Set-Location $srcDir
        $ok = $false; $attempts = 0
        $log = New-Object System.Collections.Generic.List[string]
        for ($attempt = 1; $attempt -le $maxRetry; $attempt++) {
            $attempts = $attempt
            switch ($mode) {
                "key"   { $out = & scp  -i $sshKey -o BatchMode=yes -P $port @opts -r $name $dest 2>&1 }
                "pscp"  { $out = & pscp -P $port -pw $remotePwd -r $name $dest 2>&1 }
                default { $out = & scp  -P $port @opts -r $name $dest 2>&1 }
            }
            $code = $LASTEXITCODE
            if ($out) { $log.Add(("[시도 $attempt] " + (($out | Out-String).Trim()))) }
            if ($code -eq 0) { $ok = $true; break }
            $log.Add("[시도 $attempt] exit=$code")
            if ($attempt -lt $maxRetry) { Start-Sleep -Seconds (5 * $attempt) }
        }
        # 완료 즉시 한 줄 실시간 출력 (병렬이라 끝나는 순서대로 찍힘 — "병렬 전송 시작" 후 로그 안나오는 것 방지)
        if ($ok) { Write-Host ("  [완료] {0} (시도 {1}회)" -f $name, $attempts) -ForegroundColor Green }
        else     { Write-Host ("  [실패] {0} (시도 {1}회, 재시도 소진)" -f $name, $attempts) -ForegroundColor Red }
        [pscustomobject]@{ Name = $name; Ok = $ok; Attempts = $attempts; Log = ($log -join "`n") }
    }

    # 결과 요약 (서비스명 정렬): 성공은 한 줄, 실패만 캡처 로그 덤프
    Write-Info "전송 결과:"
    foreach ($r in ($results | Sort-Object Name)) {
        if ($r.Ok) { Write-Info ("  OK   {0}  (시도 {1}회)" -f $r.Name, $r.Attempts) }
        else       { Write-Warn ("  FAIL {0}  (시도 {1}회)" -f $r.Name, $r.Attempts) }
    }
    $failedItems = @($results | Where-Object { -not $_.Ok })
    if ($failedItems.Count -gt 0) {
        Write-Warn "---- 실패 서비스 상세 로그 ----"
        foreach ($r in $failedItems) {
            Write-Warn ("==== {0} ====" -f $r.Name)
            Write-Host $r.Log
        }
        throw "Copy 실패: $(($failedItems.Name) -join ', ') ($maxRetry회 재시도 후 실패)"
    }
    Write-Success "Copied $($names.Count) service folder(s) -> $dest"
}

# ---- Main ----
Write-Title "DMS Server Deploy Script (staging)"
Write-Info "SolutionRoot : $SolutionRoot"
Write-Info "Version      : $Version"
Write-Info "ReleaseRoot  : $ReleaseRoot"
Write-Info "Flavor       : $Flavor"
Write-Info "Mode         : $Mode"
if ($Service.Count -gt 0) { Write-Info "Target : $($Service -join ', ')" }

if (-not (Get-Command "dotnet" -ErrorAction SilentlyContinue)) { throw ".NET SDK is not installed or not in PATH" }
if (-not (Test-Path $BuildDir)) { throw "Build directory not found: $BuildDir" }

$targets = Resolve-TargetServices -AllServices $ServerServices -Filter $Service
if (-not $targets -or $targets.Count -eq 0) { Write-Warn "No matching services to publish."; exit 0 }

foreach ($t in $targets) { Publish-ServerService -ServiceObject $t }

# 전체 빌드일 때만 config / LinuxIpp 수집
if ($Service.Count -eq 0) {
    Copy-ConfigFiles
    Copy-LinuxIpp
}

# deploy-server 실행 시 항상 버전 폴더로 staging = CI 서버 백업본. 원격 복사도 이 폴더에서 수행.
# (-NoStage 는 로컬에서 app 폴더만 빠르게 만들 때만; CI 는 항상 staging)
$copyFrom = $ServicesDir
if ($NoStage) {
    Write-Title "Deploy Complete (staging skipped: -NoStage)"
    Write-Info "Artifacts published under: $ServicesDir"
} else {
    $releaseDir = Join-Path (Join-Path $ReleaseRoot $Flavor) $Version
    Publish-Staging -ReleaseDir $releaseDir
    $copyFrom = Join-Path $releaseDir "Services"
    Write-Title "Deploy (staging) Complete"
    Write-Info "Artifacts staged at: $releaseDir"
}
if ($Copy) {
    Copy-ToRemote -Targets $targets -SourceDir $copyFrom
    Write-Warn "복사 완료. 서버측 이미지 빌드/푸시 & k8s 배포는 수동입니다 (build_and_push.sh / generate-deployment.sh / start_dms_pod.sh)."
} else {
    Write-Warn "Remote copy 생략(-Copy 미지정). 전송/서버측 배포는 수동입니다 (scp / build_and_push.sh / kubectl)."
}
exit 0
