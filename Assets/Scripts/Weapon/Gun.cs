﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; //총 이름
    public float range; // 사정거리
    public float accuracy; // 정확도
    public float fireRate; // 연사속도
    public float reloadTime; //재장전 속도 

    public int damage; // 총의 데미지

    public int reloadBulletCount; //총알 재장전 갯수
    public int currentBulletCount; //현재 탄창에 남아있는 총알 갯수
    public int maxBulletCount; // 최대 소유 가능한 총알 갯수
    public int carryBulletCount; // 현재 소유하고 있는 총알 갯수

    public float retroActionForce; // 반동 세기
    public float retroActioFineSigntForce; // 정조준시의 반동 제거

    public Vector3 fineSightOriginPos;

    public Animator anim;

    public ParticleSystem muzzleFlash;

    public AudioClip fireSound; //총 발사 소리  
}
