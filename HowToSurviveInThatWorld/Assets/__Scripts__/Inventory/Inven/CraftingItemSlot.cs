using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItemSlot : CraftingInventory
{
    public ItemDataSo _itemData { get; set; }
    protected override void Start()
    {
        _itemData = null;
        base.Start();
    }
    public override void SlotDataSet()
    {
        if (_itemData != null && _itemData.KeyNumber != 0)
        {
            _itemDataSo = new ItemDataSo[_itemData.CraftingItems.Length];
            for (int i = 0; i < _itemData.CraftingItems.Length; i++)
            {
                _itemDataSo[i] = new ItemDataSo(_itemData.CraftingItems[i].item);
                _itemDataSo[i].CurrentAmont = _itemData.CraftingItems[i].itemValue;
            }
        }
        base.SlotDataSet();
    }
}
