# 📦 Project-Topdown
탑뷰/로그라이트/Youtube: darkswear참조/

## 📚 Table of Contents
---
장르 : Top Down RougeLike
프로젝트 설정
>Unity 버전 : Unity 2022.3.61f1

## 📦Assets
 ┣ 📂Animations 
 ┣ 📂Materials
 ┣ 📂Prefabs
 ┣ 📂Scenes
 ┣ 📂Script
 ┃ ┣ 📂Enemy (# Enemy 로직: PathFinding, Damage, Health, Attack...)
 ┃ ┣ 📂Management (# 관리 로직: Weapon, Scene, Map, UI .....)
 ┃ ┣ 📂Misc (# 기타 환경 로직: 상화작용, 스테이지 ....)
 ┃ ┣ 📂player
 ┃ ┗ 📂UI
 ┣ 📂ScriptableObjects
 ┣ 📂Settings
 ┣ 📂Sprites
 ┗ 📂TileMap


## Weapon System
---
> 플레이어는 기본적으로 한번에 한 개의 무기만 을 소지 할수 있습니다.
> 무기의 종류는 현재 3자리 종류로
>
> **근접 무기, 원거리 무기, 마법 무기** ![IconUI](https://github.com/user-attachments/assets/08676b36-4e85-470e-8774-f12066f3a314)

### 무기 획득 방식
>필드 내에서 획득
- 씬 플레이 중 플레이어가 WeaponPickup 트리거 영역에 진입
- 지정한 키(G 기본값) 입력 시, 기존 무기 해제 후 새 무기 장착
- 무기 공격 입력 시 현재 장착된 무기(BaseWeapon 파생 클래스)에서 +OnAttack()+ 실행
- 무기 교체 시 UI 아이콘 자동 갱신

### 무기 시스템 방향
>3종류의 무기는 게임을 진행해 나가면서 더 좋은 등급의 무기로 ** 무기 교체 OR 기존 무기 업그레이드 ** 중 하나를 선택하면서 자신만의 멋진 무기를 완성해 나갈수 있습니다.




###### Commit
| Prefix     | 의미                     | Ex                 |
| ---------- | ---------------------- | -------------------------- |
| `feat`     | 새로운 기능 추가               | `feat: 무기 시스템 구현`        |
| `fix`      | 버그 수정                      | `fix: 물리 에러 해결`           |
| `refactor` | 리팩토링 (기능 변화 없음)      | `refactor: 사용자 서비스 구조 개선`  |
| `docs`     | 문서 수정                      | `docs: README에 ERD 추가`     |
| `test`     | 테스트 코드 추가/수정          | `test: 감정 추천 API 테스트 추가`   |
| `chore`    | 빌드, 패키지 관리 등 잡일      | `chore: .gitignore 업데이트`   |
| `system`   | 시스템 수정                    | `system: 무기 시스템 구조 수정`   |
| `revert`   | 이전 커밋 되돌리기             | `revert: 적 움직임 기능 롤백`        |

###### Description
🐞 BUG 내용
  -
  -
🔧 FIX 내용
  -
  -
🎯 다음 작업 목표
  -
  -
🐼 코멘트
  -
  -
