using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleUIManager : MonoBehaviour
{
    public string SceneName = "GameScene"; //다음 씬으로 이동할 이름 : 게임 씬;
    public static TitleUIManager instance;

    public static string startMode;


    public enum StartMode {    
        NEWSTART,
        LOAD
    }

    private void Awake()
    {
        //싱글톤
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
            Destroy(this.gameObject);
    }


    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        LoadingSceneManager.isLoad = false;
    }

    public void ClickStart()
    {
        Debug.Log("게임 새로 시작");
        StartCoroutine(GameStartCoroutine(StartMode.NEWSTART.ToString()));
        startMode = StartMode.NEWSTART.ToString();
    }

    public void ClickLoad()
    {
        //다음 씬(월드)로 이동시킨뒤 세이브 데이터를 가져와야한다.
        //그렇지 않으면 순서상의 오류가 생겨 NULL값이 들어갈것이다.
        //동기화 시켜줘야한다.
        Debug.Log("데이터 로드 실행");
        StartCoroutine(GameStartCoroutine(StartMode.LOAD.ToString()));
        startMode = StartMode.LOAD.ToString();
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    private IEnumerator GameStartCoroutine(string _startMode)
    {
        LoadingSceneManager.SetLoadScene(SceneName); //로딩 씬을 실행
        yield return new WaitForSeconds(LoadingSceneManager.loadingTime + 1f); // 비동기 로딩 시간 + 1초만큼 대기
    }
}
