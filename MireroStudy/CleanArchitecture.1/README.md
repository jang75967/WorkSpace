## 키워드
1. gRPC
2. Dependency Injection
3. Unit Test

## 패키지
1. xUnit
2. Fluent Assertion


#### API Service

Client나 다른 Service가 Database에 직접 접근하여 데이터를 핸들링하는 것은 대단히 위험합니다.
지금 대다수의 프로젝트에서 Client에 Database 접속 정보를 입력해야 사용 할 수 있습니다.
하지만 API Service(Backend)를 사용하면, Client나 다른 Service는 Database 접속 정보를 몰라도 되며, 요청에 대해 API Service가 검증 할 수 있습니다.
그렇게 되면 Database의 부하가 적어지게 됩니다.

RESTful API를 많이 사용하지만 불특정 다수에게 API를 제공하지 않기 때문에 성능 측면에서 유리한 gRPC를 선택했습니다.

#### Unit Test
현재 사내에서 Client나 서비스를 테스트 하기 위해서 Client를 실행하거나 서비스를 실행하고 파일을 특정 폴더에 넣거나 하는 행동을 하고 있습니다.
이는 굉장히 소모적인 행동으로 자신이 개발하는 부분에 대해서만 디버깅이 가능해야 합니다.
또한 배포 할 때 불안에 떨어야 하지만 테스트가 많이 있다면, 부담을 덜 수 있고, 사이드 이펙트가 발생하는 부분을 빠르게 찾을 수 있습니다.

gRPC Controller 부분을 서비스 실행 없이 디버깅을 하는 방법에 대해 알아봅니다.



