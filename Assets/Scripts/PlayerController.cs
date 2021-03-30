using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    //보호수준은 유지되면서 인스펙터 창에서 수정할 수 있다.
    //물론 예외도 있다..
    //SerializeField는 GetComponent보다 속도가 느려서 유니티에서 권장하지 않는다
    [SerializeField]
    private float walkSpeed; //플레이어 속도
    [SerializeField]
    private float runSpeed; //달리기 속도
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed; //달리거나 걷거나 하는 것을 함수를 따로 만들지 않고 적용시키도록 만든 공간

    [SerializeField]
    private float jumpForce;

    //상태변수
    public bool isWalk = false;
    public bool isRun = false;
    private bool isGround;
    private bool isCrouch = false;

    //앉았을 때 얼마나 앉을 지 결정하는 변수
    [SerializeField]
    private float crouchPosY; //앉았을때 카메라 위치
    private float originPosY; //원래 서있을 때 카메라 위치
    private float applyCrouchPosY; //위의 변수들을 적용시킬 


    //카메라 민감도
    [SerializeField]
    private float lookSensitivity; // 카메라의 민감도. 자신에 맞게 조절할 수 있도록

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit; // 플레이어의 시야 각도제한을 위한 제한설정
    private float currentCameraRotationX = 0f; //카메라가 바라보는 각도 설정. 정면을 바라보게 0
    //어차피 0으로 초기화 되긴 한다.


    //움직임 체크 변수
    private Vector3 lastPos;


    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid; // 플레이어의 실질적인 몸
    //RigidBody는
    //Collider에 물리학을 입히는 역할
    private GunController theGunController;
    private CrossHair theCrossHair;
    private StatusController theStatusController;


    private CapsuleCollider capsuleCollider; //땅 착지 여부를 위한 컴포넌트

    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    private CloseWeapon currentCloseWeapon;



    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType은 자신의 Hierachy를 전부 뒤져서 해당하는 컴포턴트를 찾는다.
        //근데 카메라가 한개가 아니라 여러개일수 있기 때문에 외부 에디터에서 직접 지정해주는게 좋다.
        // theCamera = FindObjectOfType<Camera>();
        //근데 이미 SerializeField로 선언해서 그냥 외부에디터에서 넣는 방법도 좋다..

        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //Rigidbody 컴포넌트를 myRigid에 넣는다는 뜻이다.
        applySpeed = walkSpeed;
        //카메라의 상대적인 기준을 해야하기 때문에 localPostiion을 사용한다.
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;

        //하이어라키를 싹다 뒤져서 GunController를 찾는다.
        theGunController = FindObjectOfType<GunController>();
        theCrossHair = FindObjectOfType<CrossHair>();
        theStatusController = FindObjectOfType<StatusController>();
    }

    // Update is called once per frame
    //1초에 대략 60번 실행되는 함수
    void Update()
    {
        IsGround();
        TryJump();
        TryRun(); //걷는지 뛰는지 판단하는 메소드. 반드시 Move 위에 있어야함
        TryCrouch();
        Move();
        MoveCheck();
        if (!Inventory.inventoryActivated)
        {
            CharacterRotation();
            CameraRotation();
        }
    }

   

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrossHair.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    //병렬 처리를 지원해주는 함수 : 코루틴
    //부드러운 카메라 이동 처리를 위한 함수
    IEnumerator CrouchCoroutine()
    {
        float posY = theCamera.transform.localPosition.y;
        int count = 0; //보간의 범위를 지정하기 위해 반복문을 카운트
        while (posY != applyCrouchPosY)//원하는 속도에 도달할 때 까지 반복
        {
            count++;
            //보간함수를 이용해서 로그함수같은 증가율을 만들도록 하자.
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            //localPostion은 벡터타입이기에 넣을때도 벡터로 넣어야함
            theCamera.transform.localPosition = new Vector3(0,posY,0);

            //보간함수의 단점은 딱 정수로 떨어지지 않기 때문에
            //계산 횟수를 제한한다.
            if(count > 15)
                break;
            yield return null; //1프레임 대기
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }



    //지면 체크
    private void IsGround()
    {
        //항상 레이저는 캡슐 기준이 아닌 절대적으로 아래로 쏴야하기 때문에 Vector.down를 쓴다.
        //bounds : Collider의 영역 / entents : bounds의 절반
        //정확히 반사이즈만 주게되면 계단을 오를때 부자연스러울 수 있기에 +0.1f 오차를 넣는다.

        //빛을 쏠때.. 고려해야할점
        //어디에서, 어느 방향으로, 얼만큼, (누굴 맞췄는지) 등등

        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.3f);
        theCrossHair.JumpingAnimation(!isGround);
        
    }

    //점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround
            && theStatusController.GetCurrentSP() > 0) //스태미너가 0이상인가?
        {
            Jump();
        }
    }

    //점프
    private void Jump()
    {
        //Debug.Log("점프한다!");
        if (isCrouch == true)
            Crouch();
        theStatusController.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;   
    }

    //달리는지 걷는지 판단하는 메소드
    private void TryRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) //왼쪽 시프트키를 꾹 누를 경우
            && theStatusController.GetCurrentSP() > 0) //스태미너가 0 이상일 경우에만
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) //왼쪽 시프트키를 뗄 경우
            || theStatusController.GetCurrentSP() <= 0) //스태미너가 0이하가 되면
        {
            RunningCancel();
        }
    }

    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrossHair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    //달리게 하는 함수
    private void Running()
    {
        if (isCrouch == true)
            Crouch();
        isWalk = false;
        isRun = true;
        theGunController.CancelFineSight();      
        theCrossHair.RunningAnimation(isRun);
        theStatusController.DecreaseStamina(10);
        applySpeed = runSpeed;
    }

    //좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float yRotation = Input.GetAxis("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        //우리가 구한 벡터값(오일러값)을 쿼터니언으로 변환시켜 곱해진 값을 리지드에 할당
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
        // Debug.Log(myRigid.rotation);//4원소
        // Debug.Log(myRigid.rotation.eulerAngles);//3원소
    }

    //상하 카메라 회전
    private void CameraRotation()
    {

        //마우스는 3차원이 아니기 때문. 마우스는 X랑 Y밖에 없다.
        //위아래로 바뀌는 변수
        //1과 -1씩 대입되기 때문에 lookSensitivity 감도를 조절한다.
        float xRotation = Input.GetAxis("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;

        //위로 올리면 위로 가게끔 -로 연산한다.
        currentCameraRotationX -= cameraRotationX; //흔히 fps의 마우스 반전과 관련이 있다.
        //범위 가두기 함수 Clamp
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        //카메라의 위치정보
        //localEulerAngle : 로테이션 x,y,z라고 생각하면 된다.
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

       // Debug.Log(myRigid.rotation.eulerAngles);//3원소
    }

    //움직임 실행
    private void Move()
    {
        //가로 입력을 받아 변수에게 값을 대입
        //Horizontal은 이미 유니티에서 설정되어있음
        float moveDirectionX = Input.GetAxisRaw("Horizontal");

        //유니티 3D공간에서는 Z가 정면과 뒤이기 때문에 Vertical을 쓴다.
        float moveDirectionZ = Input.GetAxisRaw("Vertical");

        //벡터는 크기 * 방향으로 이루어져야 한다.
        Vector3 moveHorizontal = transform.right * moveDirectionX; // (1,0,0) * 
        Vector3 moveVertical = transform.forward * moveDirectionZ; // (0,0,1) * 

        //벡터의 합
        //normalize는 벡터를 단위함수로 만든다.유니티에서 권장하기도 하고..
        //1초에 얼마나 이동시킬것인지 계산하기 쉬워진다.
        //거기에 속력을 곱하면 속도가 되는 것이지.
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        //Time.deltaTime
        //Update 함수에 맞게 시간을 쪼개서 설정
        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {

        //걷는 조건 총 3가지
        //1. 땅에 닿아 있는가?
        //2. 엎드린 상태가 아닌가?
        //3. 달리는 상태가 아닌가?
        if (isGround && !isCrouch  && !isRun)
        {
            //전프레임 플레이어 위치와 현재 위치와 다르면
            //위치 변동이 된것이기 때문에 값을 현재것으로 변경
            if (Vector3.Distance(lastPos,transform.position) >= 0.01)
                //경사면에서 살짝 미끄러져도 걷는다고 판단하지 않도록 위처럼 설계
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }
            theCrossHair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }       
}
