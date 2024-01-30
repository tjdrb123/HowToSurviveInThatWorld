using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Popup
{
    enum E_Button
    {
        Btn_Close,
    }
    enum E_Object
    {
        BaseInven,
        BackPack,
        Equipments
    }
    //private Inventory _Equipinven;
    private void Start()
    {
        DataReset();
    }
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        Debug.Log("작동");
        BindButton(typeof(E_Button));
        BindObject(typeof(E_Object));

        GetButton((int)E_Button.Btn_Close).onClick.AddListener(BtnClose);
        return true;
    }
    public void DataReset()
    {
        GetObject((int)E_Object.BaseInven).GetComponent<Inventory>().SlotAndDataReset(Manager_Inventory.Instance.BaseSlotDatas);
        GetObject((int)E_Object.BackPack).GetComponent<Inventory>().SlotAndDataReset(Manager_Inventory.Instance.BackPackSlotDatas);
        GetObject((int)E_Object.Equipments).GetComponent<Inventory>().SlotAndDataReset(Manager_Inventory.Instance.EquipMentSlotDatas);
        Manager_Inventory.Instance.BackPackInventory = GetObject((int)E_Object.BackPack).GetComponent<Inventory>();
        Manager_Inventory.Instance.EquipInventory = GetObject((int)E_Object.Equipments).GetComponent<Inventory>();
    }
    private void BtnClose()
    {
        for (int i = 0; i < 15; i++)
        {
            Manager_Inventory.Instance.BaseSlotDatas[i] = GetObject((int)E_Object.BaseInven).GetComponent<Inventory>().BaseSlot[i].ItemData;
            Manager_Inventory.Instance.BackPackSlotDatas[i] = GetObject((int)E_Object.BackPack).GetComponent<Inventory>().BaseSlot[i].ItemData;
            if (i < 8)
            {
                Manager_Inventory.Instance.EquipMentSlotDatas[i] = GetObject((int)E_Object.Equipments).GetComponent<Inventory>().BaseSlot[i].ItemData;
            }
        }
        ClosePopup();
    }
    //public void BackPackCheck()
    //{
    //    Inventory BackPackinven = GetObject((int)E_Object.BackPack).GetComponent<Inventory>();
    //    _Equipinven = GetObject((int)E_Object.Equipments).GetComponentInChildren<Inventory>();
    //    if (BackPackinven != null && _Equipinven != null)
    //    {
    //        if (_Equipinven.GetSlot(E_ItemType.BackPack).SlotType == E_ItemType.BackPack && _Equipinven.GetSlot(E_ItemType.BackPack).ItemData.keyNumber != 0)
    //            BackPackinven._inventoryAvailableSlots = 5;
    //        else if (_Equipinven.GetSlot(E_ItemType.BackPack).SlotType == E_ItemType.BackPack && _Equipinven.GetSlot(E_ItemType.BackPack).ItemData.keyNumber == 0)
    //            BackPackinven._inventoryAvailableSlots = -1;
    //    }
    //}
}
