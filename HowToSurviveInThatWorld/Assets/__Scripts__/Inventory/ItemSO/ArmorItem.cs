using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ArmorItemType
{
    Helmet = 20,
    Shirt,
    Trousers,
    Boots,
    BackPack,
    Sub_Weapon,
}


[CreateAssetMenu(fileName = "ArmorItemData", menuName = "ItemDataSO/ArmorItem", order = 0)]
public class ArmorItem : ItemDataSo
{
    public E_ArmorItemType armorType;

    public ArmorItem(ArmorItem armorItem) : base(armorItem)
    {
        armorType = armorItem.armorType;
    }
}
