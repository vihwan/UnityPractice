using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pig : WeakAnimal
{
    protected override void ResetWalking()
    {
        base.ResetWalking();
        RandomAction();
    }


    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("돼지 : 대기");
    }
    private void Eat()
    {
        currentTime = waitTime;
        animator.SetTrigger("Eat");
        Debug.Log("돼지 : 풀뜯기");
    }
    private void Peek()
    {
        currentTime = waitTime;
        animator.SetTrigger("Peek");
        Debug.Log("돼지 : 두리번");
    }

    private void RandomAction()
    {
        isAction = true;
        RandomNormalSound();
        int _random = Random.Range(0, 4); //대기, 풀뜯기, 두리번, 걷기
        //마지막 4는 포함되지 않는다. 0~3

        if (_random == 0)
            Wait();
        if (_random == 1)
            Eat();
        if (_random == 2)
            Peek();
        if (_random == 3)
            TryWalk();
    }
}
