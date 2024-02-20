using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInventory : MonoBehaviour
{
    [SerializeField] private ItemSlot[] _baseSlot; //Inventory가 현재 가지고 있는 슬롯들
    [SerializeField] private ItemDataSo[] _itemDataSo;
    public ItemSlot[] BaseSlot { get => _baseSlot; }
    private void Awake()
    {
        _baseSlot = GetComponentsInChildren<ItemSlot>(); //자기 자식들중 ItemSlot을 가지고 있는 오브젝트들을 가져옵니다.
        SlotDataSet();
    }
    public void SlotDataSet() //슬롯의 값들을 셋팅해주기 위한 함수
    {
        int i = 0;
        for (; i < _itemDataSo.Length; i++)
            _baseSlot[i].SetInfo(_itemDataSo[i], i, -1, false);
        for (; i < _baseSlot.Length; i++)
            _baseSlot[i].SetInfo(new ItemDataSo(), i, -1, false);
    }
}
