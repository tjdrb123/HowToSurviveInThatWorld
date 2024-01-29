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
    private void Start()
    {
        this.GetComponentInParent<UI_Inventory>().DataReset();
    }
    public void SlotAndDataReset(ItemData[] items) //슬롯과 아이템 초기화 , itemData는 어떠한 형식으로 받으리 고민해야함 아이템이 들어있으면 저장되어있는 값들을 가져오도록
    {
        if (this.name == "Equipments") //장착할 수 있는 Inventory는 Slot의 타입을 각각 부여합니다.
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
        _firstSlot = firstslot; //현재 슬롯 (내가 옮기고 싶은 Slot)
        _secondSlot = secondSlot; //옛날 슬롯 (DragSlot에 저장되어 있는 Slot입니다.)
        
        if (_firstSlot.slotIndex <= _inventoryAvailableSlots)
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
        this.GetComponentInParent<UI_Inventory>().BackPackCheck();
    }
    private void DataSwap() //Data를 교체한다.
    {
        ItemData tempData = _firstSlot.ItemData;
        _firstSlot.ItemData = _secondSlot.ItemData;
        _secondSlot.ItemData = tempData;
    }
    private void ImageSwap()  //슬롯의 이미지를 교체한다. 현재는 스프라이트를 전달하지만 itemSlot에 함수를 고쳐 변경하겠습니다.
    {
        var secondSlotImage = _secondSlot.SpriteRenderer.sprite;
        _secondSlot.Swap(_firstSlot.SpriteRenderer.sprite);
        _firstSlot.Swap(secondSlotImage);
    }
    private void AddItem(ItemData item) //아이템을 추가하는 함수입니다. Data를 이용해 Slot에 전달합니다. 
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
    public ItemSlot GetSlot(E_ItemType ItemType) 
    {
        return _baseSlot[(int)ItemType];
    }
}
