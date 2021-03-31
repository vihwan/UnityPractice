using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/craft")]
public class Crafts : ScriptableObject
{
    public string craftName; //건축물의 이름
    [TextArea]
    public string craftDescription; // 건축물의 설명
    public Sprite craftImage; //건축물의 이미지
}
