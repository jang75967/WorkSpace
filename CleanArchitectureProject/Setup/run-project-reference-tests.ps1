# ProjectReference 아키텍처 테스트 실행
Write-Host "Building and Running Project Reference Architecture Tests..."

# 프로젝트 루트 디렉토리로 이동
$projectRoot = Split-Path -Parent $PSScriptRoot
Set-Location $projectRoot

$testProject = Get-ChildItem -Path "Tests\AppTests\Architecture.Tests" -Filter "*.csproj"

if (-not $testProject) {
    Write-Host "Project reference test project not found in Tests\AppTests\Architecture.Tests"
    exit 1
}

Write-Host "Found project reference test project: $($testProject.Name)"

Write-Host "Testing: $($testProject.Name)"
dotnet test $testProject.FullName --configuration Release --verbosity quiet --logger "console;verbosity=minimal" --nologo 2>&1 | Where-Object { 
    $_ -notlike "*warning CS*" -and 
    $_ -notlike "*.cs(*): warning*"
}
if ($LASTEXITCODE -ne 0) {
    Write-Host "Project reference architecture tests failed for $($testProject.Name)"
    exit 1
} else {
    Write-Host "Project reference architecture tests passed for $($testProject.Name)"
}

Write-Host "All project reference architecture tests passed successfully"
