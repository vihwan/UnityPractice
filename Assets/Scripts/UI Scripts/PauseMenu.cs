using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private SaveAndLoad theSaveAndLoad;

    private TitleUIManager theTitleUIManager;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
                CallMenu();
            else
                CloseMenu();
        }
    }
    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        //시간의 흐름을 조절할 수 있는 함수. 
        Time.timeScale = 0f; //0배속
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1f; //1배속
    }

    public void ClickSave()
    {
        Debug.Log("세이브");
        theSaveAndLoad.SaveData();
    }

    public void ClickLoad()
    {
        theSaveAndLoad.LoadData();
        CloseMenu();
        Debug.Log("게임 내에서 로드");
    }
    public void ClickExit()
    {
        CloseMenu();
        theTitleUIManager = FindObjectOfType<TitleUIManager>();
        Destroy(theTitleUIManager.gameObject);
        LoadingSceneManager.SetLoadScene("GameTitle");
    }

}
