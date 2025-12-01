# UnitTests 폴더의 모든 테스트 프로젝트 실행
Write-Host "Building and Running Unit Tests..."

# 프로젝트 루트 디렉토리로 이동
$projectRoot = Split-Path -Parent $PSScriptRoot
Set-Location $projectRoot

$testProjects = Get-ChildItem -Path "Tests\UnitTests" -Filter "*.csproj" -Recurse

if ($testProjects.Count -eq 0) {
    Write-Host "No unit test projects found in Tests\UnitTests"
    exit 0
}

Write-Host "Found $($testProjects.Count) unit test project(s):"
foreach ($testProject in $testProjects) {
    Write-Host "  - $($testProject.Name)"
}

foreach ($testProject in $testProjects) {
    Write-Host "Testing: $($testProject.Name)"
    dotnet test $testProject.FullName --configuration Release --verbosity quiet --logger "console;verbosity=minimal" --nologo 2>&1 | Where-Object { 
        $_ -notlike "*warning CS*" -and 
        $_ -notlike "*.cs(*): warning*"
    }
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Unit tests failed for $($testProject.Name)"
        exit 1
    } else {
        Write-Host "Unit tests passed for $($testProject.Name)"
    }
}

Write-Host "All unit tests passed successfully"