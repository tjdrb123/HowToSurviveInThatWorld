using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemSlot[] _baseSlot; //Inventory가 현재 가지고 있는 슬롯들

    //교체를 하기위한 슬롯
    private ItemSlot _firstSlot; 
    private ItemSlot _secondSlot;

    public ItemSlot[] BaseSlot { get => _baseSlot; }
    public int _inventoryAvailableSlots { get; set; }
    private void Awake()
    {
        _baseSlot = GetComponentsInChildren<ItemSlot>(); //자기 자식들중 ItemSlot을 가지고 있는 오브젝트들을 가져옵니다.
        _inventoryAvailableSlots = (this.name == "BackPack") ? -1 : _baseSlot.Length;
    }
    public void SlotDataSet(ItemData[] items) //슬롯의 값들을 셋팅해주기 위한 함수
    {
        if (this.name == "Equipments") // 장착 인벤토리는 슬롯의 타입을 변경하기 위해서 이런식으로 만들었음 어떤식으로 변경할지 생각해야함
        {
            for (int i = 0; i < _baseSlot.Length; i++)
                _baseSlot[i].SetInfo(items[i], i, i);
        }
        else
        {
            for (int i = 0; i < _baseSlot.Length; i++)
                _baseSlot[i].SetInfo(items[i], i);
        }
    }
    public void CombineSlot(ItemData item) //아이템이 인벤토리에 추가 되면 아이템을 합치기 위한 함수
    {
        for (int i = 0; i < _baseSlot.Length; i++)
        {
            if (_baseSlot[i].ItemData.name == item.name && _baseSlot[i].ItemData.stack != item.maxStack)
            {
                int stack = _baseSlot[i].MaxStackCheck(item.stack);
                if (stack != 0) //Item의 수량이 0이 아니면 아이템 추가하기
                {  
                    item.stack = stack;
                    continue;
                }
                else
                    return;
            }
        }
        AddItem(item);
    }
    public void CombineSlot(ItemDataSo item) //아이템이 인벤토리에 추가 되면 아이템을 합치기 위한 함수
    {
        UseItem itme;
        if (item.KeyNumber >= 50 && item.KeyNumber < 100)
        {
            itme = item as UseItem;
            for (int i = 0; i < _baseSlot.Length; i++)
            {
                if (_baseSlot[i].ItemData.name == item.name && _baseSlot[i].ItemData.stack != itme.MaxStack)
                {
                    int stack = _baseSlot[i].MaxStackCheck(itme.CurrentAmont);
                    if (stack != 0) //Item의 수량이 0이 아니면 아이템 추가하기
                    {
                        itme.CurrentAmont = stack;
                        continue;
                    }
                    else
                        return;
                }
            }
        }
    }
    public void SlotSwap(ItemSlot firstslot, ItemSlot secondSlot) //드래그할 위치, 드래그 슬롯에 저장된 슬롯을 옮기기 위해서
    { 
        _firstSlot = firstslot; //현재 슬롯 (내가 옮기고 싶은 Slot)
        _secondSlot = secondSlot; //옛날 슬롯 (DragSlot에 저장되어 있는 Slot입니다.)
        if (_firstSlot.SlotIndex <= _inventoryAvailableSlots)
        {
            if (_firstSlot.ItemData.name == _secondSlot.ItemData.name && !(_secondSlot.ItemData.stack == _secondSlot.ItemData.maxStack || _firstSlot.ItemData.stack == _firstSlot.ItemData.maxStack))
            {
                int stack = _secondSlot.MaxStackCheck(firstslot.ItemData.stack); //옮길위치에 슬롯의 Data에게 값을 전달하여 숫자를 증가시킴
                if (stack == 0)
                    _firstSlot.ItemData = new ItemData();
                else
                    _firstSlot.ItemData.stack = stack;
            }
            DataSwap();
            ImageSwap();
        }
        Manager_Inventory.Instance.BackPackCheck(); //스왑을 했을 때 가방쪽으로 옮겼는지 확인하는 함수
    }
    private void DataSwap() //Data를 교체한다.
    {
        ItemData tempData = _firstSlot.ItemData;
        _firstSlot.ItemData = _secondSlot.ItemData;
        _secondSlot.ItemData = tempData;
    }
    private void ImageSwap()  //슬롯의 이미지를 교체한다.
    {
        _secondSlot.Swap();
        _firstSlot.Swap();
    }
    private void AddItem(ItemData item) //아이템을 추가하는 함수입니다.
    {
        for (int i = 0; i < _baseSlot.Length; i++)
        {
            if (_baseSlot[i].ItemData.keyNumber == 0)
            {
                _baseSlot[i].AddItem(new ItemData(item));
                break;
            }
        }
    }
    public ItemSlot GetSlot(E_ItemType ItemType) //타입을 매개변수로 받아 타입에 맞게 슬롯을 전달함
    {
        return _baseSlot[(int)ItemType];
    }
}
