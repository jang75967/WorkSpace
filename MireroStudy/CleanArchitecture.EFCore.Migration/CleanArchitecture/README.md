# Ű����

* DI�� �����ϸ�, Postgresql -> Oracle -> MsSQL-> MongoDB�� �����Ӱ� ���� ����
* Docker�� ����ؼ� IntegratedTest�� ��� �� �� Database�� �ڵ����� �����ؼ� ����ϵ��� ��

* EF Core, DI, ������ ���� ��Ģ(DIP)
* IntegratedTest(Docker ���), Migrations(Test data Database�� �ʱ�ȭ�ϱ�)

## Database ���� �׽�Ʈ

* 4�������� Postgres�� MongoDB�� ����� MSSQL�� Oracle�� ���� �� ����

#### 1. Postgresql, MsSQL

* ���� ���� �� �ʿ� ���� �׽�Ʈ �ڵ� ���� ����

#### 2. MongoDB �׽�Ʈ
* PersistenceExtension -> services.AddMongoDB(configuration);
* RepositoryExtension -> services.AddScoped<IUserRepository, Infrastructure.MongoDB.Repositories.UserRepository>();

* �� ���� �𸣰�����, ConfigureWebHost�� ���������� ��������

#### 3. ����Ŭ

* ���� Docker Image 4.5GB
* �����ϰ� ������, Migrations �������� migration.bat �����ؾ���.


## ApplicationDbContext �߰� ����

* ���ͳݿ� CleanArchitecture �ҽ� �ڵ峪 ADCEdge �ҽ� �ڵ带 ���� ApplicationDbContext�� Application Layer�� ������.
* ������ EF Core ��ü�� �߻�ȭ �Ǿ� �ִٰ� �Ǵ��ϱ� ������
  * ���� �̾߱��ϸ� EF Core�� Oracle, Postgres, MsSQL �� ��û ���� Database�� �����ϱ� ������ ���� Application Layer���� �߻�ȭ �� �ʿ䰡 ����
* ������ ���⼭�� IRepository�� ����� ApplicationDbContext�� Infrastructure Layer�� ����
* ������ EF Core�� MongoDB�� �������� �ʰ�, EF Core <-> Dapper�� ���� �����ϵ��� ����� ����

## ����(�ϰ� ������ �õ� ����)
* Dapper�� �߰��Ѵ�.
* Migrations(https://fluentmigrator.github.io/) ����ϸ� ��
