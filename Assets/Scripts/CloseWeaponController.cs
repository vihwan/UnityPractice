using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{ //미완성 메소드가 하나 있기 때문에 저절로 미완성 클래스가 된다.
    //미완성 : HitCoroutine();
    //abstract는 다른 클래스에서 직접적으로 추가시킬 수 없다.


    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    //공격중
    protected bool isAttack = false; //공격중인지
    protected bool isSwing = false; //팔을 휘두르고 있는지

    protected RaycastHit hitInfo;
    [SerializeField] protected LayerMask layerMask;

    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1"))
            {
                if (isAttack == true)
                {
                    //Debug.Log("근접 공격시도 실행");
                    //딜레이를 준다.
                    //공격 코루틴
                    StopAllCoroutines();
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        // Debug.Log("근접 공격 코루틴 실행");
        //isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA); //공격중 대기시간
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB); //데미지 대기시간
        isSwing = false;

        //총 대기시간 - 공격중 대기시간 - 데미지 대기시간
        //총 attackDelay 만큼 딱 대기해야한다.
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }


    //자식 클래스가 기능을 완성시켜야한다.
    //abstract : 자식에서 기능을 완성시켜라.
    //추상 코루틴
    protected abstract IEnumerator HitCoroutine();


    //공격시 대상 체크
    protected bool CheckObject()
    {
        //지정된 위치에서 앞을 향해 빛을 쏴서 범위 대에 대상이 있는가?
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        return true;
    }

    //가상함수 : 완성함수 이지만 추가 편집이 가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        this.currentCloseWeapon = closeWeapon;
        WeaponManager.currentWeapon = this.currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = this.currentCloseWeapon.anim;
        //  Debug.Log("무기 교체 컴포넌트 실행");

        //무기 교체시 위치가 바뀔수 있기에 초기화
        this.currentCloseWeapon.transform.localPosition = Vector3.zero;
        this.currentCloseWeapon.gameObject.SetActive(true);
    }
}
