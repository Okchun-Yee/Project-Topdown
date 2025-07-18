# 🎮 Project-Topdown

> Unity 기반의 게임 프로젝트입니다.  
> Top view 시점을 기반으로하는 로그라이크 게임입니다.
> 여러 종류의 무기가 있으며 한 개의 무기를 자신만의 스타일로 강화해나가서 게임을 진행할 수 있습니다.

---

## 📌 프로젝트 개요

- **장르**: TopDown Rougelike
- **플랫폼**: Windows 
- **엔진 버전**: Unity 2022.3.6f1 (LTS)
- **언어**: C#
- **진행 기간**: 2025.06.21 ~ 

---

## 🔧 주요 기능

- ✅ 무기 교체 및 무기 강화 시스템
- ✅ 어드밴처 형식과 보스전

---

## 🗂️ 폴더 구조

┣ 📂Animations/<br>
┣ 📂Materials/<br>
┣ 📂Prefabs/<br>
┣ 📂Scenes/<br>
┣ 📂Script/<br>
┃ ┣ 📂Enemy/ (# Enemy 로직: PathFinding, Damage, Health, Attack...)<br>
┃ ┣ 📂Management/ (# 관리 로직: Weapon, Scene, Map, UI .....)<br>
┃ ┣ 📂Misc/ (# 기타 환경 로직: 상호작용, 스테이지 ....)<br>
┃ ┣ 📂player/<br>
┃ ┗ 📂UI/<br>
┣ 📂ScriptableObjects/<br>
┣ 📂Settings/<br>
┣ 📂Sprites/<br>
┗ 📂TileMap/<br>

---

## 🛠️ 사용 기술 및 툴

| 항목 | 기술/툴 |
|------|---------|
| Engine | Unity 2022.3.6f1 (LTS) |
| Language | C# |
| Version Control | Git / GitHub |
| 외부 라이브러리 | - |
| 협업 도구 | Notion |

---

## ▶️ 실행 방법

`unkonwn`

---

## 🚀 빌드 방법

`nukonwn`

---

## 🤝 기여자

| 이름 | 역할 |
|------|------|
| 한채훈 | 시스템 구현, UI 개발 |
| -- | 게임 기획, 레벨 디자인 |
| -- | 아트 디자인 |

---

## 📄 라이선스

`--`

---

## 🧩 Weapon System

> 무기 교체 & 강화 시스템
> 플레이어는 게임을 진행하면서 여러가지 무기들을 마주치게 됩니다. 이때 모아둔 재료들을 사용해서 자신의 무기를 강화할지 OR 더 높은 등급의 무기로 교체할지 선택하게됩니다.
> 게임을 진행하면서 자신만의 스타일의 무기로 강화하며 플레이 할 수 있습니다.

---

### 📌 시스템 개요

- 무기 강화: 강화 재료를 소모하여 독특한 특징을 가지는 무기로 강화할 수 있습니다.
- 더 좋은 등급의 무기로 교체 할 수 있습니다. 교체시 기존 무기에 사용한 재료 및 강화단계는 초기화됩니다.

---

### 🗂️ 구성 요소

| 요소명 | 설명 |
|--------|------|
| `IWeapon`     | 무기가 제공할 __행동 계약__을 정의 (모든 무기 타입이 가져야하는 기능을 통일된 형태로 선언) |
| `Weapoinfo`    | 3종류의 무기에 대한 UI 아이콘 이미지 & Prefabs & 무기 정보 & 무기 쿨타임 & 무기 피해량 & 무기 사정 거리 등 무기에 대한 기본 정보를 가지는 Class |
| `BaseWeapon`   | 각자의 무기들이 상속받을 부모 클래스로 모든 종류의 무기들이 기본적으로 가져야할 무기 정보를 상속 & Attack 동작 및 무기별 쿨타임 로직을 관리 |
| `ActiveWeapon` | 공격 버튼 입력 & 무기 교체 시 기존 무기 삭제 and 새로운 무기 장착을 관리 |

---

### 🎮 작동 방식

#### 📍 상태 흐름
1. 필드에 존재하는 무기 Prefab에 접근 후 일정 범위 내에서 `G` 입력
2. UI 아이콘은 3종류 중 해당 하는 아이콘으로 변경
3. 기존 무기 존재 시 -> 기존 무기 파괴 -> 새로운 무기 장착
4. `Mouse Left` 시 Onattak() 호출 -> 각 무기마다 재정의한 OnAtack이 동작

#### 📍 Input
- 입력 키: `G` (기본값)
- 공격 키: `Mouse Left` 등

---

### 🪄 UI

UI:          ![curItemSlotsUI](https://github.com/user-attachments/assets/ecb425c6-4d9f-424f-98bc-76a461e8b802) <br>
Weapon Icon: ![IconUI](https://github.com/user-attachments/assets/c6408c98-21a0-4166-9d23-91595cccdf97)

---

### 📈 확장 방향

- unkonwn

---
## 🧪 개발 노트

 - [x] 무기 교체 시스템(UI 교체 포함)
 - [ ] 무기 강화 시스템
 - [ ] 무기 쿨다운 UI 추가
 - [ ] 근접 무기 콤보 입력 추가
 - [ ] 마법사 무기 스킬 시스템 추가

---
## 🧩 Enemy System

---
## 🧩 Skill System
