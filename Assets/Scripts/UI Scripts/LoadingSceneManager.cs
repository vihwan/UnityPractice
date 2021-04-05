using System;
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

    private Title title;

    public static bool isLoad = false;

    public static float loadingTime = 0.0f;
    public static AsyncOperation operation;

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
        operation = SceneManager.LoadSceneAsync(nextscene); //다음 씬을 비동기식 동작
        operation.allowSceneActivation = false; //씬 동작을 비활성화

        float timer = 0.0f;
        while (!operation.isDone)
        {
            yield return null;

            loadingTime += Time.deltaTime;
            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                theLoadingSlider.value = Mathf.Lerp(theLoadingSlider.value, operation.progress, loadingTime);
                if (theLoadingSlider.value >= operation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                theLoadingSlider.value = Mathf.Lerp(theLoadingSlider.value, 1f, loadingTime);
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
