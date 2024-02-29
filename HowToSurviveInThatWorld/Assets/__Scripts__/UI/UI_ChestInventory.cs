using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChestInventory : UI_Popup
{
    enum E_Button
    {
        Btn_Close,
    }
    enum E_Object
    {
        BaseInven,
        BackPack,
        ChestInven
    }
    public ItemData[] ItemData;
    public GameObject Object;
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
        GetObject((int)E_Object.ChestInven).GetComponent<Inventory>().SlotDataSet(Manager_Inventory.Instance.ChestItemDatas);
        GetObject((int)E_Object.BackPack).GetComponent<Inventory>().inventoryAvailableSlots = Manager_Inventory.Instance.EquipInventory.GetSlot(E_SlotType.BackPack).ItemData.PlusValue;
    }
    private void BtnClose()
    {
        for (int i = 0; i < 15; i++)
        {
            Manager_Inventory.Instance.BaseSlotDatas[i] = GetObject((int)E_Object.BaseInven).GetComponent<Inventory>().BaseSlot[i].ItemData;
            Manager_Inventory.Instance.BackPackSlotDatas[i] = GetObject((int)E_Object.BackPack).GetComponent<Inventory>().BaseSlot[i].ItemData;
            Manager_Inventory.Instance.ChestItemDatas[i] = GetObject((int)E_Object.ChestInven).GetComponent<Inventory>().BaseSlot[i].ItemData;
        }
        Manager_Inventory.Instance.BtnSounds(1);
        if (Object != null && Manager_Inventory.Instance.ObjectDestroy(Object))
            Destroy(Object);
        ClosePopup();
    }
}
