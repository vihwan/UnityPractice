using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]  //에디터에서 보여주게 하기 위해 꼭 붙여야함
public class Craft
{
    public string craftName; //이름
    public GameObject go_Prefab; //실제 설치될 프리팹
    public GameObject go_PreviewPrefab; //미리보기 프리팹
}

public class CraftManual : MonoBehaviour
{
    //상태변수
    public static bool isActivated = false;         //크래프트 UI가 활성화되어 있는가?
    public static bool isCrafting = false;  //미리보기가 생성되어있는가?             //

    private GameObject go_Preview; //미리보기 프리팹을 담을 변수
    private GameObject go_Prefab; //실제 생성될 프리팹

    //필요한 컴포넌트
    [SerializeField]
    private GameObject go_BaseUI; //기본베이스 UI

    [SerializeField]
    private Craft[] craftsArray; //모닥불용 탭

    [SerializeField]
    private Transform tf_Player;
    [SerializeField]
    private Text craftName; //슬롯 내 크래프트 이름
    [SerializeField]
    private Text craftDescription; //슬롯 내 크래프트 설명
    [SerializeField]
    private Image craftImage; //슬롯 내 크래프트 이미지 스프라이트

    //Raycast 필요 변수 선언
    private RaycastHit hitinfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float rayRange;

    private static int craftsArrayNum = 0;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isCrafting)
            Window();

        if(isActivated || isCrafting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Cancel();

            if (Input.GetButtonDown("Fire1"))
                Build();
        }
        if (isCrafting)
            PreViewPostionUpdate();
    }

    //탭의 메뉴를 클릭할 때
    public void TabClick(Crafts _crafts)
    {
        //탭을 누를 때 마다, 그 크래프트의 정보를 가져와서 
        //슬롯에 보여지는 크래프트의 이미지, 이름, 설명, 프리팹 정보를 해당 크래프트에 맞게 바꾼다.
        craftName.text = _crafts.craftName;
        craftDescription.text = _crafts.craftDescription;
        craftImage.sprite = _crafts.craftImage;

        if (craftName.text == "FireCamp")
        {
            craftsArrayNum = 0; //FireCamp;
            return;
        }
        else if (craftName.text == "Stair")
        {
            craftsArrayNum = 1; //Stair;
            return;
        }
    }


    //슬롯 내용을 클릭할 때
    public void SlotClick()
    {
        go_Preview = Instantiate(craftsArray[craftsArrayNum].go_PreviewPrefab
                        , tf_Player.position + tf_Player.forward
                        , Quaternion.identity); ;
        go_Prefab = craftsArray[craftsArrayNum].go_Prefab;

        GameManager.isOpenCraftManual = false;
        GunController.isGunAttack = false;
        CloseWeaponController.canAttack = false;
        isCrafting = true;
        isActivated = false;
        go_BaseUI.SetActive(false);
    }


    //미리보기 프리팹에서 왼클릭하여 빌드할 때
    private void Build()
    {
        if (isCrafting && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(go_Prefab, hitinfo.point, Quaternion.identity);
            Destroy(go_Preview);
            go_Preview = null;
            go_Prefab = null;
            isActivated = false;
            isCrafting = false;

            StartCoroutine(IsActivateChangeTime());
        }
    }

    //건축물 설치와 동시에 바로 공격이 나가지 않도록 대기 시간을 걸어준다.
    private IEnumerator IsActivateChangeTime()
    {
        yield return new WaitForSeconds(1f);
        GunController.isGunAttack = true;
        CloseWeaponController.canAttack = true;
    }

    //미리보기 프리팹의 위치를 실시간으로 플레이어 초점으로 이동시킨다.
    private void PreViewPostionUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitinfo, rayRange, layerMask))
        {
            if (hitinfo.transform != null)
            {
                Vector3 _location = hitinfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }



    //빌드 과정을 취소한다면 프리팹을 파괴시키고 모든 변수를 초기화시킨다.
    private void Cancel()
    {
        if (isCrafting)
            Destroy(go_Preview);

        isActivated = false;
        isCrafting = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);
    }


    //UI가 작동할 때 창을 열고 닫는다.
    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()
    {
        GameManager.isOpenCraftManual = true;
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        GameManager.isOpenCraftManual = false;
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
}
