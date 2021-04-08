using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private int hp;               //바위의 체력
    [SerializeField] private int rockMaxCount;     //바위 최대 생성 갯수
    [SerializeField] private float destroyTime;    //바위 파괴후 부서진 바위 파괴 대기시간
    [SerializeField] private string strike_Sound;  //바위를 내려칠 때 사운드
    [SerializeField] private string destroy_Sound; //바위가 파괴될 때 사운드

    //필요한 컴포넌트
    [SerializeField] private GameObject go_Rock;            //일반 바위
    [SerializeField] private GameObject go_Debris;          //부서진 바위
    [SerializeField] private GameObject go_Effect_Prefab;   //채굴 이펙트 - 작은 바위 파편
    [SerializeField] private GameObject go_RockItem_Prefab; //돌멩이 아이템
    [SerializeField] private SphereCollider sphereCollider; //남아있는 컬라이더를 비활성화 시킬 목적

    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);
        //Sphere컬라이더의 가장자리 센터, 쿼터니언 방향 identity(기본값)을 받는다.
        ///param 
        ///1. gameObject 
        ///2. Vector3 position 
        ///3. Quaternion rotation
        var clone = Instantiate(go_Effect_Prefab,sphereCollider.bounds.center,Quaternion.identity);
        Destroy(clone, destroyTime);
        hp--;
        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);
        sphereCollider.enabled = false;
        //그냥 Random으로 하면 모호한 값이라고 오류가 뜬다.
        //그 이유는 UnityEngine에도 있고, C#내의 .NET에서도 지원하기 때문에 확실하게 명시할 필요가 있다.
        for (int i = 0; i < Mathf.Round(UnityEngine.Random.Range(1, rockMaxCount)); i++)
        {
            Instantiate(go_RockItem_Prefab, go_Rock.transform.position, Quaternion.identity); //돌 아이템 생성
        }
        Destroy(go_Rock);

        go_Debris.SetActive(true);
        Destroy(go_Debris, destroyTime); //destroyTime만큼 일정 유예 시간을 준뒤 파괴
    }
}
