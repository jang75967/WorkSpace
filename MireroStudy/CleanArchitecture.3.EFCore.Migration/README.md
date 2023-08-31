# Ű����

1. Dependency Injection
2. ������ ���� ��Ģ(DIP)
3. EF Core
4. Repository Pattern
5. ���� �׽�Ʈ(Container)

* DI�� �����ϸ�, Postgresql -> Oracle -> MsSQL-> MongoDB�� �����Ӱ� ���� ����
* Docker�� ����ؼ� IntegratedTest�� ��� �� �� Database�� �ڵ����� �����ؼ� ����ϵ��� ��

## Database ���� �׽�Ʈ

* 4�������� Postgres�� MongoDB�� ����� MSSQL�� Oracle�� ���� �� ����

#### 1. Postgresql, MsSQL

* ���� ���� �� �ʿ� ���� �׽�Ʈ �ڵ� ���� ����
* PostgresFactory.cs�� Line 24���� BreakPoint�� ��� ���� URL�� Ȯ�� ����(Port ���ε��� �������� �Ǳ� ����)
* GetConnectionString()�� F12�� ������ 
Database: Postgres
Username : postgres
Password : postgres
�� �Ǿ� ����
Port�� 5432������ Container�� ������鼭 ���� Port�� ������ �Ǳ� ������ Ȯ���ؼ� �����ؾ���.


#### 3. ����Ŭ

* ���� Docker Image 4.5GB
* �����ϰ� ������, Migrations �������� migration.bat �����ؾ���.


## ApplicationDbContext �߰� ����

* ���ͳݿ� CleanArchitecture �ҽ� �ڵ峪 ADCEdge �ҽ� �ڵ带 ���� ApplicationDbContext�� Application Layer�� ������.
* ������ EF Core ��ü�� �߻�ȭ �Ǿ� �ִٰ� �Ǵ��ϱ� ������
  * ���� �̾߱��ϸ� EF Core�� Oracle, Postgres, MsSQL �� ��û ���� Database�� �����ϱ� ������ ���� Application Layer���� �߻�ȭ �� �ʿ䰡 ����
* ������ ���⼭�� IRepository�� ����� ApplicationDbContext�� Infrastructure Layer�� ����
* ������ EF Core�� MongoDB�� �������� �ʰ�, EF Core <-> Dapper�� ���� �����ϵ��� ����� ����


## ����(�ϰ� ������ �õ�)
* Dapper�� �߰��Ѵ�.
* Migrations(https://fluentmigrator.github.io/) ����ϸ� �?