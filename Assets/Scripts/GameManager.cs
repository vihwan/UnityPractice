using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;      //플레이어 움직임 제어
    public static bool isOpenInventory = false;   //인벤토리 창
    public static bool isOpenCraftManual = false; //건축 메뉴얼 창

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false;

    private WeaponManager theWeaponManager;

    private bool flag = false;


    private void Start()
    {
        theWeaponManager = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            Cursor.lockState = CursorLockMode.None; //커서 화면 가두기 해제
            Cursor.visible = true;
            canPlayerMove = false;
            ActionController.pickupActivated = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; //커서 화면 가두기
            Cursor.visible = false;                   //커서 비활성화
            canPlayerMove = true;
            ActionController.pickupActivated = true;
        }

        if (isWater)
        {
            if (!flag)
            {
                StopAllCoroutines();
                StartCoroutine(theWeaponManager.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                theWeaponManager.WeaponOut();
                flag = false;
            }
        }
    }
}
