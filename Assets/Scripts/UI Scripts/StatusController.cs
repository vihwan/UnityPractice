using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //체력
    [SerializeField]
    private int hp;
    private int currentHp;

    //스태미너
    [SerializeField]
    private int sp;
    private int currentSp;

    [SerializeField]
    private int spIncreaseSpeed;

    //스태미너 재회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;


    //스태미너 감소 여부
    private bool spUsed = false;

    //방어력
    [SerializeField]
    private int dp;
    private int currentDp;

    //배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    [SerializeField]
    private int hungryDecreaseTime; //배고픔이 감소하는 시간 설정
    private int currentHungryDecreaseTime; //배고픔 수치변환 대기 시간

    //목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;


    //만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    //필요한 이미지
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3,
        THIRSTY = 4, SATISFY = 5;



    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        GaugeUpdate();
        SPRecharageTime();
        SPRecover();
    }

    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp; //비율의 정도로 따라 이미지를 채운다.
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }


    public void IncreaseHp(int _count)
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }


    public void DecreaseHp(int _count)
    {
        currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("캐릭터의 HP가 0이 되었습니다."); //실제로는 게임 오버

    }

    public void IncreaseSp(int _count)
    {
        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;
    }

    public void DecreaseSp(int _count)
    {
        currentSp -= _count;
    }



    public void IncreaseDp(int _count)
    {
        if (currentDp > 0)
        {
            DecreaseDp(_count);
            return;
        }
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    public void DecreaseDp(int _count)
    {
        currentDp -= _count;
        if (currentDp <= 0)
            Debug.Log("캐릭터의 DP가 0이 되었습니다."); //실제로는 게임 오버
    }

    public void IncreaseHungry(int _count)
    {
 
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)
    {
        if(currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            //사용자가 설정한 배고픔 대기 시간을 넘어서면 현재 배고픔을 깎는다.
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++; //시간 게이지 증가
            }
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0; // 다시 0으로 초기화
            }
        }
        else Debug.Log("목마름 수치가 0이 되었습니다.");
    }

    private void Hungry()
    {
        if (currentHungry > 0)
        {
            //사용자가 설정한 배고픔 대기 시간을 넘어서면 현재 배고픔을 깎는다.
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
            {
                currentHungryDecreaseTime++; //시간 게이지 증가
            }
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0; // 다시 0으로 초기화
            }
        }
        else Debug.Log("배고픔 수치가 0이 되었습니다.");
    }


    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }

    private void SPRecharageTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }

    private void SPRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }




}
