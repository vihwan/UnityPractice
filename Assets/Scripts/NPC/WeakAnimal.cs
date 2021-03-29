using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    // Start is called before the first frame update
    public void Run(Vector3 _targetPos)
    {
        //돼지가 뛸 때는 데미지를 받았을 때
        //돼지의 방향을 플레이어의 반대방향으로 바라보게 한뒤 뛰게 만들것이다.
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        animator.SetBool("Running", isRunning);
        Debug.Log("도망(뛰기)");
    }

    public override void Damage(int _damage, Vector3 _targetPos)
    {
        base.Damage(_damage, _targetPos);
        if (!isDead)
        {
            Run(_targetPos); //데미지를 입으면 뛴다.
        }
    }

}
