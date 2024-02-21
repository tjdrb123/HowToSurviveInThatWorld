using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewItemSlot : MonoBehaviour
{
    private ItemSlot _slot;

    private void Awake()
    {
        _slot = GetComponent<ItemSlot>();
    }
    public void NewItemAdd()
    {
        ItemDataSo[] itemSlots = Manager_Inventory.Instance.BaseSlotDatas;
        for (int i = 0; i < _slot.ItemData.CraftingItems.Length; i++)
        {
            for (int j = 0; j < itemSlots.Length; j++)
            {
                if (_slot.ItemData.CraftingItems[i].item.KeyNumber == itemSlots[j].KeyNumber)
                {

                }
            }
        }
    }
}
