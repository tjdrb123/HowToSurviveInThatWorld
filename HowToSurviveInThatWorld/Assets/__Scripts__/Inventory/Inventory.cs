using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemSlot[] _baseSlot;      //기본 슬롯 배열

    // 슬롯을 교체하기 위한 index
    private ItemSlot _firstSlot; 
    private ItemSlot _secondSlot;

    private void Awake()  //임시 싱글톤
    {
        _baseSlot = GetComponentsInChildren<ItemSlot>();
        SlotAndDataReset();
    }
    private void SlotAndDataReset() //슬롯과 아이템 초기화 , itemData는 어떠한 형식으로 받으리 고민해야함 아이템이 들어있으면 저장되어있는 값들을 가져오도록
    {
        if (this.name == "Equipments")
        {
            for (int i = 0; i < _baseSlot.Length; i++)
                _baseSlot[i].SetInfo(new ItemData(), i, i);
        }
        else
        {
            for (int i = 0; i < _baseSlot.Length; i++)
                _baseSlot[i].SetInfo(new ItemData(), i);
        }
    }
    public void CombineSlot(ItemData item)
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
    public void SlotSwap(ItemSlot firstslot, ItemSlot secondSlot) //슬롯의 번호와 target번호를 가져와 저장
    { 
        _firstSlot = firstslot; //현재 슬롯
        _secondSlot = secondSlot; //옛날 슬롯
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
    private void DataSwap() //Data를 교체한다.
    {
        ItemData tempData = _firstSlot.ItemData;
        _firstSlot.ItemData = _secondSlot.ItemData;
        _secondSlot.ItemData = tempData;
    }
    private void ImageSwap()  //슬롯의 이미지를 교체한다.
    {
        var secondSlotImage = _secondSlot.SpriteRenderer.sprite;
        _secondSlot.Swap(_firstSlot.SpriteRenderer.sprite);
        _firstSlot.Swap(secondSlotImage);
    }
    private void AddItem(ItemData item)
    {
        for (int i = 0; i < _baseSlot.Length; i++)
        {
            if (_baseSlot[i].SpriteRenderer.sprite == null)
            {
                _baseSlot[i].AddItem(new ItemData(item));
                break;
            }
        }
    }
    public E_ItemType TypeCheck(ItemSlot slot)
    {
        return (E_ItemType)slot.ItemData.itemBaseType;
    }
}
