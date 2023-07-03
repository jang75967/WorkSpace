# 키워드

* DI만 조정하며, Postgresql -> Oracle -> MsSQL-> MongoDB를 자유롭게 변경 가능
* Docker를 사용해서 IntegratedTest를 사용 할 때 Database를 자동으로 구축해서 사용하도록 함

* EF Core, DI, 의존성 역전 원칙(DIP)
* IntegratedTest(Docker 사용), Migrations(Test data Database에 초기화하기)

## Database 변경 테스트

* 4차에서는 Postgres와 MongoDB만 남기고 MSSQL과 Oracle은 제거 할 예정

#### 1. Postgresql, MsSQL

* 따로 설정 할 필요 없이 테스트 코드 실행 가능

#### 2. MongoDB 테스트
* PersistenceExtension -> services.AddMongoDB(configuration);
* RepositoryExtension -> services.AddScoped<IUserRepository, Infrastructure.MongoDB.Repositories.UserRepository>();

* 왜 인지 모르겠지만, ConfigureWebHost이 정상적으로 먹지않음

#### 3. 오라클

* 비추 Docker Image 4.5GB
* 실행하고 싶으면, Migrations 폴더에서 migration.bat 실행해야함.


## ApplicationDbContext 추가 설명

* 인터넷에 CleanArchitecture 소스 코드나 ADCEdge 소스 코드를 보면 ApplicationDbContext가 Application Layer에 존재함.
* 이유는 EF Core 자체가 추상화 되어 있다고 판단하기 때문에
  * 쉽게 이야기하면 EF Core가 Oracle, Postgres, MsSQL 등 엄청 많은 Database를 지원하기 때문에 따로 Application Layer에서 추상화 할 필요가 없다
* 하지만 여기서는 IRepository를 만들고 ApplicationDbContext가 Infrastructure Layer에 존재
* 이유는 EF Core가 MongoDB를 지원하지 않고, EF Core <-> Dapper를 변경 가능하도록 만들기 위함

## 과제(하고 싶으면 시도 ㄱㄱ)
* Dapper를 추가한다.
* Migrations(https://fluentmigrator.github.io/) 사용하면 됌
