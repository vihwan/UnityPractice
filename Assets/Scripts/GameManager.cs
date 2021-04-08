using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //플레이어의 상태변수
    public static bool canPlayerMove = true;      //플레이어 움직임 제어
    public static bool isWater = false;           //물 속에 있는가?
    private bool isWaterflag = false;             //→ 물 속에 있을 때 무기가 보이지 않게 해주는 추가 상태변수

    //환경 상태 변수
    public static bool isNight = false;           //밤인가?

    //시스템 상태 변수
    public static bool isPause = false;           //일시정지 상태
    public static bool isOpenInventory = false;   //인벤토리 창
    public static bool isOpenCraftManual = false; //건축 메뉴얼 창
    public static bool isPreviewCanvasActivated = true; // 게임 시작전 데이터를 불러오는 대기시간동안 보여주는 대기화면 창


    //필요한 컴포넌트
    private WeaponManager theWeaponManager;
    private SaveAndLoad theSaveAndLoad;
    [SerializeField] private Canvas thePreviewCanvas;
    [SerializeField] private Text theGameLoadSuccessText;
    private TitleUIManager titleUIManager;
    private DayAndNight theDayAndNight;


    private void Start()
    {
        theWeaponManager = FindObjectOfType<WeaponManager>();
        theDayAndNight = FindObjectOfType<DayAndNight>();

        if (LoadingSceneManager.isLoad) //비동기를 통해 씬의 로드가 완료가 되었다면
        {
            titleUIManager = FindObjectOfType<TitleUIManager>();
            titleUIManager.GetComponentInChildren<Canvas>().gameObject.SetActive(false); //Title 씬의 TitleUI Canvas를 비활성화
            Invoke(nameof(StartORLoadData), 2.0f); //데이터 로드 함수 2초간 대기
        }

        CloseWeaponController.canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {

        /* 다음 중 하나를 만족하면 
         * -> 플레이어는 움직일 수 없다. 
         * -> 아이템을 주울 수 없다.
         * 
         * 1.인벤토리가 열려있다
         * 2.건축메뉴얼 창이 열려있다
         * 3.일시정지 상태이다
         * 4.게임시작전 대기화면이 보이는 상태이다.
         * **/

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


        if (isWater) // 물 속에 있을 경우
        {
            if (!isWaterflag)
            {
                StopAllCoroutines(); //모든 코루틴 정지
                StartCoroutine(theWeaponManager.WeaponInCoroutine()); //무기 집어넣기 코루틴을 실행
                isWaterflag = true;
            }
        } 
        else //물 속이 아닐 경우
        {
            if (isWaterflag)
            {
                theWeaponManager.WeaponOut(); //무기 꺼내기
                isWaterflag = false;
            }
        }
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
                theGameLoadSuccessText.text = "데이터 로드를 성공하였습니다.\n 행운을 빕니다.";
                Invoke(nameof(SetActiveFalseGameLoadText), 3f);
                break;
        }
    }


    public void SetActiveFalsePreviewCanvas()
    {
        isPreviewCanvasActivated = false;
        thePreviewCanvas.gameObject.SetActive(false);
    }

    private void SetActiveFalseGameLoadText()
    {
        theGameLoadSuccessText.gameObject.SetActive(false);
    }
}
