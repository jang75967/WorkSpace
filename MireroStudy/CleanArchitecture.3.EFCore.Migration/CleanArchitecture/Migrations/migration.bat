
::migration_ver 규칙
::migration_ver=1.0.0.0  (major,minor,hitfix,test)
::major  : 프로젝트 갈아 엎음. (대규모 변경사항, 프로젝트 구조 및 아키텍처 변경)
::minor  : Entity 변경 및 스키마 변경 (큰 장애대처, 큰 이슈, 큰 요구사항 추가)
::hitfix : 소소한 버그 픽스 및 (작은 요구사항 추가, 요구사항 내역 변경, 소소한 버그수정, 작은 이슈)
::test   : 테스트 버전.
::::::::::::::::::::::::::

:: dotnet efcore 없으면 설치하는 스크립트
ECHO Checking for 'dotnet ef' installation...
(dotnet tool list -g | findstr "dotnet-ef") > nul

IF ERRORLEVEL 1 (
    ECHO 'dotnet ef' is not installed.
    ECHO Installing 'dotnet ef'...
    dotnet tool install --global dotnet-ef
    ECHO 'dotnet ef' has been installed.
)

:: dotent Migration 시작
:: SET DATABASE=MsSql
:: SET DATABASE=Postgres
SET DATABASE=Oracle
SET VERSION=1.0.0
::dotnet ef migrations add -o Migrations/%DATABASE% Migration_%DATABASE%_%VERSION% --project ../CleanArchitecture.csproj
dotnet ef migrations add Migration_%VERSION% --project ../CleanArchitecture.csproj
echo %VERSION%
pause