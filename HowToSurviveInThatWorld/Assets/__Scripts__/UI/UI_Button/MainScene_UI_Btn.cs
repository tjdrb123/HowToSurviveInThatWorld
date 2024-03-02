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
        BackPack_Button,
        InformationCanvas,
    }
    enum E_Text
    {
        InformItemName,
        InformDescription,
    }
    enum E_Image
    {
        InformItemImage
    }

    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _setting;

    private void Start()
    {
        Manager_Inventory.Instance.MainScene_UI = this;
    }
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));
        BindObject(typeof(E_Object));
        BindText(typeof(E_Text));
        BindImage(typeof(E_Image));
        GetButton((int)E_Button.Setting_Button).onClick.AddListener(OpenSetting);
        GetButton((int)E_Button.BackPack_Button).onClick.AddListener(OpenInven);
        return true;
    }
    private void Start()
    {
        Manager_Inventory.Instance.MainScene_UI = this;
    }
    public void OpenInformation(ItemDataSo itemDataSo, bool isOpen) 
    {
        GetObject((int)E_Object.InformationCanvas).SetActive(isOpen);
        GetText((int)E_Text.InformItemName).text = itemDataSo.Name;
        GetText((int)E_Text.InformDescription).text = itemDataSo.Description;
        GetImage((int)E_Image.InformItemImage).sprite = itemDataSo.ItemImage;
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
