using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    //특징 변수
    [SerializeField] protected string animalName; //동물의 이름
    [SerializeField] protected int hp;          //동물의 체력
    [SerializeField] protected float walkSpeed; //걷는 속도
    [SerializeField] protected float runSpeed; //달리는 속도
    [SerializeField] protected float meatMAXCount; //고기 최대 생성 갯수

    protected Vector3 destination; //목적지

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
    [SerializeField] protected GameObject go_Animal;       //동물의 게임오브젝트
    [SerializeField] protected Animator animator;              //동물의 애니메이터
    [SerializeField] protected Rigidbody rigid;            //동물의 리지드바디
    [SerializeField] protected BoxCollider boxCollider;    //동물의 바디콜라이더
    protected AudioSource theAudio;                        //오디오소스
    [SerializeField] protected AudioClip[] soundNormal;    //오디오클립 : 평상시 
    [SerializeField] protected AudioClip SoundHurt;        //오디오클립 : 데미지 입을 때
    [SerializeField] protected AudioClip SoundDead;        //오디오클립 : 죽을 때
    protected NavMeshAgent nav;                            //동물의 NavMeshAgent

    /*
    NavMeshAgent 컴포넌트를 사용하면 RigidBody 컴포넌트가 잠기는 상태가 된다.
    즉, 작동이 되지 않기 때문에
    */

    [SerializeField] protected GameObject go_MeatRawItem_Prefab; //날고기아이템 프리팹


    
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    private void Update()
    {
        if (!isDead)
        {
            Move();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            nav.SetDestination(transform.position + destination * 5);
            // rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
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
        nav.speed = walkSpeed;
        nav.ResetPath(); //네브매시경로를 초기화
        animator.SetBool("Walking", isWalking); animator.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }

    protected void TryWalk()
    {
        isWalking = true;
        animator.SetBool("Walking", isWalking);
        currentTime = waitTime;
        nav.speed = walkSpeed;
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

        //슈퍼 몬스터면 아이템이 더 많이 생성되어 한꺼번에 터져나오도록 생성
        //돼지가 죽은 자리에 고기 아이템이 생성되도록 설정
        CreateItem(); 
        StartCoroutine(SetAnimalActiveFalse());
    }


    //사망시 아이템 생성
    protected virtual void CreateItem()
    {
        if (go_Animal.transform.CompareTag("WeakAnimal"))
        {
            for (int i = 0; i < Mathf.Round(Random.Range(1, meatMAXCount)); i++)
            {
                Instantiate(go_MeatRawItem_Prefab, transform.position + transform.up, Quaternion.identity);
                Debug.Log("고기 생성");
            }
        }
        else if (go_Animal.transform.CompareTag("WeakAnimalSuper"))
        {
            for (int i = 0; i < Mathf.Round(Random.Range(40, meatMAXCount)); i++)
            {
                //고기가 사방으로 생성되게끔 구현
                Instantiate(go_MeatRawItem_Prefab
                          , transform.position + new Vector3(Random.Range(-1f, 1f), 2f, Random.Range(-1f, 1f))
                          , Quaternion.identity);           
            }
        }
    }

    //돼지가 죽으면 일정 시간 뒤에 비활성화를 하도록 설정
    protected IEnumerator SetAnimalActiveFalse()
    {
        yield return new WaitForSeconds(destroyTime);
        go_Animal.SetActive(false);
    }
}