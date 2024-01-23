using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : UI_Popup
{
    enum E_Button
    {
        Btn_Close,
    }
    enum E_Object
    {
        BaseInven,
        DragSlot,
    }

    [SerializeField] private ItemData[] _itemDatas;
    [SerializeField] private ItemSlot[] _slot;

    [SerializeField] private int _inventorySlot;

    public static Inventory Instance;
    private void Awake()
    {
        Instance = this;
    }
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindObject(typeof(E_Object));
        BindButton(typeof(E_Button));
        
        GetButton((int)E_Button.Btn_Close).onClick.AddListener(BtnClose);

        _slot = GetObject((int)E_Object.BaseInven).GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < _inventorySlot; i++) //아이템 초기화
        {
            _itemDatas[i] = new ItemData();
            _slot[i].SetInfo(_itemDatas[i], i, GetObject((int)E_Object.DragSlot), this);
        }
        return true;
    }

    public void DataChange(int firstSlotIndex, int secondSlotIndex)
    {
        Debug.Log($"{firstSlotIndex} : {secondSlotIndex}");
        ItemData tempData = _itemDatas[secondSlotIndex];
        _itemDatas[secondSlotIndex] = _itemDatas[firstSlotIndex];
        _itemDatas[firstSlotIndex] = tempData;
    }

    private void BtnClose()
    {
        ClosePopup();
    }
}
