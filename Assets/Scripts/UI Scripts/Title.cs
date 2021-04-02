using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Title : MonoBehaviour
{
    public string SceneName = "GameScene"; //다음 씬으로 이동할 이름;


    public static Title instance;

    private SaveAndLoad theSaveAndLoad;

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
        Debug.Log("로딩");
        SceneManager.LoadScene(SceneName);
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }


    public void ClickLoad()
    {
        //다음 씬(월드)로 이동시킨뒤 세이브 데이터를 가져와야한다.
        //근데...순서상의 오류가 생겨 NULL값이 들어갈것이다.
        //동기화 시켜줘야한다.
        StartCoroutine(LoadCoroutine()); 
        Debug.Log("로드");
    }

    private IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);

        while (!operation.isDone)
        {
            yield return null;
            //operation.process를 이용해서 여기다 로딩화면을 구현하면 된다.
        }
        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
        theSaveAndLoad.LoadData();
        this.gameObject.SetActive(false);
    }
}
