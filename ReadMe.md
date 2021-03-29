# 개발 일지

## 확인된 버그
* 달리다가 자기 멋대로 걷는 순간이 있다.
이후 일부 기능들이 제대로 동작하지 않는다.
모든 무기가 다 이렇게 동작한다.
* UI 상태창이 해상도에 맞게 조절되지 않는다.
* 가끔 지형의 고도에 따라 캡슐이 지멋대로 돌아간다.
* 왼쪽 마우스를 클릭하여 공격하면, 공격 모션이 여러번 실행된다.
-------------------------------------------------------------------

## 2021.03.29
1. 동물 Pig를 생성
2. 걷기, 뛰기, 달리기 등등의 애니메이션 생성
3. 스스로 행동을 랜덤하게 취하는 AI를 제작
4. 피해를 입을 시, 도망가도록 설정 및 사망 시 날고기 아이템을 떨어뜨림
5. Animal 스크립트를 생성. StrongAnimal, WeakAnimal의 자식 스크립트를 생성 및 Pig에 WeakAnimal을 상속
6. (Bug Fix) 슬롯 상세 정보창이 켜진 상태에서 인벤토리를 닫을 시, 정보창이 같이 사라지지 않는 현상을 수정.


## 2021.03.26
1. 인벤토리 UI 생성
2. 아이템을 획득할 시 인벤토리 슬롯에 아이템이 들어가도록 설정
3. 인벤토리 내 아이템끼리 슬롯을 바꿀 수 있도록 설정
4. 슬롯 내 아이템의 상세정보를 볼 수 있도록 설정
