using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private ItemSlot _selectSlot;

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
        GetButton((int)E_Button.RemoveBtn).onClick.AddListener(RemoveItem);
        GetButton((int)E_Button.SelectBtn).onClick.AddListener(UseItem);
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
    public void SelectSlot(ItemSlot itemSlot) //선택버튼과 버리기 버튼의 알파값을 조절해줍니다.
    {
        if (itemSlot != null && itemSlot.ItemData.KeyNumber != 0)
        {
            _selectSlot = itemSlot;
            if (itemSlot.ItemData.BaseType == E_BaseType.UseItem)
            {
                SetAlpha(GetObject((int)E_Object.SelectBtn).GetComponent<Image>(), true);
                SetAlpha(GetObject((int)E_Object.RemoveBtn).GetComponent<Image>(), true);
            }
            else
            {
                SetAlpha(GetObject((int)E_Object.SelectBtn).GetComponent<Image>());
                SetAlpha(GetObject((int)E_Object.RemoveBtn).GetComponent<Image>(), true);
            }
        }
        else
        {
            SetAlpha(GetObject((int)E_Object.SelectBtn).GetComponent<Image>());
            SetAlpha(GetObject((int)E_Object.RemoveBtn).GetComponent<Image>());
        }
    }
    private void SetAlpha(Image image, bool isbool = false) //인벤토리의 아이템이 없으면 이미지의 Color값을 변경함
    {
        Color colAlpha = image.color;
        colAlpha.a = isbool ? 1f : 0.4f;
        image.color = colAlpha;
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
    public int GetBackPackSlot()
    {
        int count = 0;  
        for (int i = 0; i < 15; i++)
        {
            if (GetObject((int)E_Object.BackPack).GetComponent<Inventory>().BaseSlot[i].ItemData.KeyNumber != 0)
                count++;
        }
        return count;
    }
    private void RemoveItem()
    {
        _selectSlot.SlotClear();
        SetAlpha(GetObject((int)E_Object.SelectBtn).GetComponent<Image>());
        SetAlpha(GetObject((int)E_Object.RemoveBtn).GetComponent<Image>());
    }
    private void UseItem()
    {
        _selectSlot.UseItem();
        if (_selectSlot.ItemData.KeyNumber == 0)
        {
            SetAlpha(GetObject((int)E_Object.SelectBtn).GetComponent<Image>());
            SetAlpha(GetObject((int)E_Object.RemoveBtn).GetComponent<Image>());
        }
    }
}
