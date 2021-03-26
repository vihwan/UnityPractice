using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{
    //활성화 여부
    public static bool isActivate = false;

    private void Start()
    {
        /*      WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
                WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;*/
    }


    private void Update()
    {
        if (isActivate)
        {
          //  Debug.Log("Hand isActivate");
            isAttack = true;
            TryAttack();
        }
    }



    public override void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        base.CloseWeaponChange(closeWeapon);
        isActivate = true;
        Debug.Log("Hand Change()");
        Debug.Log(isActivate);
    }


    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                Debug.Log("핸드 히트 코루틴 실행");
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
}
