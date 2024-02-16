using System;
using UnityEngine;

public enum E_SlotType
{
    None = -1,
    Helmet = 20,
    Shirt,
    Trousers,
    Boots,
    BackPack,
    Sub_Weapon,
    Weapon,
    Quick
}
public enum E_BaseType
{
    EtcItem,
    ArmorItem,
    SubWeaponItem = 25,
    WeaponItem,
    UseItem,
}

[CreateAssetMenu(fileName = "ItemBaseData", menuName = "ItemDataSO/ItemBase", order = 0)]
public class ItemDataSo : ScriptableObject
{
    public int KeyNumber; // 아이템 고유 번호
    public string Name; //아이템 이름 
    public string Description; // 아이템 정보
    public int PlusValue; // 아이템 사용 및 장착시 플러스 점수
    public int CurrentAmont; //현재 수량
    public Sprite ItemImage; //아이템이 쌓이는지 안쌓이는지 확인
    public E_BaseType BaseType;

    public ItemDataSo() { }

    public ItemDataSo(ItemDataSo item)
    {
        KeyNumber = item.KeyNumber;
        Name = item.Name;
        Description = item.Description;
        PlusValue = item.PlusValue;
        CurrentAmont = item.CurrentAmont;
        ItemImage = item.ItemImage;
        BaseType = item.BaseType;
    }
}