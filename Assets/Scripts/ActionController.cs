using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //습득 가능한 최대거리

    public static bool pickupActivated = false;


    private RaycastHit hitInfo;

    //아이템 레이어에게만 반응을 해줘야한다.
    [SerializeField]
    private LayerMask layerMask;


    //필요한 컴포넌트
    [SerializeField]
    private Text actionText; //아이템 획득 출력 텍스트
    [SerializeField]
    private Inventory theInventory;

    //광선을 쏴서 범위 내에 충돌체가 있다면
    //E키를 눌러 아이템을 획득할 수 있다.

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void CheckItem()
    {
        //플레이어 위치에서 플레이어가 바라보는 위치로 광선을 쏜다.
        //충돌체의 위치를 받아 저장하며, 범위 내에 해당하는 레이어가 있는지 확인
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            ItemInfoDisappear();
    }

    private void ItemInfoAppear()
    {
        //주울 수 있게 된다
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 "
            + "<color=#fff400>" + "(E)" + "</color>";
    }


    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }


    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
}
