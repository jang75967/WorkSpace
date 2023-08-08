## 키워드
* CleanArchitecture, DI, Mapper
* WebApplicationFactory.ConfigureWebHost()
* Xunit -> IAsyncLifetime

## 추가사항
* 만약 추후 Mapper를 사용하게 된다면 AutoMapper를 사용 할 예정입니다.
 
## Mapper

백엔드 개념이 들어가게 되면 API Service와 Client나 타 서비스는 DTO라는 Type을 가지고 서로 주고 받게 됩니다.
이때 DTO와 Entity, Model등 계속 변환하는 로직이 들어가게 되면 도메인 로직이 훼손되게 됩니다.
도메인 로직을 명확히 사용하기 위해서 도와주는 패키지가 Mapper가 있습니다.


## WebApplicationFactory

서비스의 특정 부분을 테스트하기 위해서는 필요한 부분이 많습니다.
이때 기본적인 DI 부분은 그대로 사용하면서 필요한 DI만 수정해서 테스트 코드를 실행 시킬 수 있게 도와주는 패키지입니다.

## 아키텍처

계층형 아키텍처, 클린 아키텍처, 헥사고날 아키텍처 등 아키텍처가 추가하는 방향성은 모두 같습니다.
특정 시스템, 패키지, 클래스 등에 의존성을 가지지 않도록 하기 위함입니다.
현재 IMapper를 통해 AutoMapper와 Mapster를 DI 만으로 변경이 가능합니다.
이는 특정 패키지에 의존성을 가지지 않는 것을 뜻합니다.
