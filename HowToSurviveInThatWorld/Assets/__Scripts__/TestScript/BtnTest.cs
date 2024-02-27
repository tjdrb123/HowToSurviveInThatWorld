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
    enum E_Object
    {
        Button,
    }

    public GameObject Inventory;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));
        BindObject(typeof(E_Object));
        GetButton((int)E_Button.Button).onClick.AddListener(OpenInven);
        return true;
    }

    private void OpenInven()
    {
        Manager_Sound.instance.AudioPlay(GetObject((int)E_Object.Button), "sound");
        Instantiate(Inventory);
    }
}
