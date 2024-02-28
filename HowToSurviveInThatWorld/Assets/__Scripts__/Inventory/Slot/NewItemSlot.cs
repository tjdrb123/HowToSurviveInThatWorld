using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class NewItemSlot : MonoBehaviour, IPointerDownHandler
{
    private ItemSlot _slot;

    private void Awake()
    {
        _slot = GetComponent<ItemSlot>();
    }
    public void NewItemAdd()
    {
        Manager_Inventory manager_Inventory = Manager_Inventory.Instance;
        ItemSlot[] baseInvenItems = manager_Inventory.BaseInventory.BaseSlot;
        ItemSlot[] BPInvenItems = manager_Inventory.BackPackInventory.BaseSlot;
        for (int i = 0; i < _slot.ItemData.CraftingItems.Length; i++)
        {
            if (_slot.ItemData.CraftingItems[i].itemValue > manager_Inventory.ItemCheck(_slot.ItemData.CraftingItems[i].item))
            {
                Debug.Log(_slot.ItemData.CraftingItems[i].item.Name + "아이템이 부족합니다.");
                return;
            }
        }
        if (manager_Inventory.InventoryMaxCheck(baseInvenItems))
        {
            ItemAdd(_slot.ItemData, manager_Inventory.BaseInventory);

        }
        else if (manager_Inventory.InventoryMaxCheck(BPInvenItems, false))
        {
            ItemAdd(_slot.ItemData, manager_Inventory.BackPackInventory);
        }
        else
        {
            Debug.Log("공간이 부족합니다.");
            return;
        }
        for (int i = 0; i < _slot.ItemData.CraftingItems.Length; i++)
        {
            int value = _slot.ItemData.CraftingItems[i].itemValue;
            for (int j = 0; j < baseInvenItems.Length; j++) //기본 인벤토리
            {
                if (_slot.ItemData.CraftingItems[i].item.KeyNumber == baseInvenItems[j].ItemData.KeyNumber)
                {
                    value = baseInvenItems[j].ItemMinus(Mathf.Abs(value));
                    if (value >= 0)
                        break;
                }
            }
            for (int j = 0; j < BPInvenItems.Length; j++) //가방 인벤토리
            {
                if (_slot.ItemData.CraftingItems[i].item.KeyNumber == BPInvenItems[j].ItemData.KeyNumber)
                {
                    value = BPInvenItems[j].ItemMinus(_slot.ItemData.CraftingItems[i].itemValue);
                    if (value >= 0)
                        break;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.GetComponentInParent<UI_CraftingInventory>().SetInventory();
        NewItemAdd();
    }
    private void ItemAdd(ItemDataSo Carrot, Inventory inventory)
    {
        if (Carrot.BaseType == E_BaseType.UseItem)
        {
            UseItem useitem = new UseItem(Carrot as UseItem);
            inventory.CombineSlot<UseItem>(useitem);
        }
        else if (Carrot.BaseType == E_BaseType.WeaponItem || Carrot.BaseType == E_BaseType.SubWeaponItem)
        {
            WeaponItem useitem = new WeaponItem(Carrot as WeaponItem);
            inventory.CombineSlot<WeaponItem>(useitem);
        }
        else if (Carrot.BaseType == E_BaseType.ArmorItem)
        {
            ArmorItem useitem = new ArmorItem(Carrot as ArmorItem);
            inventory.CombineSlot<ArmorItem>(useitem);
        }
        else
        {
            EtcItem etcItem = new EtcItem(Carrot as EtcItem);
            inventory.CombineSlot<EtcItem>(etcItem);
        }
    }
}
