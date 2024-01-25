using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Euipable
{
    Armor,
    Weapon,
}
public class Equipable : ItemBase
{
    public E_Euipable EquipableType;
}
