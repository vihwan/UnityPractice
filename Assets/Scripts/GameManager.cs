using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;      //플레이어 움직임 제어
    public static bool isOpenInventory = false;   //인벤토리 창
    public static bool isOpenCraftManual = false; //건축 메뉴얼 창

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false;

    public static bool isPreviewCanvasActivated = true;

    private WeaponManager theWeaponManager;
    private SaveAndLoad theSaveAndLoad;
    [SerializeField]
    private Canvas thePreviewCanvas;
    [SerializeField]
    private Text theGameLoadSuccessText;
    private TitleUIManager titleUIManager;

    private bool isWaterflag = false;


    private void Start()
    {
        theWeaponManager = FindObjectOfType<WeaponManager>();

        if (LoadingSceneManager.isLoad)
        {
            titleUIManager = FindObjectOfType<TitleUIManager>();
            titleUIManager.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            Invoke(nameof(StartORLoadData), 2.0f); //데이터 로드 2초간 대기
        }     
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPause || isPreviewCanvasActivated)
        {
            Cursor.lockState = CursorLockMode.None; //커서 화면 가두기 해제
            Cursor.visible = true;
            canPlayerMove = false;
            ActionController.pickupActivated = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; //커서 화면 가두기
            Cursor.visible = false;                   //커서 비활성화
            canPlayerMove = true;
            ActionController.pickupActivated = true;
        }

        if (isWater)
        {
            if (!isWaterflag)
            {
                StopAllCoroutines();
                StartCoroutine(theWeaponManager.WeaponInCoroutine());
                isWaterflag = true;
            }
        }
        else
        {
            if (isWaterflag)
            {
                theWeaponManager.WeaponOut();
                isWaterflag = false;
            }
        }
    }

    public void SetActiveFalsePreviewCanvas()
    {
        isPreviewCanvasActivated = false;
        thePreviewCanvas.gameObject.SetActive(false);
    }

    public void StartORLoadData()
    {
        if (LoadingSceneManager.isLoad)
        {
            SetActiveFalsePreviewCanvas();  //GameScene에 있는 프리뷰대기 캔버스를 비활성화
            IsDataLoad(TitleUIManager.startMode);
            LoadingSceneManager.isLoad = false;
        }
        else
        {
            Debug.Log("로딩이 제대로 이루어지지 않았습니다.");
        }
    }

    private void IsDataLoad(string _startMode)
    {
        switch (_startMode)
        {
            case "NEWSTART":
                Debug.Log("게임 시작 성공");
                Invoke(nameof(SetActiveFalseGameLoadText), 3f);
                break;

            case "LOAD":
                //'GameScene' 내에 존재하는 SaveAndLoad 찾기
                theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
                theSaveAndLoad.LoadData();
                Debug.Log("데이터 로드 성공");
                theGameLoadSuccessText.text = "데이터 로드를 성공하였습니다.";
                Invoke(nameof(SetActiveFalseGameLoadText), 3f);
                break;
        }
    }

    private void SetActiveFalseGameLoadText()
    {
        theGameLoadSuccessText.gameObject.SetActive(false);
    }

}
