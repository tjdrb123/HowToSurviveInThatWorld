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
    public Inventory BaseInventory { get; set; }
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
    public void BackPackCheck() //가방인벤토리를 사용이 가능한지 체크
    {
        if (BackPackInventory != null && EquipInventory != null)
        {
            if (EquipInventory.GetSlot(E_SlotType.BackPack).SlotType == E_SlotType.BackPack && EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.KeyNumber != 0)
            {
                ArmorItem armorItem =  EquipInventory.GetSlot(E_SlotType.BackPack).ItemData as ArmorItem;
                BackPackInventory.inventoryAvailableSlots = armorItem.PlusValue;
            }
            else if (EquipInventory.GetSlot(E_SlotType.BackPack).SlotType == E_SlotType.BackPack && EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.KeyNumber == 0)
                BackPackInventory.inventoryAvailableSlots = -1;
        }
    }
    public int ItemCheck(ItemDataSo item) //아이템의 갯수를 확인하는 함수
    {
        int currentValue = 0;
        for (int i = 0; i < BaseInventory.BaseSlot.Length; i++)
        {
            if (item.KeyNumber == BaseInventory.BaseSlot[i].ItemData.KeyNumber)
            {
                currentValue += BaseInventory.BaseSlot[i].ItemData.CurrentAmont;
            }
        }
        for (int i = 0; i < BackPackInventory.BaseSlot.Length; i++)
        {
            if (item.KeyNumber == BackPackInventory.BaseSlot[i].ItemData.KeyNumber)
            {
                currentValue += BaseInventory.BaseSlot[i].ItemData.CurrentAmont;
            }
        }
        return currentValue;
    }
    public bool InventoryMaxCheck(Inventory inventory) //인벤토리 공간 체크
    {
        int count = 0;
        for (int i = 0; i < inventory.inventoryAvailableSlots; i++)
        {
            if (inventory.BaseSlot[i].ItemData.KeyNumber != 0)
                count++;
        }
        return count < inventory.inventoryAvailableSlots;
    }
}
