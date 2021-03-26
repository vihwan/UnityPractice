using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    //필요하면 HUD 호출, 필요 없으면 비활성화
    [SerializeField]
    private GameObject go_BulletHUD;

    //총알 갯수 텍스트
    [SerializeField]
    private Text[] text_Bullet;


    // Update is called once per frame
    void Update()
    {
        CheckBullet();




    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        //텍스트는 항상 스트링으로 받는다.
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = "/";
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
