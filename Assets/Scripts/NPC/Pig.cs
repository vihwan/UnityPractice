using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    //특징 변수
    [SerializeField] private string animalName;
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;

    //상태변수
    private bool isWalking; // 걷는지 안 걷는지 판별
    private bool isAction; //행동중인지 아닌지 판별

    //세부 능력변화 변수
    [SerializeField] private float walkTime;
    [SerializeField] private float waitTime;
    private float currentTime;

    // 필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

}
