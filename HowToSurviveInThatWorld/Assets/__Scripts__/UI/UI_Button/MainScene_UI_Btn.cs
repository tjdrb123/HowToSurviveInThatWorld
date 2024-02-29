using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainScene_UI_Btn : UI_Base
{
    enum E_Button
    {
        Setting_Button,
        BackPack_Button
    }
    enum E_Object
    {
        Setting_Button,
        BackPack_Button
    }

    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _setting;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));
        BindObject(typeof(E_Object));
        GetButton((int)E_Button.Setting_Button).onClick.AddListener(OpenSetting);
        GetButton((int)E_Button.BackPack_Button).onClick.AddListener(OpenInven);
        return true;
    }
    private void OpenSetting()
    {
        Manager_Inventory.Instance.BtnSounds(0);
        Instantiate(_setting);
    }
    private void OpenInven()
    {
        Manager_Inventory.Instance.BtnSounds(1);
        Instantiate(_inventory);
    }
}
