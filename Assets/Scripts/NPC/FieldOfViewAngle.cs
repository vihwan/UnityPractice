using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{

    [SerializeField] private float viewAngle; //시야각 (120도)
    [SerializeField] private float viewDistance; //시야 거리(10미터)
    [SerializeField] private LayerMask targetMask; //타겟 마스크 (플레이어)

    private Pig thePig;

    private void Start()
    {
        thePig = GetComponent<Pig>();
    }


    // Update is called once per frame
    void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        //돼지의 y값이 변할때마다 z축이 움직인다.
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad) //x
            , 0f
            , Mathf.Cos(_angle * Mathf.Deg2Rad)); //삼각함수를 이용
    }


    private void View()
    {
        //바라보는 방향(z축)을 기준으로 반을 나눠서 영역을 설정
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        //시야 내에 있는 모든 객체를 저장
        //그 중 플레이어 레이어만을 찾는다
        //OverlapSphere 대상 지점으로 부터 구를 그려 그 안에 있는 컬라이더를 모두 찾아 저장
        //param position, radius, layermask;
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        //타켓을 전부 뒤져서 대상을 찾아 행동을 시행한다.
        for(int i = 0; i < _target.Length; i++)
        {
            Transform _targetTransform = _target[i].transform;
            if (_targetTransform.name == "Player")
            {
                Vector3 _direction = (_targetTransform.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward); //자신이 바라보는 방향부터 플레이어의 방향 사이의 각도

                if(_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up , _direction, out _hit, viewDistance))
                    {
                        if(_hit.transform.name == "Player") //빛을 쐈을 때 플레이어 여야만 반응하도록
                        {
                            Debug.Log("플레이어가 돼지 시야 내에 있습니다");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }
            }
        }
    }
}
