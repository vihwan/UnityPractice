using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    //컨트롤러 활성화 여부
    public static bool isActivate = false;

    private void Start()
    {
/*        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;*/
    }

    private void Update()
    {
        if (isActivate)
            TryAttack();
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
                if (currentCloseWeapon.isAxe && hitInfo.transform.CompareTag("Tree")) //나무 베기!
                {
                    
                    hitInfo.transform.GetComponent<TreeComponent>().WoodCutting(hitInfo.point);
                }
                isSwing = false;
            }
            yield return null;
        }
    }
}
