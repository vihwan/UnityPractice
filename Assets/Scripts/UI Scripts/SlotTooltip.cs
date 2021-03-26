using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text text_ItemName;
    [SerializeField]
    private Text text_ItemDescription;
    [SerializeField]
    private Text text_ItemHowToUse;

    public void ShowToolTip(Item _item, Vector3 _position)
    {
        go_Base.SetActive(true);
        //툴팁이 표시되는 위치를 오른쪽 아래로 오게끔 바꾼다.
        //평소에 마우스를 중심으로 가운데 정렬이 되기 때문에
        //툴팁 가로와 세로의 절반만큼 더 추가하면 될 것이다. 
        _position += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, - go_Base.GetComponent<RectTransform>().rect.height * 0.7f, 0f);
        go_Base.transform.position = _position;

        text_ItemName.text = _item.itemName;
        text_ItemDescription.text = _item.itemDescription;
        text_ItemHowToUse.text = _item.itemDescription;


        if(_item.itemType == Item.ItemType.Equipment)
            text_ItemHowToUse.text = "우클릭 : 장착";
        else if (_item.itemType == Item.ItemType.Used)
            text_ItemHowToUse.text = "우클릭 : 사용";
        else
            text_ItemHowToUse.text = "";
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
