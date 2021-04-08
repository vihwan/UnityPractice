using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeComponent : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private float woodMaxCount;
    [SerializeField] private float destroyTime;   //나무 오브젝트 파괴 시간
    [SerializeField] private string cutting_Sound;
    [SerializeField] private string destroy_Sound; //나무가 파괴될 때 사운드
    [SerializeField] private float fallDownForce; //나무가 쓰러질때 가하는 힘
    [SerializeField] private string logChange_sound; //나무가 생성될 때 나오는 소리


    [SerializeField] private GameObject go_TreeDebris; //나무 파편
    [SerializeField] private GameObject go_Tree;       //부모 트리
    [SerializeField] private GameObject go_ChildTree;  //자식 트리 (줄기)
    [SerializeField] private GameObject go_TreeItem;   //나무 아이템 (Wood)

    [SerializeField] private GameObject[] go_treePieces;  // 가운데 조각을 제외한 깎일 5 개의 나무 테두리 조각들.
    private Rigidbody treeTrunkRigid; //나무줄기의 리지드바디



    /// <summary>
    /// param 
    /// 1. _pos 도끼질 위치 : 도끼질한 위치에서 나무파편이 떨어져야함
    /// 2.
    /// </summary>
    public void WoodCutting(Vector3 _pos)
    {
        HitTreeEffect(_pos);

        SoundManager.instance.PlaySE(cutting_Sound);
        hp--;
        if(hp <= 0)
        {
            FallDownTree();
        }
    }


    //나무를 때릴 때 파편 생성 이펙트
    private void HitTreeEffect(Vector3 _pos)
    {
        //나무 파편 생성
        //나한테 생성되면 안되잖아.
        var clone = Instantiate(go_TreeDebris, _pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, destroyTime);
    }


    //나무가 쓰러짐
    private void FallDownTree()
    {
        #region 나무 파괴시 설명
        /* 나무가 파괴될 경우
         * 1. 전체 CapsuleCollider를 비활성화
         * 2. 나무줄기 CapsuleCollider를 활성화
         * 3. 나무줄기 RigidBody의 Gravity를 true;
         * 이를 통해 나무가 중력을 받고 쓰러지도록 연출을 만들 것이다.
         * **/
        #endregion
        SoundManager.instance.PlaySE(destroy_Sound);

        //나무 쓰러짐
        go_Tree.GetComponent<CapsuleCollider>().enabled = false; //나무전체 CapsuleCollider를 비활성화
        treeTrunkRigid = go_ChildTree.GetComponent<Rigidbody>();
        treeTrunkRigid.gameObject.GetComponent<CapsuleCollider>().enabled = true;  //나무줄기의 캡슐컬라이더를 활성화
        treeTrunkRigid.useGravity = true; //나무줄기 RigidBody의 Gravity를 true;

        //내가 바라보는 방향의 반대 방향으로 힘이 가해지면 좋겠다..
        treeTrunkRigid.AddForce(Random.Range(-fallDownForce - 1f, 0f), 0f, Random.Range(-fallDownForce, fallDownForce));

        //나무 아이템을 생성
        for (int i = 1; i <= woodMaxCount; i++)
            Instantiate(go_TreeItem, 
                        go_ChildTree.transform.position + (go_ChildTree.transform.up*(i*3)), 
                        Quaternion.LookRotation(go_ChildTree.transform.up));

        SoundManager.instance.PlaySE(logChange_sound);

        //나무 줄기 파괴
        Destroy(go_ChildTree.gameObject, destroyTime);
    }
}
