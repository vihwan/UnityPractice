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
    private bool isActivated = false;
    private bool isPreviewActivated = false;

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


    public void SlotClick()
    {
        go_Preview = Instantiate(craftsArray[craftsArrayNum].go_PreviewPrefab
                        , tf_Player.position + tf_Player.forward
                        , Quaternion.identity); ;
        go_Prefab = craftsArray[craftsArrayNum].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
            Window();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();

        if (Input.GetButtonDown("Fire1"))
            Build();

        if (isPreviewActivated)
            PreViewPostionUpdate();
    }

    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(go_Prefab, hitinfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }

    }

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

    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);
    }

    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }
    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }


    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }


}
