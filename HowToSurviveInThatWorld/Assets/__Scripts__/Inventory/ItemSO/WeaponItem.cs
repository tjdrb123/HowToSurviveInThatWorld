using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_WeaponItemType
{
    Sword = 1,
    Pistol,
    Rifle,
    Axe,
    Pick,
}

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "ItemDataSO/WeaponItem", order = 0)]
public class WeaponItem : ItemDataSo
{
    public E_WeaponItemType WeaponType;

    public WeaponItem(WeaponItem weaponItem) : base(weaponItem)
    {
        WeaponType = weaponItem.WeaponType;
    }
}
