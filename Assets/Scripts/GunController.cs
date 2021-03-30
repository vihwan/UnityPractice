using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    //활성화 여부
    public static bool isActivate = false;


    //현재 장착된 총
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate; //연사속도 조절

    //상태변수
    private bool isReload = false; 
    [HideInInspector]
    public bool isFineSightMode = false;

    // 조준 본래 포지션값
    private Vector3 originPos;
    // 효과음 재생
    private AudioSource audioSource;
    [SerializeField]
    private Camera theCam; //정조준 카메라 정보
    private CrossHair theCrosshair;

    //충돌 정보 받아오는 레이저
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;


    [SerializeField]
    private GameObject hitEffect_Prefab; //타격 피격 이펙트
    private PlayerController thePlayerController;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originPos = Vector3.zero;
        theCrosshair = FindObjectOfType<CrossHair>();
        thePlayerController = FindObjectOfType<PlayerController>();

/*        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;*/
    }


    private void Update()
    {
        if (isActivate)
        {
            GunFireRateCalculate();
            if (!Inventory.inventoryActivated)
            {
                TryFire();
                TryReload();
                TryFineSight();
                //RunningWithGun();
            }
        }
    }

/*    private void RunningWithGun()
    {
        if (thePlayerController.isWalk)
        {
            currentGun.anim.SetBool("Walk", thePlayerController.isWalk);
        }
    }*/

    //조준 시도
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && isReload == false)
        {
            FineSight();
        }
        
      
    }

    //조준 취소
    public void CancelFineSight()
    {
        if(isFineSightMode == true)
        {
            FineSight();
        }
    }


    //조준 실행
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightNon_ActivateCoroutine());
        }
    }

    //정조준 활성화
    IEnumerator FineSightActivateCoroutine()
    {
        Debug.Log("정조준 코루틴 실행");
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition,
                currentGun.fineSightOriginPos,0.2f);

            yield return null;
        }
    }

    //정조준 비활성화
    IEnumerator FineSightNon_ActivateCoroutine()
    {
        Debug.Log("정조준 코루틴 해제");
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition,
                originPos, 0.2f);

            yield return null;
        }
    }


    //리로드 시도
    private void TryReload()
    {

        if(Input.GetKeyDown(KeyCode.R) && isReload == false 
            && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    //발사 시도
    private void TryFire()
    {
        if(thePlayerController.isRun == false)
        {
            //Fire 입력과 연사속도가 0이하로 발사할 준비가 되었다면
            if (Input.GetButton("Fire1") && currentFireRate <= 0 && isReload == false)
            {
                Fire();
            }
        }
    }

    //발사전
    private void Fire()
    {
        if(isReload == false)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();

            }
            else
            {
                CancelFineSight();
                //코루틴으로
                StartCoroutine(ReloadCoroutine());
            }
        }         
    }


    //리로드 실행
    IEnumerator ReloadCoroutine()
    {
        Debug.Log("리로드 코루틴 실행");
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }


    public void CancelReload()
    {
        if (!isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }


    //발사후
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;

        currentFireRate = currentGun.fireRate; //연사속도 재계산
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Hit();
        //총기반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

       
    }

    //총알을 맞출 때
    private void Hit()
    {
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0)
                        , out hitInfo, currentGun.range, layerMask))
        {
            var Clone = Instantiate(hitEffect_Prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(Clone,2f); //2초후 클론 파괴
            Debug.Log(hitInfo.transform.name);
        }
    }


    //반동 실행
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y,originPos.z);// 평상시의 최대 반동
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActioFineSigntForce,currentGun.fineSightOriginPos.y,currentGun.fineSightOriginPos.z); // 정조준 했을때 최대 반동

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;
            //반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;  
            }

        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;
            //반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActioFineSigntForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    //연사속도가 0이 되면 다음 총알을 발사할 수 있는 상태가 되도록
    private void GunFireRateCalculate()
    {
       if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; //60분의 1
            //이를 update로 돌리니 1초에 1 감소
        }
    }

    //효과음 실행
    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentGun = gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;


        //무기 교체시 위치가 바뀔수 있기에 초기화
        currentGun.transform.localPosition = Vector3.zero; 
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
