using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_WeaponType
{
    Hand,   //손
    Axe,    //도끼
    Pick,   //곡괭이
    Sword,  //검
    Gun,    //총
}
public class MeleeData : WeaponData //근접 무기
{
    public E_WeaponType WeaponType;
}
