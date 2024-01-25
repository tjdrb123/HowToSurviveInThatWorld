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
        RightEquipments,
    }

    [SerializeField] private ItemSlot[] _baseSlot;      //기본 슬롯
    [SerializeField] private ItemSlot[] _equipSlot;     //장착 슬롯

    [SerializeField] private int _inventorySlot;

    public static Inventory Instance;

    // 슬롯을 교체하기 위한 index
    private ItemSlot _firstSlot; 
    private ItemSlot _secondSlot;

    private void Awake()  //임시 싱글톤
    {
        Instance = this;
    }
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindObject(typeof(E_Object));
        BindButton(typeof(E_Button));
        
        GetButton((int)E_Button.Btn_Close).onClick.AddListener(BtnClose);

        _baseSlot = GetObject((int)E_Object.BaseInven).GetComponentsInChildren<ItemSlot>(); //기본 아이템 슬롯
        _equipSlot = GetObject((int)E_Object.RightEquipments).GetComponentsInChildren<ItemSlot>(); //장착 슬롯
        SlotAndDataReset();
        return true;
    }
    private void SlotAndDataReset() //슬롯과 아이템 초기화 , itemData는 어떠한 형식으로 받으리 고민해야함 아이템이 들어있으면 저장되어있는 값들을 가져오도록
    {
        for (int i = 0; i < _baseSlot.Length; i++)
        {
            _baseSlot[i].SetInfo(new ItemData(), i);
        }
        for (int i = 0; i < _equipSlot.Length; i++)
        {
            _equipSlot[i].SetInfo(new ItemData(), i + 15, (E_ItemType)i);
        }
    }

    public void SlotSwap(ItemSlot firstslot, ItemSlot secondSlot) //슬롯의 번호와 target번호를 가져와 저장
    { 
        _firstSlot = firstslot;
        _secondSlot = secondSlot;
        DataChange();
        ImageSwapping();
    }
    private void DataChange() //Data를 교체한다.
    {
        ItemData tempData = _firstSlot.ItemData;
        _firstSlot.ItemData = _secondSlot.ItemData;
        _secondSlot.ItemData = tempData;
    }
    private void ImageSwapping()  //슬롯의 이미지를 교체한다.
    {
        var secondSlotImage = _secondSlot.SpriteRenderer.sprite;
        var secondSlotQuantity = _secondSlot.QuantityText.text;
        _secondSlot.Swap(_firstSlot.SpriteRenderer.sprite, _firstSlot.QuantityText.text);
        _firstSlot.Swap(secondSlotImage, secondSlotQuantity);
    }
    public E_ItemType TypeCheck(ItemSlot slot)
    {
        return (E_ItemType)slot.ItemData.itemBaseType;
    }

    private void BtnClose()
    {
        ClosePopup();
    }
}
