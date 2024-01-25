using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ItemBaseType
{
    Consumable, //소비
    Weapon, //무기
    Etc, //기타
}
public class ItemBase
{
    public int KeyNumber; // 아이템 고유 번호
    public string Name; //아이템 이름 
    public string Description; // 아이템 정보
    public E_ItemBaseType ItmeBaseType; // 아이템 타입 = 소비, 장착, 기타
}


