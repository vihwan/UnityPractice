using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    //현실세계의 1초와 게임 세계의 1초는 달라야한다.
    [SerializeField] private float secondPerRealTimeSecond; // 게임세계의 100초 = 현실 세계의 1초
    [SerializeField] private float fogDensityDeltaTime;     //시간에 따른 안개밀도 증감량 forDensityDeltatime* 100  = secondPerRealTimeSecond
    [SerializeField] private float nightFogDensity;         //밤에 얼마나 짙게 깔리는지
    private float dayFogDensity;                            //낮 상태의 fog 밀도
    private float currentFogDensity;                        //현재 안개밀도

    //프로퍼티 : 캡슐화
    public float CurrentFogDensity { get => currentFogDensity; set => currentFogDensity = value; }

    private

    void Start()
    {
        fogDensityDeltaTime = secondPerRealTimeSecond * 0.01f;
        InitializeFogSetting();
    }

    private void InitializeFogSetting()
    {
        RenderSettings.fog = false;
        RenderSettings.fogDensity = 0f;
        CurrentFogDensity = 0f;
        dayFogDensity = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        //어느 방향으로 1초에 얼마만큼 꺾을 것인가
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        //태양의 3차원x축이 170이 넘어가면 밤이 된다.
        if (transform.eulerAngles.x >= 170)
        {
            GameManager.isNight = true;
            RenderSettings.fog = true;
        }
        else if (transform.eulerAngles.x >= 0)
        {
            //(10 미만 = 350이상)이 되면 낮이 된다
            GameManager.isNight = false;
        }

        if (GameManager.isNight && !GameManager.isWater) //밤 상태
        {
            //설정한 밤의 밀도보다 현재밀도가 작다면
            if (CurrentFogDensity <= nightFogDensity)
            {
                //밀도를 더해준다.
                CurrentFogDensity += 0.1f * fogDensityDeltaTime * Time.deltaTime;
                RenderSettings.fogDensity = CurrentFogDensity;
            }
        }
        else if(!GameManager.isNight && !GameManager.isWater) //낮 상태
        {
            //설정한 낮의 밀도보다 현재밀도 크다면
            if (CurrentFogDensity >= dayFogDensity)
            {
                //밀도를 빼서 밝게 해준다.
                CurrentFogDensity -= 0.1f * fogDensityDeltaTime * Time.deltaTime;
                RenderSettings.fogDensity = CurrentFogDensity;
                if (RenderSettings.fogDensity <= 0)
                {
                    RenderSettings.fog = false;
                }
            }
        }
    }
}
