using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Inventory : MonoBehaviour
{
    public ItemDataSo[] BaseSlotDatas { get; set; }
    public ItemDataSo[] BackPackSlotDatas { get; set; }
    public ItemDataSo[] EquipMentSlotDatas { get; set; }
    public ItemDataSo[] ChestItemDatas { get; set; }
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
        BaseSlotDatas = new ItemDataSo[15];
        BackPackSlotDatas = new ItemDataSo[15];
        ChestItemDatas = new ItemDataSo[15];
        EquipMentSlotDatas = new ItemDataSo[8];
        for (int i = 0; i < 15; i++)
        {
            BaseSlotDatas[i] = new ItemDataSo();
            BackPackSlotDatas[i] = new ItemDataSo();
            ChestItemDatas[i] = new ItemDataSo();
            if (i < 8)
            {
                EquipMentSlotDatas[i] = new ItemDataSo();
            }
        }
    }
    public void BackPackCheck()
    {
        if (BackPackInventory != null && EquipInventory != null)
        {
            if (EquipInventory.GetSlot(E_SlotType.BackPack).SlotType == E_SlotType.BackPack && EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.KeyNumber != 0)
            {
                ArmorItem armorItem =  EquipInventory.GetSlot(E_SlotType.BackPack).ItemData as ArmorItem;
                BackPackInventory._inventoryAvailableSlots = armorItem.PlusValue;
            }
            else if (EquipInventory.GetSlot(E_SlotType.BackPack).SlotType == E_SlotType.BackPack && EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.KeyNumber == 0)
                BackPackInventory._inventoryAvailableSlots = -1;
        }
    }
}
