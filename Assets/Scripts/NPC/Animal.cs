using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    //특징 변수
    [SerializeField] protected string animalName; //동물의 이름
    [SerializeField] protected int hp;          //동물의 체력
    [SerializeField] protected float walkSpeed; //걷는 속도
    [SerializeField] protected float runSpeed; //달리는 속도
    [SerializeField] protected float turningSpeed; //회전 속도
    [SerializeField] protected float meatMAXCount; //고기 최대 생성 갯수

    protected float applySpeed; // 각 상황마다 속도를 다르게 할 수 있도록 저장하는 변수

    protected Vector3 direction; //방향


    //상태변수
    protected bool isWalking; // 걷는지 안 걷는지 판별
    protected bool isRunning; //뛰는중인지 판별
    protected bool isAction; //행동중인지 아닌지 판별
    protected bool isDead; //죽었는지 판별

    //세부 능력변화 변수
    [SerializeField] protected float walkTime; //걷기 시간
    [SerializeField] protected float waitTime; //대기 시간
    [SerializeField] protected float runTime; //뛰기 시간
    [SerializeField] protected float destroyTime; // 동물 죽은 이후 사라지는 시간
    protected float currentTime;

    // 필요한 컴포넌트
    [SerializeField] protected GameObject go_Animal;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCollider;
    protected AudioSource theAudio;
    [SerializeField] protected AudioClip[] soundNormal;
    [SerializeField] protected AudioClip SoundHurt;
    [SerializeField] protected AudioClip SoundDead;

    [SerializeField] private GameObject go_MeatRawItem_Prefab; //날고기아이템 프리팹


    
    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    private void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            //회전이 자연스럽도록 Lerp함수 사용

            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
    }

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                ResetWalking();
                //다음 랜덤 액션 개시
            }
        }
    }

    protected virtual void ResetWalking()
    {
        isWalking = false; isAction = true; isRunning = false;
        applySpeed = walkSpeed;
        animator.SetBool("Walking", isWalking); animator.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }

    protected void TryWalk()
    {
        isWalking = true;
        animator.SetBool("Walking", isWalking);
        currentTime = waitTime;
        applySpeed = walkSpeed;
        Debug.Log("돼지 : 걷기");
    }


    //돼지가 데미지를 입을 때 실행됨
    public virtual void Damage(int _damage, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _damage;

            if (hp <= 0)
            {
                Dead();
                return;
            }
            PlaySE(SoundHurt);
            animator.SetTrigger("Hurt");
        }
    }

    protected void RandomNormalSound()
    {
        int _random = Random.Range(0, 3); //일상 사운드 3개
        PlaySE(soundNormal[_random]);
    }


    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    protected void Dead()
    {
        PlaySE(SoundDead);
        isWalking = false; isRunning = false; isDead = true;
        animator.SetTrigger("Dead");

        //돼지가 죽은 자리에 고기 아이템이 생성되도록 설정
        for (int i = 0; i < Mathf.Round(Random.Range(1, meatMAXCount)); i++)
        {
            Instantiate(go_MeatRawItem_Prefab, transform.position + transform.forward, Quaternion.identity);
            Debug.Log("고기 생성");
        }

        StartCoroutine(SetAnimalActiveFalse());
    }

    //돼지가 죽으면 일정 시간 뒤에 비활성화를 하도록 설정
    protected IEnumerator SetAnimalActiveFalse()
    {
        yield return new WaitForSeconds(destroyTime);
        go_Animal.SetActive(false);
    }
}