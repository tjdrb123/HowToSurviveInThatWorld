using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Inventory : MonoBehaviour
{
    public ItemData[] BaseSlotDatas { get; set; }
    public ItemData[] BackPackSlotDatas { get; set; }
    public ItemData[] EquipMentSlotDatas { get; set; }
    public ItemData[] ChestItemDatas { get; set; }

    public Inventory BackPackInventory { get; set; }
    public Inventory EquipInventory { get; set; }
    public static Manager_Inventory Instance;

    private void Awake()
    {
        Instance = this;
        DataSetting();

    }
    private void DataSetting()
    {
        BaseSlotDatas = new ItemData[15];
        BackPackSlotDatas = new ItemData[15];
        ChestItemDatas = new ItemData[15];
        EquipMentSlotDatas = new ItemData[8];
        for (int i = 0; i < 15; i++)
        {
            BaseSlotDatas[i] = new ItemData();
            BackPackSlotDatas[i] = new ItemData();
            ChestItemDatas[i] = new ItemData();
            if (i < 8)
            {
                EquipMentSlotDatas[i] = new ItemData();
            }
        }
    }
    public void BackPackCheck()
    {
        if (BackPackInventory != null && EquipInventory != null)
        {
            if (EquipInventory.GetSlot(E_ItemType.BackPack).SlotType == E_ItemType.BackPack && EquipInventory.GetSlot(E_ItemType.BackPack).ItemData.keyNumber != 0)
                BackPackInventory._inventoryAvailableSlots = 5;
            else if (EquipInventory.GetSlot(E_ItemType.BackPack).SlotType == E_ItemType.BackPack && EquipInventory.GetSlot(E_ItemType.BackPack).ItemData.keyNumber == 0)
                BackPackInventory._inventoryAvailableSlots = -1;
        }
    }
}
