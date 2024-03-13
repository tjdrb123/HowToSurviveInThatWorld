using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameMainUI : UI_Scene
{
    enum E_Button 
    {
        SettingsBtn,
        PlayGameBtn,
        Point1,
        Point2,
        Point3,
        Point4,
        Point5,
        NoBtn
    }
    enum E_Object
    {
        PlayGamePanel
    }

    [SerializeField] private GameObject _setting;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));
        BindObject(typeof(E_Object));
        GetButton((int)E_Button.SettingsBtn).onClick.AddListener(() => OpenSetting());
        GetButton((int)E_Button.PlayGameBtn).onClick.AddListener(() => OpenPlayGamePanel());
        GetButton((int)E_Button.Point1).onClick.AddListener(() => ButtonSound());
        GetButton((int)E_Button.Point2).onClick.AddListener(() => ButtonSound());
        GetButton((int)E_Button.Point3).onClick.AddListener(() => ButtonSound());
        GetButton((int)E_Button.Point4).onClick.AddListener(() => ButtonSound());
        GetButton((int)E_Button.Point5).onClick.AddListener(() => ButtonSound());
        GetButton((int)E_Button.NoBtn).onClick.AddListener(() => ButtonSound());
        return true;
    }

    private void ButtonSound()
    {
        Manager_Sound.instance.AudioPlay(GetButton((int)E_Button.PlayGameBtn).gameObject, "Sounds/SFX/ButtonSound");
    }

    private void OpenPlayGamePanel()
    {
        Manager_Sound.instance.AudioPlay(GetButton((int)E_Button.PlayGameBtn).gameObject, "Sounds/SFX/ButtonSound");
        GetObject((int)E_Object.PlayGamePanel).SetActive(true);
    }

    private void OpenSetting()
    {
        Manager_Sound.instance.AudioPlay(GetButton((int)E_Button.SettingsBtn).gameObject, "Sounds/SFX/ButtonSound");
        Instantiate(_setting);
    }
}
