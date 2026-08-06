#requires -Version 7.0
# deploy.server.prod.ps1
# [승격] deploy.server.ps1 이 만든 스테이징(D:\DeployServer\<Flavor>\<Version>\Services)의
#        서비스 폴더(dms.*)를 원격 리눅스 서버로 그대로 복사한다. publish/빌드는 하지 않음(재빌드 없는 승격).
#        (deploy.client.prod.ps1 과 같은 역할 — test 가 만든 산출물을 운영으로 승격만)
# Usage:
#   .\deploy.server.prod.ps1 -Version <ver> -Flavor Memory|Foundry `
#       -RemoteHost <ip> -RemoteServiceDir <docker-service|...> `
#       [-SshKey <key> | -RemotePassword <pw>] [-RemoteUser mireroadmin] [-RemotePort 22] `
#       [-RemoteBasePath /appdata/dms/volumes/setup] [-ReleaseRoot D:\DeployServer]

[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)] [string]$Version,
    [ValidateSet("Memory", "Foundry")] [string]$Flavor = "Memory",
    [string]$ReleaseRoot = "D:\DeployServer",

    [Parameter(Mandatory=$true)] [string]$RemoteHost,
    [Parameter(Mandatory=$true)] [string]$RemoteServiceDir,
    [string]$RemoteUser = "mireroadmin",
    [string]$SshKey,
    [string]$RemotePassword,
    [int]$RemotePort = 22,
    [string]$RemoteBasePath = "/appdata/dms/volumes/setup"
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

if ([string]::IsNullOrWhiteSpace($Version)) { throw "-Version 필수 (deploy-server 가 만든 스테이징 버전)" }

$copyFrom  = Join-Path (Join-Path (Join-Path $ReleaseRoot $Flavor) $Version) "Services"
$remoteDir = "$RemoteBasePath/$RemoteServiceDir"
$dest      = "$RemoteUser@${RemoteHost}:$remoteDir/"

Write-Title "DMS Server Promote (copy-only)"
Write-Info "Version : $Version"
Write-Info "Flavor  : $Flavor"
Write-Info "Staging : $copyFrom"
Write-Info "Target  : $dest  (port $RemotePort)"
if (-not (Test-Path $copyFrom)) { throw "스테이징 없음: $copyFrom (deploy-server 가 이 버전($Version)을 만들었는지 확인)" }

# 전송 방식: SshKey(키, 자동화 권장) > pscp(-pw, PuTTY) > scp(대화형 비번 입력)
$pscp = Get-Command pscp -ErrorAction SilentlyContinue
if ($SshKey)                        { $mode = "key" }
elseif ($RemotePassword -and $pscp) { $mode = "pscp" }
else                                { $mode = "scp" }
Write-Info "Transfer mode: $mode"
if ($mode -eq "scp" -and $RemotePassword) {
    Write-Warn "pscp(PuTTY) 미발견 -> OpenSSH scp 사용: 비밀번호를 대화형으로 입력해야 합니다. (CI 자동화엔 -SshKey 권장)"
}

# keepalive/타임아웃 — 혼잡 시 idle 연결이 reset 되는 것 방지. 서비스당 재시도(최대 $maxRetry)로 일시적 끊김 흡수.
$sshOpts  = @("-o","StrictHostKeyChecking=accept-new","-o","ServerAliveInterval=15","-o","ServerAliveCountMax=4","-o","ConnectTimeout=30")
$maxRetry = 3
$throttle = 4   # 동시 전송 개수 (과하면 reset 우려라 보수적으로 4)

# 전송 대상: 스테이징 Services 하위 서비스 폴더(dms.*) 전체
$names = @(Get-ChildItem $copyFrom -Directory | ForEach-Object { $_.Name })
if ($names.Count -eq 0) { throw "전송할 서비스 폴더가 없음: $copyFrom" }
Write-Info "병렬 전송 시작 (동시 $throttle 개, 서비스 $($names.Count) 개)"

# ForEach-Object -Parallel: 라이브 출력/scp 배너를 2>&1 로 캡처(콘솔 오염 방지)하고 결과만 객체로 반환.
#   완료 후 메인에서 서비스별 요약을 한 번에 출력하고, '실패한 서비스만' 캡처 로그를 덤프.
#   Set-Location 으로 staging 폴더 이동 후 '상대경로(dms.<svc>)'로 전송 — Windows 'C:\' 콜론이 scp 원격경로로 오인되는 것 방지.
$results = $names | ForEach-Object -ThrottleLimit $throttle -Parallel {
    $ErrorActionPreference = 'Continue'   # 비0 종료가 throw 되지 않게 (수동으로 $LASTEXITCODE 판정)
    $name = $_
    $mode = $using:mode; $sshKey = $using:SshKey; $remotePwd = $using:RemotePassword
    $port = $using:RemotePort; $opts = $using:sshOpts; $dest = $using:dest
    $srcDir = $using:copyFrom; $maxRetry = $using:maxRetry

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
Write-Warn "복사 완료. 서버측 이미지 빌드/푸시 & k8s 배포는 수동입니다 (build_and_push.sh / generate-deployment.sh / start_dms_pod.sh)."
exit 0
