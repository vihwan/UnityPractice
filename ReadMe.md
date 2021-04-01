# 개발 일지

## 확인된 버그
* UI 상태창이 해상도에 맞게 조절되지 않는다.
* 가끔 지형의 고도에 따라 플레이어가 지멋대로 돌아간다.
* 왼쪽 마우스를 클릭하여 공격하면, 공격 모션이 여러번 실행된다.
* 돼지에게서 날고기 아이템이 생성될 경우, 간헐적으로 Terrain을 뚫고 생성된다.
* 돼지가 벽으로 몰릴 경우, 혼자 버벅거리는 현상이 있다. 길을 찾지 못하는 듯 하다. ->Collider의 한계인가


-------------------------------------------------------------------

## 추가 개발하고 싶은 것
* 

-------------------------------------------------------------------

## 2021.04.01 (목)
1. 인벤토리 혹은 크래프트 UI가 열려있을 때, 캐릭터가 특정 동작하지 않도록 조건문을 추가하여 수정.
2. 슈퍼 몬스터를 잡았을 때, 평소보다 더 많은 아이템이 펑 하고 흩어지며 터져나오도록 구현 (Random.Range를 이용하여 추가적으로 Vector3를 더했다.)
3. (Bug Fix) Stair를 설치할 때, 설치 할 수 없는 곳에 프리팹의 색이 빨강으로 변하지 않는 현상을 수정.
4. (Bug Fix) 특정 무기를 든 상태에서 똑같은 무기로 교체할 경우, 교체 모션이 나오는 현상을 수정.
5. (Bug Fix) 공격시 RayCast의 값이 null로 받아져 NullReferenceException 오류가 뜨는 현상을 수정.
6. (Bug Fix) 달릴 때, 스태미너 소모가 지속적으로 일어나지 않는 현상을 개선.





### 2021.03.31 (수)
1. 총으로 공격할 때, Raycast를 통해 대상의 hitinfo를 찾아 데미지를 주도록 실행.
2. Pig의 Box Collider 크기를 재설정
3. SuperMonster를 사냥할 경우, 드랍되는 아이템의 수가 다르도록 구현
4. FireCamp 설치를 위한 Craft UI를 생성
5. FireCamp 아이콘을 클릭 시, 화면에 미리보기 prefab이 생성되고 원하는 위치에 클릭하면 생성되도록 구현
6. OnTriggerEnter와 Exit를 이용해 설치하려는 장소에 다른 Collider가 존재할 경우 설치할 프리팹의 색상을 변경하도록 구현
7. Craft UI에 Stair 오브젝트를 생성 및 똑같이 동작되도록 구현 -> 코드를 좀 더 간략화


### 2021.03.30 (화)
1. Terrain을 재구성. 
2. Pig에 Nav Mesh Agent를 적용.
3. Particle System을 이용하여 모닥불을 구현
4. 모닥불 안에 Point Light를 생성
5. (Bug Fix) 달리다가 자기 멋대로 굳는 현상을 수정


### 2021.03.29 (월)
1. 동물 Pig를 생성
2. 걷기, 뛰기, 달리기 등등의 애니메이션 생성
3. 스스로 행동을 랜덤하게 취하는 AI를 제작
4. 피해를 입을 시, 도망가도록 설정 및 사망 시 날고기 아이템을 떨어뜨림
5. Animal 스크립트를 생성. StrongAnimal, WeakAnimal의 자식 스크립트를 생성 및 Pig에 WeakAnimal을 상속
6. (Bug Fix) 슬롯 상세 정보창이 켜진 상태에서 인벤토리를 닫을 시, 정보창이 같이 사라지지 않는 현상을 수정.


### 2021.03.26 (금)
1. 인벤토리 UI 생성
2. 아이템을 획득할 시 인벤토리 슬롯에 아이템이 들어가도록 설정
3. 인벤토리 내 아이템끼리 슬롯을 바꿀 수 있도록 설정
4. 슬롯 내 아이템의 상세정보를 볼 수 있도록 설정
