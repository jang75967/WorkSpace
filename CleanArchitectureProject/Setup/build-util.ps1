# Utils 폴더의 모든 프로젝트 빌드 확인
Write-Host "Building Utils projects..."

# 프로젝트 루트 디렉토리로 이동
$projectRoot = Split-Path -Parent $PSScriptRoot
Set-Location $projectRoot

$utilProjects = @(Get-ChildItem -Path "Utils" -Filter "*.csproj" -Recurse)

if ($utilProjects.Count -eq 0) {
    Write-Host "No util projects found in Utils"
    exit 0
}

Write-Host "Found $($utilProjects.Count) util project(s):"
foreach ($utilProject in $utilProjects) {
    Write-Host "  - $($utilProject.Name)"
}

$builtCount = 0

foreach ($utilProject in $utilProjects) {

    Write-Host "Building: $($utilProject.Name)"
    Write-Host "Project path: $($utilProject.FullName)"
    
    # Restore NuGet packages first (especially important for CI environments)
    Write-Host "Restoring NuGet packages..." -ForegroundColor Cyan
    $restoreOutput = & dotnet restore $utilProject.FullName --verbosity minimal --nologo 2>&1
    $restoreExit = $LASTEXITCODE

    if ($restoreExit -ne 0) {
        Write-Host "Restore failed for $($utilProject.Name)" -ForegroundColor Red
        $restoreOutput | ForEach-Object { Write-Host $_ }
        exit 1
    }
    
    # 빌드 실행 및 오류 캡처
    $buildOutput = & dotnet publish $utilProject.FullName --configuration Release --verbosity minimal --nologo 2>&1
    $exit = $LASTEXITCODE
    
    # 오류만 필터링하여 출력 (경고는 제외)
    $errors = $buildOutput | Where-Object { 
        $_ -match ":\s*error\s+"
    }
    
    if ($errors) {
        Write-Host "Build errors for $($utilProject.Name):" -ForegroundColor Red
        $errors | ForEach-Object { Write-Host $_ -ForegroundColor Red }
    }

    if ($exit -ne 0) {
        Write-Host "Build failed for $($utilProject.Name) with exit code: $exit" -ForegroundColor Red
        Write-Host "Full build output:" -ForegroundColor Yellow
        $buildOutput | ForEach-Object { Write-Host $_ }
        exit 1
    }
    else {
        Write-Host "Build succeeded for $($utilProject.Name)"
        $builtCount++
    }
}

if ($builtCount -eq 0) {
    Write-Host "No util projects were built (all projects were excluded)."
    exit 0
}

Write-Host "All util projects (except excluded) built successfully"
exit 0