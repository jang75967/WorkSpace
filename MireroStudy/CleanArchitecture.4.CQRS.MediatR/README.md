## 키워드

* CQRS
* AOP => Cross-Cutting
  * 
* MediatR
  * MediatR IPipelineBehavior => Chain of Responsibility Pattern (유사패턴 => Decorator Pattern, Proxy Pattern)
* MockQueryable.Moq
* ITestCaseOrderer

## CQRS

CQRS는 Command Query Responsibility Segregation으로 명령(Create, Delete, Update)와 조회(Select) 책임을 분리하는 설계 패턴입니다.
CQRS를 사용하면 복잡한 비즈니스를 독립적으로 만들어 한 곳에서 집중해서 관리가 가능하게 해줍니다.
또한 Command와 Query를 분리하게 되면 추후 서비스와 데이터베이스 자체를 분리하여 사용하기도 편합니다.

#### MediatR
C#에서 CQRS를 편하게 만들어주는 대표적인 라이브러리는 [MediatR](https://github.com/jbogard/MediatR)이 있습니다.

#### MediatR IPipelineBehavior

MediatR에서는 PipeLineBehavior을 통해 Handler 전, 후에 어떠한 작업을 할 수 있습니다.
이는 [Chain of Responsibility Pattern을 사용](https://arturkrajewski.silvrback.com/chain-of-responsibility-pattern-for-handling-cross-cutting-concerns)해서 Cross-Cutting 문제를 해결 할 수 있습니다.

* [참고](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api#use-the-mediator-pattern-in-memory-in-the-command-pipeline)

## CQRS 테스트
## 단위 테스트

Database를 실행 시켜 테스트하는 통합테스트는 속도가 느리고, Seed 데이터가 고정적입니다.
개발자가 자신이 만든 테스트 데이터로 마음대로 만들어 테스트하기에는 단위 테스트를 사용하는 것이 좋습니다.
dbContext를 Mocking하기 위해 [MockQueryable](https://github.com/romantitov/MockQueryable)를 사용하면 편합니다.

하지만 CRUD를 테스트하기 위해서는 따로 Setup을 진행해줘야 합니다.
MockDbContext를 참고하세요.

#### 통합 테스트

통합 테스트는 Database를 실행 시켜 테스트를 하게 됩니다.
하지만 문제가 있는데, xUnit은 순서 없이 실행 되기 때문에 CRUD 시 Seed 데이터가 변경되기 때문에 테스트 하기 까다롭습니다.

이때 xUnit에서 제공하는 [ITestCaseOrderer](https://learn.microsoft.com/ko-kr/dotnet/core/testing/order-unit-tests?pivots=xunit)를 사용하면 순서를 정해 편하게 테스트할 수 있습니다.

