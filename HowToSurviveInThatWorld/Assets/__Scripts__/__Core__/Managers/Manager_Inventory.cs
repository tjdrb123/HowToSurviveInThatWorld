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
    private bool InventoryMaxCheck(Inventory inventory, ItemDataSo[] itemDataSo) //Data의 공간을 체크를 합니다.
    {
        int count = 0;
        for (int i = 0; i < itemDataSo.Length; i++)
        {
            if (itemDataSo[i].KeyNumber != 0)
            {
                var item = itemDataSo[i].BaseType == E_BaseType.UseItem ? itemDataSo[i] as UseItem : itemDataSo[i] as EtcItem;
                if (itemDataSo[i].CurrentAmont < item.MaxStack) //Data가 들어있지만 현재의 MaxStack아래면 True
                    return true;
                count++;
            }
        }
        return count < inventory.inventoryAvailableSlots;
    }
    public void Additem(ItemDataSo itemData, int value) //아이템을 추가하기 위해
    {
        bool isBaseInven = InventoryMaxCheck(BaseInventory, BaseSlotDatas);
        bool isBackPackInven = InventoryMaxCheck(BackPackInventory, BackPackSlotDatas);
        if (!isBaseInven && !isBackPackInven)
        {
            Debug.Log("인벤토리에 현재 공간이 없습니다.");
            return;
        }
        var item = itemData.BaseType == E_BaseType.UseItem ? new UseItem(itemData as UseItem) : new EtcItem(itemData as EtcItem);
        item.CurrentAmont = value;
        if (isBaseInven)
            CombineData(item, BaseSlotDatas);
        else if (isBackPackInven)
            CombineData(item, BackPackSlotDatas);
    }
    private void CombineData(ItemDataSo itemData, ItemDataSo[] itemDatas) //기타 아이템과 소비 아이템을 추가하기 위한 함수
    {
        if (itemData.BaseType == E_BaseType.UseItem || itemData.BaseType == E_BaseType.EtcItem)
        {
            var currentItem = itemData.BaseType == E_BaseType.UseItem ? itemData as UseItem : itemData as EtcItem;
            for (int i = 0; i < itemDatas.Length; i++)
            {
                if (itemDatas[i].KeyNumber != 0 && itemDatas[i].Name == itemData.Name && itemDatas[i].CurrentAmont != currentItem.MaxStack) //현재 Data가 안들어있고 
                {
                    Debug.Log("if 작동");
                    int stack = MaxValueCheck(currentItem, itemDatas[i]);
                    if (stack != 0) //Item의 수량이 0이 아니면 아이템 추가하기
                    {
                        currentItem.CurrentAmont = stack; //Stack의 값이 0이 아니고 정수이면 값 추가하기
                        continue;
                    }
                    else
                        return;
                }
                else if (itemDatas[i].KeyNumber == 0)
                {
                    BaseSlotDatas[i] = itemData;
                    return;
                }
            }
        }
    }
    private int MaxValueCheck(ItemDataSo currenData, ItemDataSo itemData) //MaxStack이 넘었는지 안넘었는지 확인하는 함수입니다.
    {
        if (currenData == null) return 0; //Data가 없으면 바로 리턴
        var item = currenData.BaseType == E_BaseType.UseItem ? currenData as UseItem : currenData as EtcItem;
        itemData.CurrentAmont += item == null ? 0 : item.CurrentAmont;
        if (itemData.CurrentAmont > item.MaxStack)
        {
            int returnValue = itemData.CurrentAmont - item.MaxStack;
            itemData.CurrentAmont = item.MaxStack;
            return returnValue;
        }
        return 0;
    }
}
