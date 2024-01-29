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
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindButton(typeof(E_Button));

        GetButton((int)E_Button.Btn_Close).onClick.AddListener(BtnClose);
        return true;
    }
    private void BtnClose()
    {
        ClosePopup();
    }
}
