#requires -Version 7.0
# deploy.client.prod.ps1
# 운영 ClickOnce 승격 (build once, promote — test(deploy-client)가 PublishTest 에 만든 산출물 재사용, 재-publish 안 함):
#   모든 파일 작업을 170.43 안에서(WinRM) 수행 — CI 러너로의 파일 전송 왕복/임시폴더 없음.
#   앱별로 170.43 로컬에서:
#     PublishTest\<App>\Application Files\<버전폴더> 내용 -> <App>_Patch\<버전폴더명>.zip 압축
#     -> 같은 폴더의 ManifestPatcher.exe 실행 (운영 provider 버전 +1, 압축해제, .application/Index.html 갱신)
#   ManifestPatcher 가 D:\mirero\DMS\Publish(_Foundry)\<App> 로컬 경로에 쓰므로 반드시 170.43 위에서 실행돼야 함.
#   주의: WinRM 원격 세션엔 기본 모듈만 로드됨 -> SmbShare/Archive 같은 모듈 autoload 가 실패함.
#         그래서 경로는 고정경로로, 압축은 .NET ZipFile 로 처리(모듈 의존 제거).
[CmdletBinding()]
param(
  [ValidateSet('Memory','Foundry')] [string]$Flavor = 'Memory',
  [string]$PublishVersion = '',     # 이 파이프라인이 만든 버전(예: 2026.06.15.1543). 지정 시 그 버전폴더만 사용(최신 추적 안 함)
  [Parameter(Mandatory=$true)] [string]$SmbUser,
  [Parameter(Mandatory=$true)] [string]$SmbPassword,
  [string]$ServerHost = '192.168.170.43'
)
$ErrorActionPreference = 'Stop'
function Log($m){ Write-Host "[PROMOTE] $m" -ForegroundColor Cyan }

if ($Flavor -eq 'Memory') {
  $srcShareName = 'PublishTest';         $dstShareName = 'Publish'
  $apps = @('AdminTool','Client','Configurator','LotStatusBoard')
} else {
  $srcShareName = 'PublishTest_Foundry'; $dstShareName = 'Publish_Foundry'
  $apps = @('AutoBBT','Client','Configurator','LotStatusBoard')
}
$cred = [pscredential]::new($SmbUser, (ConvertTo-SecureString $SmbPassword -AsPlainText -Force))

Log "운영 승격 시작 ($Flavor)  src=$srcShareName  dst=$dstShareName  ver=$(if($PublishVersion){$PublishVersion}else{'(미지정->최신)'})"

foreach ($app in $apps) {
  Log "[$app] 170.43 에서 zip + ManifestPatcher 실행..."

  # 모든 작업(버전폴더 선택 / zip / ManifestPatcher)을 170.43 로컬에서 수행.
  # 경로는 D:\mirero\DMS\<공유명> 고정(공유명=폴더명 규칙, ManifestPatcher 도 동일 루트 사용),
  # 압축은 .NET ZipFile 사용 -> WinRM 세션에서 SmbShare/Archive 모듈 autoload 실패를 원천 회피.
  $res = Invoke-Command -ComputerName $ServerHost -Credential $cred -Authentication Basic -ScriptBlock {
    param($srcShareName, $dstShareName, $app, $publishVersion)
    $ErrorActionPreference = 'Stop'
    try {
      $srcBase  = "D:\mirero\DMS\$srcShareName"        # 예: D:\mirero\DMS\PublishTest
      $dstBase  = "D:\mirero\DMS\$dstShareName"        # 예: D:\mirero\DMS\Publish
      $afDir    = Join-Path (Join-Path $srcBase $app) 'Application Files'
      $patchDir = Join-Path $dstBase "${app}_Patch"
      if (-not (Test-Path $afDir))    { throw "PublishTest Application Files 없음: $afDir (test 배포가 먼저 완료됐는지 확인)" }
      if (-not (Test-Path $patchDir)) { throw "운영 패치 폴더 없음: $patchDir" }

      # 이 파이프라인이 만든 버전폴더를 정확히 선택 (publishVersion 지정 시). 미지정 시에만 최신으로 폴백.
      if ($publishVersion) {
        $folderName = "Mirero.DMS.Client.Apps.${app}_$($publishVersion -replace '\.','_')"
        $verDir = Get-Item (Join-Path $afDir $folderName) -ErrorAction SilentlyContinue
        if (-not $verDir) { throw "지정 버전폴더 없음: $folderName (이 파이프라인 산출물이 PublishTest에 있는지 확인)" }
      } else {
        $verDir = Get-ChildItem $afDir -Directory |
                  Where-Object { $_.Name -like "Mirero.DMS.Client.Apps.${app}_*" } |
                  Sort-Object LastWriteTime -Descending | Select-Object -First 1
        if (-not $verDir) { throw "버전 폴더를 찾을 수 없음: $afDir" }
      }

      # 버전폴더 '내용'(flat dll + .dll.manifest + en-US/Windows)을 <App>_Patch\<버전폴더명>.zip 으로 (전부 로컬, 모듈 불필요)
      $zip = Join-Path $patchDir "$($verDir.Name).zip"
      if (Test-Path $zip) { Remove-Item $zip -Force }
      Add-Type -AssemblyName System.IO.Compression.FileSystem
      [System.IO.Compression.ZipFile]::CreateFromDirectory($verDir.FullName, $zip)

      # 같은 폴더의 ManifestPatcher 실행 (y 자동 입력)
      $exe = Join-Path $patchDir 'ManifestPatcher.exe'
      if (-not (Test-Path $exe)) { throw "ManifestPatcher.exe 없음: $exe" }
      $out = ("y" | & $exe $zip 2>&1 | Out-String)
      [pscustomobject]@{ Ok = $true; Version = $verDir.Name; ExitCode = $LASTEXITCODE; Output = $out; Error = $null }
    } catch {
      [pscustomobject]@{ Ok = $false; Version = $null; ExitCode = -1; Output = $null; Error = $_.Exception.Message }
    }
  } -ArgumentList $srcShareName, $dstShareName, $app, $PublishVersion

  if (-not $res.Ok) { throw "[$app] 승격 실패: $($res.Error)" }
  Log "[$app] 소스 버전폴더: $($res.Version)"
  if ($res.Output) { Write-Host $res.Output }
  if ($res.ExitCode -ne 0) { throw "[$app] ManifestPatcher 실패 (exit=$($res.ExitCode))" }
  Log "[$app] 운영 승격 완료"
}
Log "모든 앱 운영 승격 완료 ($Flavor)"
