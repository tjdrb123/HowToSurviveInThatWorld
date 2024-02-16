using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Popup
{
    enum E_Button
    {
        Btn_Close,
        SelectBtn,
        RemoveBtn,
    }
    enum E_Object
    {
        BaseInven,
        BackPack,
        Equipments,
        SelectBtn,
        RemoveBtn,
    }
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
        GetObject((int)E_Object.Equipments).GetComponent<Inventory>().SlotDataSet(Manager_Inventory.Instance.EquipMentSlotDatas);
        Manager_Inventory.Instance.BackPackInventory = GetObject((int)E_Object.BackPack).GetComponent<Inventory>();
        Manager_Inventory.Instance.EquipInventory = GetObject((int)E_Object.Equipments).GetComponent<Inventory>();
    }
    public void SelectSlot(ItemSlot itemSlot)
    {
        if (itemSlot != null)
        {
            if (itemSlot.ItemData.BaseType == E_BaseType.UseItem)
            {
                GetObject((int)E_Object.SelectBtn).GetComponent<Color>();
                GetObject((int)E_Object.SelectBtn).GetComponent<Color>();
            }
            
        }
    }
    private void SetAlpha(Color color, bool isbool = false) //인벤토리의 아이템이 없으면 이미지의 Color값을 변경함
    {
        Color colAlpha = color;
        colAlpha.a = isbool ? 1f : 0.2f;
        color = colAlpha;
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
}
