# 웹 프론트엔드 변천사와 아키텍쳐

## 웹 프론트엔드를 바닐라로 만들다보면 꼭 나오는 주제

- 규모가 점점 커지고 있다.
- 프론트도 객체지향 프로그래밍을 할 수 있지 않을까?
- MVC 프레임워크 적용한다면?

<br>

## 프론트엔드 트렌드 변천사

> 1. HTML, CSS, JS의 탄생 | 관심의 분리와 느슨한 결합
> 2. jQuery 시대 | DOM을 쉽게 쓰자
> 3. HTML + JS 합치니까 더 낫다? | MVC 컴포넌트 방식의 탄생
> 4. 선언적으로 만들자 | 데이터 바인딩 + 템플릿 => MVVM 웹 프레임워크 탄생
> 5. 컴포넌트 간 데이터 교환이 너무 복잡하고 어렵다. | Container-Presenter 방식
> 6. Props Drill 문제 해결 | Flux와 Redux
> 7. 너무 복잡하다 | hooks와 context, Recoil, Zustand, jotai
> 8. 대부분 서버 API 관리를 위해 사용하는 용도일텐데? | React Query, Redux Query

<br>

#### 1. HTML, CSS, JS의 탄생 | 관심의 분리와 느슨한 결합

- HTML, JS 다음 CSS 순서대로 탄생하여 각자의 방식대로 성장
- HTML은 서버가 작성하는 영역
- JS는 간단한 동작
- CSS는 화면 관리

#### 2. jQuery 시대 | DOM을 쉽게 쓰자

- Ajax 탄생으로 서버에서 HTML 만들지 않고 데이터만 교환 가능하게 됨
- JS를 이용해 데이터로 DOM을 조작하는 작업이 중요해짐
- jQuery 같은 Ajax, DOM 잘 다룰 수 있는 라이브러리 사용

#### 3. HTML + JS 합치니까 더 낫다? | MVC 컴포넌트 방식의 탄생

- HTML과 JS를 함께 다루는 편이 더 나았고 이후 앱을 만들던 MVC 아키텍쳐를 표방
- 데이터를 조작하고 DOM을 조작하는 로직을 하나로 관리하는 방향으로 발전
- 화면 단위가 아닌 컴포넌트 단위로 발전

<br>

## MVC [-백엔드-] 수행 절차

- client의 **[-request-]** 를 받는다.
- request를 **[-분석-]** 한다. (Routing)
- 필요한 **[-데이터를 수집/가공-]** 한다.
- **[-뷰를 생성-]** 하고 response 한다. (client에 보여줄 웹페이지)

<br>

#### 문제점 | 강한 의존성 발생

**각각의 Layer들이 다음 단계의 Layer의 존재를 알고 있어야 한다.**

- Controller는 Model을 알아야 한다.
- Model은 View를 알아야 한다.(호출해야 한다.)
- View는 Controller를 호출해서 결과 응답한다.

![1](/uploads/3e5412fa5a3140a85c29ff190ada52ad/1.png)

<br>

#### 실제 MVC 활용법

- Controller가 Model을 통해 데이터 수집, 가공 요청해 받아온다.
- 받아온 Model 정보를 View에 전달한다.

```js
model = new Model();
view = new view(model);
view.makeHTML();
```

> **[+완전한 의존성 제거는 하지 못하지만 중간 계층인 Controller에 많은 역할을 부여해 어느 정도 해결한다.+]**

![2](/uploads/6b8e1fb0a7da8f525378ee6f7b58751b/2.png)

<br>

## [+프론트+]도 MVC 사용하면 될까?

#### 보통 MVC에서 View는 만들어지는 결정체일뿐

- 옛날 웹페이지와 다르게 요즘 프론트는 복잡한 view가 많아짐
- 프론트는 모두 View라고 할 수 있다.
- 프론트의 View는 **온갖 이벤트**가 발생한다.

![3](/uploads/d651fdf34f6d98abd80140e9f348c42c/3.png)

<br>

#### 프론트엔드의 특성을 Model과 View의 관계로 정리해보면?

- 1. View의 변경으로 Model을 바꿔야 하는 경우
- 2. Model의 변경으로 View를 바꿔야 하는 경우

![6](/uploads/bb107a40378bf8772e0bd101368a19a8/6.png)

> **View와 Model이 양방향으로 인터렉션 일어난다. (강한 의존성)**  
> **프론트엔드의 View는 매우 많다.**  
> **프론트엔드 구현 복잡도가 올라간다.**  
> **[+중간에 Controller를 둔다고 해도 Controller의 역할이 너무 많아진다.+]**

![7](/uploads/a358fec2f66a4b38b8166a38e5d0a8c5/7.png)

<br>

#### [+프론트엔드 특징 中+] | View는 계층적인 구조를 가지는게 필요하다.

- View는 DOM을 표현하는 것 (트리구조)
- 우리가 View를 나눌때도 DOM만큼 잘게 나누진 않지만, 어느 정도의 트리구조를 가지면서 제어하도록 설계되어야 한다.
- **[-리렌더링(DOM 조작)이 잦기 때문에-]** 거대한 View를 계속 리렌더링 하기보단, 잘게 나눈 단위로 리렌더링이 효율적

![8](/uploads/3d303943a7ffb999530b73a35d1b83d5/8.png)

<br>

> 정리 | 프론트엔드는
>
> - View가 매우 많다.
> - 양방향 처리가 필요하다.
> - Controller 제어 시 Controller의 역할이 매우 커진다.
> - View 간 계층 처리가 필요하다.

> 필요한 것
>
> - 복잡한 View와 Model 관계 단순화
> - View 계층 처리로 쉽고 효율적인 DOM 처리

#### 4. 선언적으로 만들자 | [+데이터 바인딩 + 템플릿+] => MVVM 웹 프레임워크 탄생

<br>

#### 데이터 바인딩

- View <-> Model 자동 변경
- 프레임워크 사용하지 않고 구현하려면 defineProperty, proxy API, 옵저버패턴 등으로 구현해야 한다.

```js
// Svelte 예시
<script>
let name = 'world';
</script>

<h1>Hello {name}!</h1>

<input bind:value={name}/>
```

#### MVVM

- View에 변경 사항 일어나면 ViewModel 적용
- ViewModel이 바뀌면 View에 변경사항 업데이트

![9](/uploads/14b61a1a8d831c26531049370898dbe6/9.png)

> ### **[+매번 데이터 변경 때마다 템플릿 방식으로 HTML을 작성하는 방안이 자동화되어 Angaulr, React, Vue 등 발전+]**

#### ex) Vue.js 에서 MVVM 프레임워크 지원

![10](/uploads/2fb572a563c8dc8e0426b21488f2a29a/10.png)

<br>

#### 5. 컴포넌트 간 데이터 교환이 너무 복잡하고 어렵다. | Container-Presenter 방식

- 데이터가 많아지고 로직이 분리되면서 점점 복잡해지는 컴포넌트
- 재사용성이 떨어지면서 **데이터를 받아서 보여주기만 하는 'Prsenter형 컴포넌트'** 와 **데이터 요청 및 처리의 'Container형 컴포넌트** 로 분리
- Container에서 props를 Presenter로 내려주면서 로직을 한군데에 모으고 화면을 다루는 View 방식이 재사용 형태의 주류 아키텍처로 적용

![14](/uploads/b14f8d67fc4326fd780719003f1a732d/14.png)

<br>

#### 6. Props Drill 문제 해결 | Flux와 Redux

- 상위 props들이 하위로 전달되는 과정에서 중간 컴포넌트를 불필요하게 거쳐가야 하는 문제 발생 (단단한 결합)
- 비즈니스 로직을 굳이 컴포넌트 계층 구조로 만들 필요 없이 View와 비즈니스 로직을 분리한 단방향 데이터 구조를 가지는 Flux 패턴, Redux 등 도입 (상태관리)

<br>

#### React Flux 아키텍쳐

**한방향으로 흐름을 제어하며 동작**

- View에서 변경 사항 생기면 Dispatch 발생
- Dispatcher를 통해 Store에 전달
- Store에 변경 사항 있으면 다시 View로 바뀜

![11](/uploads/54505647bd5787521ed5e44c9e00b8f1/11.png)

#### React 상태관리 라이브러리 (Redux)

- Flux 아키텍쳐 따라 만들거나
- Store가 바뀌면 View를 렌더링한다.

![12](/uploads/238cce8536a4d77cfc9a58217abceb85/12.png)

#### ex) Redux | store에 state 사용법

- state 보관

![15](/uploads/90f5d85e358696fc4cfe14c11fa45a25/15.png)

- state 꺼내는 법

![16](/uploads/4f00dd5540c57200bf0fd50b1055d52c/16.png)

- state 변경

![17](/uploads/a477e12762372b836dba230639b465dc/17.png)

![18](/uploads/62623636f214f263495f5eb2e328e10d/18.png)

<br>

#### 7. 너무 복잡하다 | hooks와 context, Recoil, Zustand

- 컨셉은 좋으나 과한 문법 체계인 Redux는 대형 프로젝트 외에 오버엔지니어링 발생
- 너무 많은 보일러플레이트 코드
- hooks (useState, useEffect 등) 을 통해 간결한 문법으로 외부 비즈니스 로직을 쉽게 연동
- React에서 기본적으로 제공하는 hook API만으론 전역 상태관리가 용이하지 않아, **전역객체를 이용해 데이터를 기록하고 변경감지를 통해 View로 전달** 하는 형태인 Recoil, Zustands 등 제시

#### ex) hooks | state 사용 예시

![19](/uploads/daead0a51c12d1359ff57d5a33dc6d54/19.png)

#### Atomic | Recoil, Svelte Store, Vue Composition, jotai

- 거대한 View영역과 Store영역을 나누어 이분법으로 생각하자는 의견에는 동의하나 Action ~ Dispatch ~ Reducer와 같은 복잡한 구조를 가져야 하는가에 대한 방법에 대해서는 회의적인 시각으로 만들어진 방법

![21](/uploads/31706c0d3533a5df40ab39bfb5c0ce94/21.png)

![22](/uploads/b7dab308a1f76149755c603de7d59705/22.png)

![23](/uploads/ea7067097a93b49635d950c02eaaa4d7/23.png)

<br>

#### 8. 대부분 서버 API 관리를 위해 사용하는 용도일텐데? | React Query, Redux Query

- 대부분의 프론트엔드에서 전역적인 상태관리가 필요한 이유는 서버와 API
- React-Query는 이러한 상태관리에 편향되어 있던 시각에서 벗어나 고전적인 ajax의 데이터를 Model로 간주
- 대부분의 프론트엔드 개발은 서버 데이터를 CRUD하고 시각으로 그리는 것에 중점이 되어 있는데, FLUX나 Atomic은 너무 복잡하다.
- **백엔드와 직접 연동해 기존 상태관리에서 로딩, 캐싱, 무효화, 업데이트 등 복잡하게 진행하던 로직들을 단순하게 만들어주는 방식 탄생**

#### ex) React Query 예시

![20](/uploads/2b71082e0d52d429f48fd514deb9a207/20.png)

<br>

## 정리

#### 패러다임의 확장과 개선이 있기 까지는 많은 시간이 걸린다.

- **이러한 개선의 시작은 바로 현재의 개발 방식에서 불편함을 느끼는 것**
- **불편한 것을 찾고 하지 말아야 할 규칙을 찾는 것부터 시작**
