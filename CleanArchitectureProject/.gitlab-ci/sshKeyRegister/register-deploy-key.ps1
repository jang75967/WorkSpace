# register-deploy-key.ps1
# 배포용 공개키(.gitlab-ci/sshKeyRegister/deploy-server-key.pub)를 대상 리눅스 서버의
# ~/.ssh/authorized_keys 에 등록하는 헬퍼.
# 서버가 초기화/증설/교체될 때 이 스크립트를 실행해 다시 등록하면 됨.
#
# 사용 예 (로컬 PC, pwsh):
#   .\.gitlab-ci\sshKeyRegister\register-deploy-key.ps1 -RemoteHost 192.168.170.44     # 통합 DEV
#   .\.gitlab-ci\sshKeyRegister\register-deploy-key.ps1 -RemoteHost 192.168.70.189     # IDC Memory
#   .\.gitlab-ci\sshKeyRegister\register-deploy-key.ps1 -RemoteHost 192.168.70.229     # IDC Foundry
#   (ssh 가 비밀번호를 물으면 mireroadmin 계정 비밀번호 입력)
#
# 키 세트(.gitlab-ci/sshKeyRegister/ 에 보관):
#   deploy-server-key      = 개인키 (CI 가 scp 인증에 사용)
#   deploy-server-key.pub  = 공개키 (이 스크립트가 서버에 등록)
#
# 대상 서버(참고):
#   192.168.170.44  : 사내 통합 DEV  (docker-service-dmm / docker-service-dmf)
#   192.168.70.189  : IDC Memory 운영 (docker-service)
#   192.168.70.229  : IDC Foundry 운영 (docker-service)
#
# 동등한 1줄 수동 명령(참고):
#   Get-Content .\.gitlab-ci\sshKeyRegister\deploy-server-key.pub | ssh mireroadmin@<IP> "mkdir -p ~/.ssh && chmod 700 ~/.ssh && cat >> ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys"

param(
    [Parameter(Mandatory=$true)] [string]$RemoteHost,
    [string]$RemoteUser = "mireroadmin",
    [int]$RemotePort = 22,
    [string]$PubKey = (Join-Path $PSScriptRoot "deploy-server-key.pub")
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $PubKey)) {
    throw "공개키를 찾을 수 없습니다: $PubKey  (deploy-server-key.pub 를 .gitlab-ci 에 두었는지 확인)"
}

Write-Host "공개키 등록: $PubKey" -ForegroundColor Cyan
Write-Host "  -> $RemoteUser@${RemoteHost}:~/.ssh/authorized_keys  (port $RemotePort)" -ForegroundColor Cyan
Write-Host "  비밀번호를 물으면 mireroadmin 계정 비밀번호를 입력하세요." -ForegroundColor Yellow

# 첫 접속 호스트키 자동 수락. 이미 등록돼 있으면 추가하지 않음(중복 방지).
$pub = (Get-Content $PubKey -Raw).Trim()
Get-Content $PubKey | ssh -p $RemotePort -o StrictHostKeyChecking=accept-new "$RemoteUser@$RemoteHost" `
    "mkdir -p ~/.ssh && chmod 700 ~/.ssh && touch ~/.ssh/authorized_keys && grep -qxF '$pub' ~/.ssh/authorized_keys && echo '[SKIP] already registered' || (cat >> ~/.ssh/authorized_keys && echo '[OK] registered'); chmod 600 ~/.ssh/authorized_keys"

if ($LASTEXITCODE -ne 0) { throw "등록 실패 (exit=$LASTEXITCODE)" }
Write-Host "완료." -ForegroundColor Green
