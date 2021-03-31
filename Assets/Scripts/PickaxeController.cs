using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    //활성화 여부
    public static bool isActivate = true;


    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    private void Update()
    {
        if (isActivate == true)
        {
            // Debug.Log("PickAxe isActivate");
            isAttack = true;
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
                if (hitInfo.transform.CompareTag("Rock"))
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.CompareTag("WeakAnimal"))
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }
/*                else if (hitInfo.transform.tag == "StringAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }*/
                Debug.Log(hitInfo.transform.name);
                isSwing = false;

            }
            yield return null;
        }
    }

}
