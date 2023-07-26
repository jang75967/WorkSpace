
ECHO Checking for 'dotnet ef' installation...
(dotnet tool list -g | findstr "dotnet-ef") > nul

IF ERRORLEVEL 1 (
    ECHO 'dotnet ef' is not installed.
    ECHO Installing 'dotnet ef'...
    dotnet tool install --global dotnet-ef
    ECHO 'dotnet ef' has been installed.
)

SET DATABASE=PostgreSql
SET VERSION=1.0.0
dotnet ef migrations add Migration_%VERSION% --project ../CleanArchitecture.csproj
echo %VERSION%
pause