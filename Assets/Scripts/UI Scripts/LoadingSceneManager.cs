﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextscene;

    [SerializeField]
    private Slider theLoadingSlider;
    public static bool isLoad = false;

    public static float loadingTime = 0.0f;

    private void Start()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    public static void SetLoadScene(string _sceneName) // 로딩씬을 실행시키는 함수
    {
        nextscene = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private IEnumerator LoadSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextscene); //다음 씬을 비동기식 동작
        operation.allowSceneActivation = false; //씬 동작을 비활성화

        float timer = 0.0f;
        while (!operation.isDone)
        {
            yield return null;

            loadingTime += Time.deltaTime;
            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                theLoadingSlider.value = Mathf.Lerp(theLoadingSlider.value, operation.progress, timer);
                if (theLoadingSlider.value >= operation.progress) //즉 진행상황이 0.9이상이 되면
                {
                    timer = 0f; //타이머를 초기화
                }
            }
            else
            {
                theLoadingSlider.value = Mathf.Lerp(theLoadingSlider.value, 1f, timer);
                if (theLoadingSlider.value == 1.0f)  //만약 로딩바가 100%가 되면
                {
                    isLoad = true;
                    operation.allowSceneActivation = true; //씬 동작을 활성화
                    loadingTime = 0.0f;
                    yield break;
                }
            }
        }
    }
}
