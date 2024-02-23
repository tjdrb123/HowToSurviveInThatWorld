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
    public int inventoryAvailableSlots { get; set; }
    private void Awake()
    {
        _baseSlot = GetComponentsInChildren<ItemSlot>(); //자기 자식들중 ItemSlot을 가지고 있는 오브젝트들을 가져옵니다.
        inventoryAvailableSlots = (this.name == "BackPack") ? -1 : _baseSlot.Length;
    }
    public void SlotDataSet(ItemDataSo[] items) //슬롯의 값들을 셋팅해주기 위한 함수
    {
        if (this.name == "Equipments") // 장착 인벤토리는 슬롯의 타입을 변경하기 위해서 이런식으로 만들었음 어떤식으로 변경할지 생각해야함
        {
            for (int i = 0; i < _baseSlot.Length; i++)
                _baseSlot[i].SetInfo(items[i], i, i + 20);
        }
        else
        {
            for (int i = 0; i < _baseSlot.Length; i++)
                _baseSlot[i].SetInfo(items[i], i);
        }
    }
    public void CombineSlot<T>(ItemDataSo item) where T : ItemDataSo//아이템이 인벤토리에 추가 되면 아이템을 합치기 위한 함수
    {
        if (item.BaseType == E_BaseType.UseItem || item.BaseType == E_BaseType.EtcItem)
        {
            var currentItem = item.BaseType == E_BaseType.UseItem ? item as UseItem : item as EtcItem;
            for (int i = 0; i < _baseSlot.Length; i++)
            {
                if (_baseSlot[i].ItemData.KeyNumber != 0 &&  _baseSlot[i].ItemImage.name == item.ItemImage.name && _baseSlot[i].ItemData.CurrentAmont != currentItem.MaxStack)
                {
                    int stack = _baseSlot[i].MaxStackCheck(item);
                    if (stack != 0) //Item의 수량이 0이 아니면 아이템 추가하기
                    {
                        currentItem.CurrentAmont = stack;
                        continue;
                    }
                    else
                        return;
                }
            }
        }
        AddItem(item);
    }
    public void SlotSwap(ItemSlot firstslot, ItemSlot secondSlot) //드래그할 위치, 드래그 슬롯에 저장된 슬롯을 옮기기 위해서
    { 
        _firstSlot = firstslot; //현재 슬롯 (내가 옮기고 싶은 Slot)
        _secondSlot = secondSlot; //옛날 슬롯 (DragSlot에 저장되어 있는 Slot입니다.)
        if (_firstSlot.SlotIndex <= inventoryAvailableSlots)
        {
            if (_secondSlot.ItemData != null && (_firstSlot.ItemData.BaseType == E_BaseType.UseItem || _firstSlot.ItemData.BaseType == E_BaseType.EtcItem))
            {
                var seconditem = _secondSlot.ItemData.BaseType == E_BaseType.UseItem ? _secondSlot.ItemData as UseItem : _secondSlot.ItemData as EtcItem;
                var firstitem = _firstSlot.ItemData.BaseType == E_BaseType.UseItem ? _firstSlot.ItemData as UseItem : _firstSlot.ItemData as EtcItem;

                if (seconditem != null && _firstSlot.ItemData.name == _secondSlot.ItemData.name && _firstSlot.ItemData.CurrentAmont != seconditem.MaxStack)
                {
                    int stack = _secondSlot.MaxStackCheck(firstitem); //옮길위치에 슬롯의 Data에게 값을 전달하여 숫자를 증가시킴
                    if (stack == 0)
                        _firstSlot.ItemData = new ItemDataSo();
                    else
                        _firstSlot.ItemData.CurrentAmont = stack;
                }
            }
            DataSwap();
            ImageSwap();
        }
        Manager_Inventory.Instance.BackPackCheck(); //스왑을 했을 때 가방쪽으로 옮겼는지 확인하는 함수
    }
    private void DataSwap() //Data를 교체한다.
    {
        ItemDataSo tempData = _firstSlot.ItemData;
        _firstSlot.ItemData = _secondSlot.ItemData;
        _secondSlot.ItemData = tempData;
    }
    private void ImageSwap()  //슬롯의 이미지를 교체한다.
    {
        _secondSlot.Swap();
        _firstSlot.Swap();
    }
    private void AddItem(ItemDataSo item) //아이템을 추가하는 함수입니다.
    {
        for (int i = 0; i < _baseSlot.Length; i++)
        {
            if (_baseSlot[i].ItemData.KeyNumber == 0)
            {
                if (item.BaseType == E_BaseType.UseItem) //소비아이템
                {
                    UseItem useItem = item as UseItem;
                    _baseSlot[i].AddItem(useItem);
                }
                else if (item.BaseType == E_BaseType.WeaponItem || item.BaseType == E_BaseType.SubWeaponItem) //무기 아이템
                {
                    WeaponItem weaponItem = item as WeaponItem;
                    _baseSlot[i].AddItem(weaponItem);
                }
                else if (item.BaseType == E_BaseType.ArmorItem) //방어구
                {
                    ArmorItem armorItem = item as ArmorItem;
                    _baseSlot[i].AddItem(armorItem);
                }
                else //기타
                {
                    _baseSlot[i].AddItem(item);
                }
                break;
            }
        }
    }
    public ItemSlot GetSlot(E_SlotType ItemType) //타입을 매개변수로 받아 타입에 맞게 슬롯을 전달함
    {
        return _baseSlot[(int)ItemType - 20];
    }
}
