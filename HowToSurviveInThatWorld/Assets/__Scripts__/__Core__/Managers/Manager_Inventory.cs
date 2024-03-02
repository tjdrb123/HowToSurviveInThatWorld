using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Injector;
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
    [SerializeField] private GameObject _weaponPosition;
    public GameObject WeaponPosition { get => _weaponPosition; }

    private void Awake()
    {
        DataSetting();
        Instance = this;
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
                ArmorItem armorItem = EquipInventory.GetSlot(E_SlotType.BackPack).ItemData as ArmorItem;
                BackPackInventory.inventoryAvailableSlots = armorItem.PlusValue;
            }
            else if (EquipInventory.GetSlot(E_SlotType.BackPack).SlotType == E_SlotType.BackPack && EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.KeyNumber == 0)
            {
                BackPackInventory.inventoryAvailableSlots = -1;
            }
        }
    }
    public int ItemCheck(ItemDataSo item) //아이템의 갯수를 확인하는 함수
    {
        int currentValue = 0;
        for (int i = 0; i < BaseInventory.BaseSlot.Length; i++)
        {
            if (item.KeyNumber == BaseInventory.BaseSlot[i].ItemData.KeyNumber)
                currentValue += BaseInventory.BaseSlot[i].ItemData.CurrentAmont;
        }
        for (int i = 0; i < BackPackInventory.BaseSlot.Length; i++)
        {
            if (item.KeyNumber == BackPackInventory.BaseSlot[i].ItemData.KeyNumber)
                currentValue += BackPackInventory.BaseSlot[i].ItemData.CurrentAmont;
        }
        return currentValue;
    }
    public bool InventoryMaxCheck(ItemSlot[] itemslot, bool isBack = true) //인벤토리 공간 체크
    {
        int count = 0;
        for (int i = 0; i < itemslot.Length; i++)
        {
            if (itemslot[i].ItemData.KeyNumber != 0)
                count++;
        }
        if (isBack)
            return count < itemslot.Length;
        else
        {
            if (EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.KeyNumber != 0)
            {
                return count < EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.PlusValue + 1;
            }
            return false;
        }
    }
    public bool InventoryMaxCheck()
    {
        bool isBase = InventoryMaxCheck(BaseInventory.BaseSlot.Length, BaseSlotDatas);
        bool isBackPack = EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.KeyNumber == 0 ? false :InventoryMaxCheck(EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.PlusValue, BackPackSlotDatas);
        if (isBackPack || isBase)
            return true;
        else 
            return false;
    }
    private bool InventoryMaxCheck(int value, ItemDataSo[] itemDataSo) //Data의 공간을 체크를 합니다.
    {
        int count = 0;
        for (int i = 0; i < itemDataSo.Length; i++)
        {
            if (itemDataSo[i].KeyNumber != 0)
            {
                if (itemDataSo[i].BaseType == E_BaseType.UseItem || itemDataSo[i].BaseType == E_BaseType.EtcItem)
                {
                    var item = itemDataSo[i].BaseType == E_BaseType.UseItem ? itemDataSo[i] as UseItem : itemDataSo[i] as EtcItem;
                    if (itemDataSo[i].CurrentAmont < item.MaxStack) //Data가 들어있지만 현재의 MaxStack아래면 True
                        return true;
                }
                count++;
            }
        }
        return count < value;
    }
    public void Additem(ItemDataSo itemData, int value) //아이템을 추가하기 위해
    {
        bool isBaseInven = InventoryMaxCheck(BaseInventory.BaseSlot.Length, BaseSlotDatas);
        bool isBackPackInven = InventoryMaxCheck(EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.PlusValue, BackPackSlotDatas);
        if (!isBaseInven && !isBackPackInven)
        {
            Debug.Log("인벤토리에 현재 공간이 없습니다."); // 이제 활성화 버튼을 눌러도 아무런 변화가 안일어나게 해야함?
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
                    itemDatas[i] = currentItem;
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
    public bool ObjectDestroy(GameObject Object) 
    {
        if (Object.tag == "Chest" || Object.tag == "Produce")
            return false;
        for (int i = 0; i < ChestItemDatas.Length; i++)
        {
            if (ChestItemDatas[i].KeyNumber != 0)
                return false;
        }
        return true;
    }
    public void BtnSounds(int SoundNum)
    {
        if (SoundNum == 0)
            Manager_Sound.instance.AudioPlay(gameObject, "Sounds/SFX/ButtonSound");
        else if (SoundNum == 1)
            Manager_Sound.instance.AudioPlay(gameObject, "Sounds/SFX/EquipSound");
        else
            Manager_Sound.instance.AudioPlay(gameObject, "Sounds/SFX/CloseSound");
    }
    public void WeaponSwap(int weaponIndex)
    {
        for (int i = 0; i < 5; i++)//무기 초기화
            Manager_Inventory.Instance.WeaponPosition.transform.GetChild(i).gameObject.SetActive(false);
        if (weaponIndex > -1)
            Manager_Inventory.Instance.WeaponPosition.transform.GetChild(weaponIndex).gameObject.SetActive(true);
    }
    public int GetWeaponTypeIndex()
    {
        WeaponItem weaponItem = EquipMentSlotDatas[6] as WeaponItem;
        if (weaponItem == null)
            return -1;
        return (int)weaponItem.WeaponType;
    }
    public int GetSubWeaponTypeIndex()
    {
        WeaponItem weaponItem = EquipMentSlotDatas[5] as WeaponItem;
        if (weaponItem == null)
            return -1;
        return (int)weaponItem.WeaponType;
    }
}
