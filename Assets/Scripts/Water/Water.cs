using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    //저항 변수
    [SerializeField] private float waterDrag; //물속 저항값
    private float originDrag; //원래 저항값

    //색상 및 Fog정도
    [SerializeField] private Color waterDayColor; //물속 색깔
    [SerializeField] private float waterDayFogDensity; // 물의 탁한 정도
    [SerializeField] private Color waterNightColor;
    [SerializeField] private float waterNightFogDensity;


    //원래대로 돌아갈 변수
    private Color originColor;
    private float originFogDensity;
    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightFogDensity;


    //private bool isInWater = false; //물속에 계속 들어가있는 상태인가?


    //사운드 변수
    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_WaterBreathe;

    //시간 변수
    [SerializeField] private float breatheTime;
    private float currentBreatheTime;


    //필요한 컴포넌트
    private Oxygen theOxygen;
    private StatusController theStatusController;
    [SerializeField] private GameObject go_BaseUI;


    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;

        theOxygen = FindObjectOfType<Oxygen>();
        theStatusController = FindObjectOfType<StatusController>();
        theOxygen.CurrentOxygen = theOxygen.TotalOxygen;
    }

    void Update()
    {
        if (GameManager.isWater)
        {
            RenderSettings.fog = true;
            currentBreatheTime += Time.deltaTime;
            if (currentBreatheTime > breatheTime)
            {
                SoundManager.instance.PlaySE(sound_WaterBreathe);
                currentBreatheTime = 0f;
            }

            if (GameManager.isNight)
            {
                RenderSettings.fogColor = waterNightColor;
                RenderSettings.fogDensity = waterNightFogDensity;
            }
            else
            {
                RenderSettings.fogColor = waterDayColor;
                RenderSettings.fogDensity = waterDayFogDensity;
            }
        }
        OxygenStatus();
    }

    private void OxygenStatus()
    {
        if (GameManager.isWater)
        {
            theOxygen.CurrentOxygen -= 10f * Time.deltaTime;
            theOxygen.Text_currentOxygen.text = Mathf.RoundToInt(theOxygen.CurrentOxygen).ToString() + " / 100";
            theOxygen.Image_gauge.fillAmount = theOxygen.CurrentOxygen / theOxygen.TotalOxygen;

            if (theOxygen.CurrentOxygen <= 0)
            {
                theOxygen.CurrentOxygen = 0;
                theStatusController.DecreaseHp(1);
            }
        }
        else
        {
            theOxygen.CurrentOxygen += 10f * Time.deltaTime;
            theOxygen.Text_currentOxygen.text = Mathf.RoundToInt(theOxygen.CurrentOxygen).ToString() + " / 100";
            theOxygen.Image_gauge.fillAmount = theOxygen.CurrentOxygen / theOxygen.TotalOxygen;

            if (theOxygen.CurrentOxygen >= theOxygen.TotalOxygen)
            {
                theOxygen.CurrentOxygen = theOxygen.TotalOxygen;
            }
        }
    }

    //물 (Water Plane)의 컬라이더와 충돌 여부
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GetOutWater(other);
        }
    }

    //물에 들어갔을 때
    private void GetWater(Collider _playerCollider)
    {
        if (!GameManager.isWater)
        {
            GameManager.isWater = true;
            SoundManager.instance.PlaySE(sound_WaterIn);
            go_BaseUI.SetActive(true);

            _playerCollider.transform.GetComponent<Rigidbody>().drag = waterDrag;

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = waterDayColor;
                RenderSettings.fogDensity = waterDayFogDensity;
            }
            else
            {
                RenderSettings.fogColor = waterNightColor;
                RenderSettings.fogDensity = waterNightFogDensity;
            }
        }    
    }

    //물에 나왔을 때
    private void GetOutWater(Collider _playerCollider)
    {
     //   isInWater = false;
        if (GameManager.isWater)
        {
            GameManager.isWater = false;
            SoundManager.instance.PlaySE(sound_WaterOut);
            _playerCollider.transform.GetComponent<Rigidbody>().drag = originDrag;
            go_BaseUI.SetActive(false);

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }
}
