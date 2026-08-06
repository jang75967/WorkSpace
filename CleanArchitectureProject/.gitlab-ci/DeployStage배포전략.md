## Deploy Stage 배포 전략

### 구성: 4개 잡 (TEST 2 + PROD 2)
| 잡 | 티어 | 트리거 | 대상 |
|---|---|---|---|
| `deploy.server` | TEST | release_/hotfix push 시 자동 | 170.44 `docker-service-dmm`(M)/`docker-service-dmf`(F) |
| `deploy.client` | TEST | release_/hotfix push 시 자동 | 170.43 `PublishTest`(M)/`PublishTest_Foundry`(F) |
| `deploy.server.prod` | 운영 | 수동 버튼 | 189(M)/229(F) `docker-service` |
| `deploy.client.prod` | 운영 | 수동 버튼 | 170.43 `Publish`(M)/`Publish_Foundry`(F) |

### 공통 규칙
- **`resource_group: deploy`** → deploy stage 잡은 **한 번에 하나씩**만 실행 (ssh/smb/IO 충돌 방지). 러너 동시성과 무관하게 직렬화.
- 브랜치 게이트: `release_*` / `hotfix*` push 에서만 동작.

### TEST 티어 (자동)
- push 시 **server·client 각각 독립 실행** (순서 무관, 한쪽 실패해도 다른쪽 진행).
- 산출물 버전을 **dotenv** 로 기록(`SERVER_PUBLISH_VERSION` / `CLIENT_PUBLISH_VERSION`) → 운영 승격이 그대로 사용.

### 운영 티어 (수동 + 조건부)
- **`needs`**: 각자 TEST(server/client)가 **성공해야** ▶버튼 활성화.
- **props 게이트**(스크립트 내 `git diff`): 해당 props(`DMSServerVersion.props`/`DMSClientVersion.props`)가 **변경된 push일 때만** 실제 배포, 아니면 눌러도 skip. `git diff` 실패 시 안전하게 skip(fail-closed).
- `when: manual` + `allow_failure: true` (안 눌러도 파이프라인 성공).
- **Build once / Promote**: 재빌드 없이 **TEST가 만든 그 버전 산출물**을 dotenv 버전으로 정확히 집어 운영으로 복사 (server=scp, client=170.43 내부 WinRM zip+ManifestPatcher).

### 동작 요약
```
push(release_/hotfix)
├─ deploy.server (TEST, 자동) ──┐ (직렬, 순서무관)
└─ deploy.client (TEST, 자동) ──┘
└─ 성공 시 ▶ deploy.*.prod 활성 → props 변경 시 누르면 운영 승격
```