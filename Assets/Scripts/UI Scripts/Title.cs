using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Title : MonoBehaviour
{
    public string SceneName = "GameScene"; //다음 씬으로 이동할 이름;
    public static Title instance;

    private SaveAndLoad theSaveAndLoad;

    public enum StartMode {    
        NEWSTART,
        LOAD
    }

    private void Awake()
    {
        //싱글턴
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
            Destroy(this.gameObject);
    }


    public void ClickStart()
    {
        Debug.Log("게임 시작");

        StartCoroutine(GameStartCoroutine(StartMode.NEWSTART.ToString()));
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }


    public void ClickLoad()
    {
        //다음 씬(월드)로 이동시킨뒤 세이브 데이터를 가져와야한다.
        //그렇지 않으면 순서상의 오류가 생겨 NULL값이 들어갈것이다.
        //동기화 시켜줘야한다.
        Debug.Log("데이터 로드 실행");
        StartCoroutine(GameStartCoroutine(StartMode.LOAD.ToString())); 
       
    }

    private IEnumerator GameStartCoroutine(string _startMode)
    {
        LoadingSceneManager.SetLoadScene(SceneName); //로딩 씬을 실행

        yield return new WaitForSeconds(LoadingSceneManager.loadingTime + 1f); // 비동기 로딩 시간만큼 대기

        if (LoadingSceneManager.isLoad)
        {
            gameObject.SetActive(false);                //DontDestroyOnLoad에 있는 기본 캔버스를 비활성화
            GameManager theGameManager = FindObjectOfType<GameManager>();
            theGameManager.PreviewCanvasDeActivated();  //게임 씬에 있는 프리뷰대기 캔버스를 비활성화
            IsDataLoad(_startMode);        
        }
    }

    private void IsDataLoad(string _startMode)
    {
        switch (_startMode)
        {
            case "NEWSTART": 
                Debug.Log("게임 시작 성공");
                break;

            case "LOAD":
                theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
                theSaveAndLoad.LoadData();
                Debug.Log("데이터 로드 성공");
                break;
        }         
    }   
}
