using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
    //인터페이스 추가 상속
    , IPointerEnterHandler //마우스 위에 올려놓으면 발생하는 이벤트 인터페이스
    , IPointerExitHandler //마우스 위에 올려놓은걸 빠져나올때 발생하는 이벤트 인터페이스
    , IPointerClickHandler //해당 포인트 위에 클릭 이벤트 인터페이스
    , IBeginDragHandler    //드래그 시작 인터페이스
    , IDragHandler         //드래그중 인터페이스
    , IEndDragHandler      //드래그 마침 인터페이스
    , IDropHandler         //드롭 이벤트 인터페이스
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템 갯수
    public Image itemImage; //아이템의 이미지

    //필요한 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;
    private ItemEffectDatabase theItemEffectDatabase;
    private SlotTooltip theSlotTooltip;

    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        //Prefab으로 된 Slot은 SerializeField로 선언할 경우 자신의 안에 있는 것들만 참조할 수 있기 때문에 상관없지만
        //밖에 있는 것들은 FindObjectOfType으로 하이어라키에서 검색하는것이 낫다.
        theSlotTooltip = FindObjectOfType<SlotTooltip>();
    }


    //이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }


    //아이템 획득
    public void AddItem(Item item, int count = 1)
    {
        this.item = item;
        itemCount = count;
        itemImage.sprite = item.itemImage;
        if (item.itemType != Item.ItemType.Equipment) //장비아이템이 아니면
        {
            text_Count.text = itemCount.ToString(); //카운트를 증가시킨다.
            go_CountImage.SetActive(true);

        }
        else //장비 아이템이면
        {
            text_Count.text = itemCount.ToString();
            go_CountImage.SetActive(false);
        }
        SetColor(1);
    }


    //아이템 갯수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        if (itemCount <= 0)
        {
            ClearSlot();
        }

    }

    //슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // 해당 대상 위에 마우스 우클릭을 할 경우
        {
            if (item != null) // 대상이 아이템이라면
            {
                theItemEffectDatabase.UseItem(item);
                if(item.itemType == Item.ItemType.Used)
                    SetSlotCount(-1); //슬롯 갯수 하나 줄이기     
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) //드래그 시작
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) //드래그 중
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //드래그가 끝나기만 해도 실행
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //다른 슬롯 위에 드래그가 끝나면 호출
        if (DragSlot.instance.dragSlot != null) // 드롭한 곳이 빈슬롯이 아니여야 위치를 바꾼다.
            ChangeSlot();

    }

    private void ChangeSlot()
    {
        //위치를 서로 바꾸려면
        //기존에 있던 아이템의 정보를 임시로 저장
        //드래그한 아이템을 옮길려고 하는 아이템 위치로 대입
        // temp = b;
        // b= a;
        // a = temp;

        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null) //옮김 당하는 아이템이 있다면 (서로 위치를 바꾼다.)
        {
            //기존 아이템 위치에 옮김 당한 아이템을 생성한다.
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else //옮김 당하는 아이템이 없다면
        {
            //기존 아이템 위치를 비운다.
            DragSlot.instance.dragSlot.ClearSlot();
        }
        theSlotTooltip.HideToolTip();
    }

    //마우스가 슬롯에 들어갈 때 발동
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            theItemEffectDatabase.ShowToolTip(item, transform.position);
        }
    }

    //마우스가 슬롯에서 빠져나올 때 발동
    public void OnPointerExit(PointerEventData eventData)
    {
        theItemEffectDatabase.HideToolTip();
    }
}
