using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
//Create 항목에 New Item 항목을 새로 만들어서 양산을 쉽게 만든다.
public class Item : ScriptableObject
    //굳이 게임 오브젝트에 ADD할 필요가 없는 스크립트
{
    public string itemName; //아이템의 이름
    [TextArea] public string itemDescription; // 아이템 설명
    public Sprite itemImage; //아이템의 이미지
    public GameObject itemPrefab; //게임내 아이템 실체
    public ItemType itemType;

    public string weaponType; // 무기 유형
    
    public enum ItemType
    {
        Equipment, //0
        Used,      //1
        Ingredient,//2
        ETC        //3
    }
}
