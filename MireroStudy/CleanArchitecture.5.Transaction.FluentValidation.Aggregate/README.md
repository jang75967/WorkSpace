## 키워드

* Transaction
    * MediatR IPipelineBehavior 사용
* FluentValidation
* Aggregate

## Transection

CQRS로 분리한 Command는 Command -> Command로 여러번 단계를 거쳐 실행 할 수 있습니다.
이 부분을 Trasaction을 통해 관리 할 수 있어야 하는데, MediatR의 IPipelineBehavior을 통해 구현하면 편리하게 구현 할 수 있습니다.

#### TransactionBehavior

TransactionBehavior는 CommandHandler 전 후로 동작할 수 있습니다.

처음 Handler가 호출 되기 전 BeginTransactionAsync를 통해 Transaction을 생성하고 Handler가 전부 호출이 끝나면 CommitTransactionAsync를 통해 Commit하면 됩니다.

## FluentValidation

현재 Users 부분만 Validator를 만들었으며, FluentValidation Nuget 패키지를 활용하면 쉽고 직관적으로 작성이 가능합니다.

Handler 전 Validator를 통해 검증이 끝난다면, Handler에 데이터 검증하는 로직을 뺄 수 있어 Handler의 로직이 간단해지는 이점이 있습니다.


#### ValidationBehavior

Validation도 MediatR의 IPipelineBehavior을 통해 구현하면 Handler 호출 전 자동으로 호출 되게 만들 수 있습니다.


## Aggregate

Aggregate는 DDD에서 나오는 이야기로 Entity들을 모아 하나의 단위로 취급하는 개념입니다.
[DDD 애그리거트란?](https://devlos.tistory.com/51)에 나오는 것과 같이 주문으로 이야기를 많이하는데, 저는 Group 활동으로 예제를 만들었습니다.

Group Entity를 회사 동호회라고 생각하면 Activity는 동호회 활동입니다.
동호회를 활동 할 때는 비용(Expense)이 발생하고 참가인원(Attendant)이 있어야 합니다.

여기서 비용과 참가인원은 동호회 활동이라는 곳에 묶여서 동작하게 됩니다.

만약 비용과 참가인원을 따로 CRUD를 하게 된다면 Activity와의 일관성이 깨지는 일이 발생합니다.

예를 들어 활동에 대한 총 비용(TotalPayment)을 따로 계산을 해놓은게 있는데, 비용을 따로 Update하게 되면 총 비용이 맞지 않는 경우가 생길 수도 있습니다.

그래서 Expense와 Attendant는 따로 Repository를 두지 않고 Activity로만 CRUD를 같이하게 됩니다.

