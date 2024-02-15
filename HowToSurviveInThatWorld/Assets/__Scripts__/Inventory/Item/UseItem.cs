using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_UseItemType
{
    Hp,
    Hunger,
}

[CreateAssetMenu(fileName = "UseItemData", menuName = "ItemDataSO/UseItem", order = 0)]
public class UseItem : ItemDataSo
{
    public E_UseItemType UseType; //아이템이 무엇을 채워줄지
    public int CurrentAmont; //현재 수량
    public int MaxStack; //얼마정도로 쌓을것인지
}
