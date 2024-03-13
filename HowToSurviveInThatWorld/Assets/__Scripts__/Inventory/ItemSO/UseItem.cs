using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_UseItemType
{
    Hp,
    Hunger,
}

[CreateAssetMenu(fileName = "UseItemData", menuName = "ItemDataSO/UseItem", order = 0)]
public class UseItem : EtcItem
{
    public E_UseItemType UseType; //아이템이 무엇을 채워줄지

    public UseItem(UseItem useItem) : base(useItem)
    {
        UseType = useItem.UseType;
    }
}
