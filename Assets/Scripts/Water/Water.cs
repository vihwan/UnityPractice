using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag; //물속 저항값
    private float originDrag; //원래 저항값

    [SerializeField] private Color waterColor; //물속 색깔
    [SerializeField] private float waterFogDensity; // 물의 탁한 정도

    [SerializeField] private Color waterNightColor;
    [SerializeField] private float waterNightFogDensity;
    
    
    //원래대로 돌아갈 변수
    private Color originColor;
    private float originFogDensity;

    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightFogDensity;

    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_WaterBreathe;

    [SerializeField] private float breatheTime;
    private float currentBreatheTime;

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;

    }

    void Update()
    {
        if (GameManager.isWater)
        {
            currentBreatheTime += Time.deltaTime;
            if(currentBreatheTime > breatheTime)
            {
                SoundManager.instance.PlaySE(sound_WaterBreathe);
                currentBreatheTime = 0f;
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
        GameManager.isWater = true;
        SoundManager.instance.PlaySE(sound_WaterIn);
       
        _playerCollider.transform.GetComponent<Rigidbody>().drag = waterDrag;


        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    //물에 나왔을 때
    private void GetOutWater(Collider _playerCollider)
    {
        GameManager.isWater = false;
        SoundManager.instance.PlaySE(sound_WaterOut);
        _playerCollider.transform.GetComponent<Rigidbody>().drag = originDrag;


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
