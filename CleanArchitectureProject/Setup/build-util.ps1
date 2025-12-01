# Utils 폴더의 모든 프로젝트 빌드 확인
Write-Host "Building Utils projects..."

# 프로젝트 루트 디렉토리로 이동
$projectRoot = Split-Path -Parent $PSScriptRoot
Set-Location $projectRoot

$utilProjects = Get-ChildItem -Path "Utils" -Filter "*.csproj" -Recurse

if ($utilProjects.Count -eq 0) {
    Write-Host "No util projects found in Utils"
    exit 0
}

Write-Host "Found $($utilProjects.Count) util project(s):"
foreach ($utilProject in $utilProjects) {
    Write-Host "  - $($utilProject.Name)"
}

# 제외할 프로젝트 이름 (DevExpress 참조 문제로 임시로 제외)
$excludeProject = "Utils.SharpXmlJsonDBMigrator.csproj"

foreach ($utilProject in $utilProjects) {

    if ($utilProject.Name -eq $excludeProject) {
        Write-Host "Skipping excluded project: $($utilProject.Name)"
        continue
    }

    Write-Host "Building: $($utilProject.Name)"
    dotnet publish $utilProject.FullName --configuration Release --verbosity quiet --nologo 2>&1 |
        Where-Object { 
            $_ -notlike "*warning CS*" -and 
            $_ -notlike "*.cs(*): warning*"
        }

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Build failed for $($utilProject.Name)"
        exit 1
    } 
    else {
        Write-Host "Build succeeded for $($utilProject.Name)"
    }
}

Write-Host "All util projects (except excluded) built successfully"
