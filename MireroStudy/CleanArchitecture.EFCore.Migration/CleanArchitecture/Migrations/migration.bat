
::migration_ver ��Ģ
::migration_ver=1.0.0.0  (major,minor,hitfix,test)
::major  : ������Ʈ ���� ����. (��Ը� �������, ������Ʈ ���� �� ��Ű��ó ����)
::minor  : Entity ���� �� ��Ű�� ���� (ū ��ִ�ó, ū �̽�, ū �䱸���� �߰�)
::hitfix : �Ҽ��� ���� �Ƚ� �� (���� �䱸���� �߰�, �䱸���� ���� ����, �Ҽ��� ���׼���, ���� �̽�)
::test   : �׽�Ʈ ����.
::::::::::::::::::::::::::

:: dotnet efcore ������ ��ġ�ϴ� ��ũ��Ʈ
ECHO Checking for 'dotnet ef' installation...
(dotnet tool list -g | findstr "dotnet-ef") > nul

IF ERRORLEVEL 1 (
    ECHO 'dotnet ef' is not installed.
    ECHO Installing 'dotnet ef'...
    dotnet tool install --global dotnet-ef
    ECHO 'dotnet ef' has been installed.
)

:: dotent Migration ����
:: SET DATABASE=MsSql
:: SET DATABASE=Postgres
SET DATABASE=Oracle
SET VERSION=1.0.0
::dotnet ef migrations add -o Migrations/%DATABASE% Migration_%DATABASE%_%VERSION% --project ../CleanArchitecture.csproj
dotnet ef migrations add Migration_%VERSION% --project ../CleanArchitecture.csproj
echo %VERSION%
pause