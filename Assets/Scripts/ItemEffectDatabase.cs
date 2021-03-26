using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //아이템의 이름, (키값)
    [Tooltip("HP, SP, DP, THIRSTY, HUNGRY, SATISFY만 가능합니다")]
    public string[] part; //부위
    public int[] num; //수치
}


public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;


    //필요한 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatusController;
    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private SlotTooltip theSlotTooltop;


    private const string HP = "HP"
        , SP = "SP"
        , DP = "DP"
        , HUNGRY = "HUNGRY"
        , THIRSTY = "THIRSTY"
        , SATISFY = "SATISFY";

    public void ShowToolTip(Item _item, Vector3 _position)
    {
        theSlotTooltop.ShowToolTip(_item, _position);
    }

    public void HideToolTip()
    {
        theSlotTooltop.HideToolTip();
    }


    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment) // 그 대상이 장비 아이템이면
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
            //"GUN", "SubmachineGun1"
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if(itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerStatusController.IncreaseHp(itemEffects[x].num[y]);
                                break;
                            case DP:
                                thePlayerStatusController.IncreaseDp(itemEffects[x].num[y]);
                                break;
                            case SP:
                                thePlayerStatusController.IncreaseSp(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatusController.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatusController.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status부위를 적용시키려고 하고 있습니다.");
                                break;
                        }
                        Debug.Log(_item.itemName + "을 사용했습니다");
                    }//for
                    return;
                }//if
            }//for
            Debug.Log("ItemEffectDatabase에 일치하는 itemName이 없습니다.");
        }
    }//UseItem

}//ItemEffectDatabase
