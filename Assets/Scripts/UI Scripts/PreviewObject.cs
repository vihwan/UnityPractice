using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{

    //충돌한 오브젝트의 컬라이더
    private List<Collider> colliderList = new List<Collider>();
    [SerializeField]
    private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;


    private void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0)
        {
            //색상을 레드로
            SetColor(red);
        }
        else
        {
            //색상을 초록으로
            SetColor(green);
        }
    }

    private void SetColor(Material mat)
    {
        //해당되는 오브젝트의 각 자식 트랜스폼마다 새로운 색상으로 바꾼다.
        foreach(Transform tf_Child in this.transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }
            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Remove(other);
    }

    public bool IsBuildable()
    {
        return colliderList.Count == 0;
    }
}
