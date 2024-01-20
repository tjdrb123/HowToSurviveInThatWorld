using System;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    Item item;

    private void Start()
    {
        item = Managers.Data.LoadData<Item>("WeaponData");
        Debug.Log(item.KeyNumber);
    }
}

[Serializable]
public class Item
{
    public int KeyNumber; // 아이템 고유 번호
    public string Name; //아이템 이름 
    public string Description; // 아이템 정보
    public int ItmeBaseType; // 아이템 타입 = 소비, 장착, 기타
}

