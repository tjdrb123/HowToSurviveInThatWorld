using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftingInventory : UI_Popup
{
    enum E_Button
    {
        Btn_Close,
    }
    enum E_Object
    {
        BaseInven,
        BackPack,
    }
    public ItemData[] ItemData;
    private void Start()
    {
        DataReset();
    }
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));
        BindObject(typeof(E_Object));
        GetButton((int)E_Button.Btn_Close).onClick.AddListener(BtnClose);
        return true;
    }
    public void DataReset()
    {
        GetObject((int)E_Object.BaseInven).GetComponent<Inventory>().SlotDataSet(Manager_Inventory.Instance.BaseSlotDatas);
        GetObject((int)E_Object.BackPack).GetComponent<Inventory>().SlotDataSet(Manager_Inventory.Instance.BackPackSlotDatas);
        GetObject((int)E_Object.BackPack).GetComponent<Inventory>().inventoryAvailableSlots = Manager_Inventory.Instance.EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.PlusValue;
        SetInventory();
    }
    public void SetInventory()
    {
        Manager_Inventory.Instance.BaseInventory = GetObject((int)E_Object.BaseInven).GetComponent<Inventory>();
        Manager_Inventory.Instance.BackPackInventory = GetObject((int)E_Object.BackPack).GetComponent<Inventory>();
    }
    private void BtnClose()
    {
        for (int i = 0; i < 15; i++)
        {
            Manager_Inventory.Instance.BaseSlotDatas[i] = GetObject((int)E_Object.BaseInven).GetComponent<Inventory>().BaseSlot[i].ItemData;
            Manager_Inventory.Instance.BackPackSlotDatas[i] = GetObject((int)E_Object.BackPack).GetComponent<Inventory>().BaseSlot[i].ItemData;
        }
        Manager_Inventory.Instance.BtnSounds(1);
        ClosePopup();
    }
}
