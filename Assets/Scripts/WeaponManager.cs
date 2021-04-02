using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    // 공유 자원 : 정적변수
    //보호수준이 떨어지고 많이 쓸수록 메모리가 낭비
    //중복 교체 시행 방지 용도
    public static bool isChangeWeapon = false;

    [SerializeField]
    private float changeWeaponDelayTime; //무기 교체 지연시간
    [SerializeField]
    private float changeWeaponEndDelayTime; //무기 교체 완전히 끝난 시점;

    //무기 종류 관리
    [SerializeField]
    private Gun[] guns; //총기들
    [SerializeField]
    private CloseWeapon[] hands; //맨손 너클 등
    [SerializeField]
    private CloseWeapon[] axes; //맨손 너클 등
    [SerializeField]
    private CloseWeapon[] pickaxes; // 곡갱이

    //관리차원에서 딕셔너리를 사용, 쉽게 접근이 가능
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();


    //현재 무기와 현재 무기의 애니메이션 : 정적변수
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;


    //현재 무기 타입
    [SerializeField]
    private string currentWeaponType;


    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;


    private void Start()
    {
        //등록된 무기를 딕셔너리에 등록
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    private void Update()
    {
        if (!isChangeWeapon)
        {
            //숫자 1 버튼을 눌렀을 경우
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            { //숫자 2를 눌렀을 경우

                //무기 교체 실행(서브머신건)
                //코루틴으로 작성
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            { //숫자 3를 눌렀을 경우

                //무기 교체 실행(서브머신건)
                //코루틴으로 작성
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            { //숫자 4를 눌렀을 경우

                //무기 교체 실행(서브머신건)
                //코루틴으로 작성
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }

    //퀵슬롯 1,2,3,4로 만들어서 사용
    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        if(!GameManager.canPlayerMove || CraftManual.isPreviewActivated)
        {//플레이어가 움직일 수 없는 상태 혹은 건축물 미리보기 프리팹이 실행중이라면 무기 교체 동작이 안된다.
            yield break;
        }

        if(currentWeaponType == _type)
        {//같은 무기의 타입이라면 교체하지 않는다. (임시)
         //나중에 무기의 이름까지 비교하도록 수정할 것.
            yield break;
        }
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out"); //무기 집어넣음

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction(); //직전 행동 취소
        WeaponChange(_type, _name);

        //무기 넣고 꺼내고 난 이후까지 대기
        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            theGunController.GunChange(gunDictionary[_name]);
            Debug.Log("총으로 체인지");
        }
        else if (_type == "HAND")
        {
            theHandController.CloseWeaponChange(handDictionary[_name]);
            Debug.Log("손으로 체인지");
        }
        else if (_type == "AXE")
        {
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
            Debug.Log("도끼로 체인지");
        }
        else if (_type == "PICKAXE")
        {
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
            Debug.Log("곡갱이로 체인지");
        }
    }


    //직전의 무기 사용 액션을 취소시키는 함수
    //기존에 쓰고 있던 무기를 비활성화 시킨다.
    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                //재장전 도중 무기가 교체되면 안되니 무기 교체시 취소시키자.
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false;
                break;
        }
    }
}
