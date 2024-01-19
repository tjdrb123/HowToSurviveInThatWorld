using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_WeaponType
{
    Hand,   //¼Õ
    Axe,    //µµ³¢
    Pick,   //°î±ªÀÌ
    Sword,  //°Ë
    Gun,    //ÃÑ
}
public class MeleeData : WeaponData //±ÙÁ¢ ¹«±â
{
    public E_WeaponType WeaponType;
}
