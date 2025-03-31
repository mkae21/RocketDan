## 2D 모바일 게임 개발자 과제

- 과제 내용: 이동하는 타워 앞에 쌓이는 몬스터 구현

## 📦 주요 기능

- **오브젝트 풀링 (Object Pooling)**

  - 좀비는 Instantiate 대신 큐를 이용한 풀링 방식으로 재사용
  - 10초 후 자동 비활성화 및 풀로 반환

- **레이어 기반 필터링**
  - `TopZombie`, `MiddleZombie`, `BottomZombie` 레이어로 라인별 관리
  - 같은 라인의 좀비만 Raycast로 감지
