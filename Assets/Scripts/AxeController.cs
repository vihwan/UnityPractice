using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{

    //활성화 여부
    public static bool isActivate = false;


    private void Start()
    {
/*        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;*/
    }

    private void Update()
    {
        if (isActivate)
        {

            TryAttack();
        }
    }


    public override void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        base.CloseWeaponChange(closeWeapon);
        isActivate = true;
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                Debug.Log("도끼 히트 코루틴 실행");
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }


}
