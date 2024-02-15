using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_WeaponItemType
{
    Sword,
    Axe,
    Pick,
    Gun
}

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "ItemDataSO/WeaponItem", order = 0)]
public class WeaponItem : ItemDataSo
{
    public E_WeaponItemType WeaponType;
}
