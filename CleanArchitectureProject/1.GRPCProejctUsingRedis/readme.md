## grpc 통신으로 redis queue에 job 저장

- stream 말고 queue만 사용해도 됨
- mediatR
- polly (redis connection 부분)
- option (grpc DTO 객체로 사용)

### 폴더 구조 (프로젝트 구조)

+- RedisLibrary  
+- Server  
+- Client

- 개발한 c# 서비스 도커파일로 만들어서 도커 컨터이너로 올리기
- VS 프로젝트에서 WorkerSerivce로 프로젝트 만들어야 함 (단순 콘솔 말고)
  - url 참고
  - 서버, 클라 플젝 동시 실행 옵션설정

---

## next step
