using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BtnTest : UI_Base
{
    enum E_Button
    {
        Button,
    }

    public GameObject Inventory;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindButton(typeof(E_Button));
        GetButton((int)E_Button.Button).onClick.AddListener(OpenInven);
        return true;
    }

    private void OpenInven()
    {
        Instantiate(Inventory);
    }
}
