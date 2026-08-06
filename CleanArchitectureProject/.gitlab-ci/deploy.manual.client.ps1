[CmdletBinding()]
param(
  [Parameter(Mandatory=$true)] [string]$SolutionRoot,
  [Parameter(Mandatory=$true)] [string]$TargetRoot,
  [string]$Flavor = "Memory",
  [string[]]$Manuals = @(),                                    # 기본값은 Flavor 기반으로 런타임에서 할당
  [string]$SmbUser, [string]$SmbPassword
)

$ErrorActionPreference = 'Stop'

# Flavor 검증 및 Manuals 기본값 설정
if ([string]::IsNullOrWhiteSpace($Flavor)) { $Flavor = "Memory" }
if ($Flavor -ne "Memory" -and $Flavor -ne "Foundry") { throw "Invalid Flavor: $Flavor (expected Memory|Foundry)" }

if (-not $Manuals -or $Manuals.Count -eq 0) {
  $Manuals = if ($Flavor -eq "Memory") {
    @('client','configurator','lotstatusboard')
  } else {
    @('autobbt','client','configurator','lotstatusboard')
  }
}

function Log([string]$m){ Write-Host "[INFO] $m" -ForegroundColor Cyan }
function Warn([string]$m){ Write-Warning $m }
function Fail([string]$m){ Write-Error $m; throw $m }

# --------------------------- 의존성 확인 ---------------------------

Log "Checking python availability..."
$pyVer = & python --version 2>&1
if ($LASTEXITCODE -ne 0) {
  Fail "python not found in PATH. Install Python 3.10+ and ensure it is on PATH on the CI runner."
}
Log ("python = {0}" -f ($pyVer -join ' '))

Log "Checking mkdocs dependencies (mkdocs-material, mkdocs-static-i18n)..."
$depOut = & python -c "import material; import mkdocs_static_i18n" 2>&1
if ($LASTEXITCODE -ne 0) {
  Fail ("mkdocs dependencies missing on runner.`n" +
        "Required packages: mkdocs-material, mkdocs-static-i18n`n" +
        "Install with: pip install mkdocs-material mkdocs-static-i18n`n" +
        "python error: " + ($depOut -join "`n"))
}
Log "mkdocs dependencies OK."

# --------------------------- 매뉴얼 빌드 ---------------------------

if (-not (Test-Path $SolutionRoot)) { Fail ("SolutionRoot not found: {0}" -f $SolutionRoot) }
$manualRoot = Join-Path $SolutionRoot "Manual\Client"
if (-not (Test-Path $manualRoot)) { Fail ("Manual root not found: {0}" -f $manualRoot) }

Push-Location $manualRoot
try {
  # Python stdout/stderr 을 UTF-8 로 강제 (Windows cp949 로캘에서 유니코드 출력 실패 방지)
  # build.bat 의 set PYTHONUTF8=1 / PYTHONIOENCODING=utf-8 와 동일 효과
  $env:PYTHONUTF8 = "1"
  $env:PYTHONIOENCODING = "utf-8"

  if ($Flavor -eq "Memory") {
    Log ("[BUILD] python build.py {0} (cwd={1})" -f ($Manuals -join ' '), $manualRoot)
    & python build.py @Manuals
  } else {
    Log ("[BUILD] python build.py --all (cwd={0})" -f $manualRoot)
    & python build.py --all
  }
  if ($LASTEXITCODE -ne 0) { Fail ("Manual build failed (python build.py exit={0})" -f $LASTEXITCODE) }
  Log "[BUILD] All target manuals built successfully."
} finally {
  Pop-Location
}

# --------------------------- 배포 ---------------------------

# SMB 연결(옵션)
$didNetUse = $false
if ($SmbUser -and $SmbPassword) {
  Log ("Opening SMB session to {0} ..." -f $TargetRoot)
  cmd /c ('net use "{0}" /user:{1} "{2}" /persistent:no' -f $TargetRoot,$SmbUser,$SmbPassword) | Out-Null
  $didNetUse = $true
}

try {
  if (-not (Test-Path $TargetRoot)) {
    if ($TargetRoot -like "\\*") { Fail ("TargetRoot not found or not accessible: {0}" -f $TargetRoot) }
    else { New-Item -ItemType Directory -Force -Path $TargetRoot | Out-Null }
  }

  $manualTargetRoot = Join-Path $TargetRoot "Manual"
  if (-not (Test-Path $manualTargetRoot)) {
    Log ("Creating manual target root: {0}" -f $manualTargetRoot)
    New-Item -ItemType Directory -Force -Path $manualTargetRoot | Out-Null
  }

  $validManuals = @('autobbt','client','configurator','lotstatusboard')

  foreach ($manual in $Manuals) {
    if ($validManuals -notcontains $manual) {
      Fail ("Unknown manual: '{0}'. Expected one of: {1}" -f $manual, ($validManuals -join ', '))
    }

    $siteDir = Join-Path $manualRoot ("manuals\{0}\site" -f $manual)
    $appTargetDir = Join-Path $manualTargetRoot $manual

    if (-not (Test-Path $siteDir)) {
      Fail ("[{0}] site directory not found after build: {1}" -f $manual, $siteDir)
    }

    Log ("[{0}] Deploying: {1} -> {2}" -f $manual, $siteDir, $appTargetDir)

    if (-not (Test-Path $appTargetDir)) {
      New-Item -ItemType Directory -Force -Path $appTargetDir | Out-Null
    }

    # robocopy: SMB 안정성 + 미러링 (오래된 파일 제거)
    # /MIR  : 미러(/E + /PURGE) — 대상 폴더가 소스와 동일하게 유지
    # /R:3  : 실패 시 3회 재시도
    # /W:5  : 재시도 간 5초 대기
    # /NFL /NDL /NP : 로그 압축
    $robocopyLog = Join-Path $env:TEMP ("robocopy_manual_{0}_{1}.log" -f $Flavor, $manual)
    $rcArgs = @($siteDir, $appTargetDir, '/MIR', '/R:3', '/W:5', '/NFL', '/NDL', '/NP', ('/LOG:' + $robocopyLog))
    & robocopy @rcArgs | Out-Null
    $rcExit = $LASTEXITCODE

    # robocopy 종료 코드: 0-7 성공 (각 비트가 상태 표시), 8 이상은 실패
    if ($rcExit -ge 8) {
      $logTail = ""
      if (Test-Path $robocopyLog) {
        $logTail = (Get-Content -Path $robocopyLog -Tail 60 -ErrorAction SilentlyContinue) -join "`n"
      }
      Fail ("[{0}] robocopy failed (exit={1}). Log tail:`n{2}" -f $manual, $rcExit, $logTail)
    }

    Log ("[{0}] Deployment success (robocopy exit={1})." -f $manual, $rcExit)
  }

  Log "All manuals deployed successfully."
}
finally {
  if ($didNetUse) {
    Log "Closing SMB session ..."
    cmd /c ('net use "{0}" /delete /y' -f $TargetRoot) | Out-Null
  }
}