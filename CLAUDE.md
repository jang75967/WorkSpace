# CLAUDE.md

Behavioral guidelines to reduce common LLM coding mistakes. Merge with project-specific instructions as needed.

**Tradeoff:** These guidelines bias toward caution over speed. For trivial tasks, use judgment.

## 1. Think Before Coding

**Don't assume. Don't hide confusion. Surface tradeoffs.**

Before implementing:

- State your assumptions explicitly. If uncertain, ask.
- If multiple interpretations exist, present them - don't pick silently.
- If a simpler approach exists, say so. Push back when warranted.
- If something is unclear, stop. Name what's confusing. Ask.

## 2. Simplicity First

**Minimum code that solves the problem. Nothing speculative.**

- No features beyond what was asked.
- No abstractions for single-use code.
- No "flexibility" or "configurability" that wasn't requested.
- No error handling for impossible scenarios.
- If you write 200 lines and it could be 50, rewrite it.

Ask yourself: "Would a senior engineer say this is overcomplicated?" If yes, simplify.

## 3. Surgical Changes

**Touch only what you must. Clean up only your own mess.**

When editing existing code:

- Don't "improve" adjacent code, comments, or formatting.
- Don't refactor things that aren't broken.
- Match existing style, even if you'd do it differently.
- If you notice unrelated dead code, mention it - don't delete it.

When your changes create orphans:

- Remove imports/variables/functions that YOUR changes made unused.
- Don't remove pre-existing dead code unless asked.

The test: Every changed line should trace directly to the user's request.

## 4. Goal-Driven Execution

**Define success criteria. Loop until verified.**

Transform tasks into verifiable goals:

- "Add validation" → "Write tests for invalid inputs, then make them pass"
- "Fix the bug" → "Write a test that reproduces it, then make it pass"
- "Refactor X" → "Ensure tests pass before and after"

For multi-step tasks, state a brief plan:

```
1. [Step] → verify: [check]
2. [Step] → verify: [check]
3. [Step] → verify: [check]
```

Strong success criteria let you loop independently. Weak criteria ("make it work") require constant clarification.

---

**These guidelines are working if:** fewer unnecessary changes in diffs, fewer rewrites due to overcomplication, and clarifying questions come before implementation rather than after mistakes.

<br>

# Global Rules

## Core Principles

- Perform the task as stated in the prompt. Do not do anything that is not requested.

## Prohibited Matters

- Do not add unsolicited refactoring, optimization, error handling, or comments.
- Do not create unnecessary abstractions, interfaces, or helper classes.
- Check before working if the scope of the change is unclear.

## How to Work

- Check and leverage existing code and fields first before adding new features.
- Briefly describe concepts changed after code work.
- During structural design and idea discussion, do not propose work before the user directs the work.

<br>

## Project Context

### 프로젝트 성격

- **20년 이상된 레거시 WPF 데스크톱 앱** (DMF25 / DMS)
- 비즈니스 로직과 UI가 분리되지 않아 테스트 자동화 어려움
- .NET 런타임 미지원 환경 → 자동화 테스트 불가, 웹처럼 완전한 검증 불가
- UI 요소 수천 개 → 기능 보장 어려움

### 작업 안전 원칙

- **기존 동작을 완벽하게 보존**하는 것이 최우선
- 코드 수정 전 반드시 영향 범위 파악 및 여러 번 검토
- 사이드이펙트 최소화: 변경 범위를 최대한 좁게 유지
- 확신이 없으면 질문 후 진행 (CLAUDE.md의 "Check before working" 규칙)
- "안전하다고 생각했는데 전혀 생각 못한 곳에서 깨지는" 패턴이 반복됨 → 과신 금지

### 현재 작업 단계

- Client 소스코드 중심으로 **짧은 범위 → 큰 범위** 순서로 개선 진행 중
- 순서: 분석 → 설계 → 구현
- 참조: docs/ 폴더의 분석/설계 문서

### #432 사례 (반면교사)

트랜잭션 패턴 대규모 변경 후 즉시 10개 파일 버그 수정 필요 → 대규모 일괄 변경의 위험성 입증

## Project References

@docs/00-dmm25-mr-reflection-analysis.md
@docs/01-project-analysis.md
@docs/02-client-deep-analysis.md
@docs/03-client-refactoring-design.md
@docs/04-client-implementation-strategy.md
@docs/05-client-core-module-safety.md
@docs/06-scicharting-sadcchart-analysis.md
@docs/07-begintransaction-nesting-analysis.md
@docs/08-client-i18n-audit.md
