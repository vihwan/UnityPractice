using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    //필요한 컴포넌트
    [SerializeField]
    private GameObject go_Inventory_Base;
    private SlotTooltip slotTooltip;

    [SerializeField]
    private GameObject go_SlotsParent;

    //슬롯들 관리
    private Slot[] slots;

    private void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        slotTooltip = FindObjectOfType<SlotTooltip>();
    }

    private void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_Inventory_Base.SetActive(false);
        slotTooltip.HideToolTip();
    }

    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_Inventory_Base.SetActive(true);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        //장비 아이템이 아닐 경우에만
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    //아이템이 기존에 있다면 갯수 증가
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
        //아이템 추가 (아이템 추가는 장비아이템일 경우에도 적용)
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }

}
