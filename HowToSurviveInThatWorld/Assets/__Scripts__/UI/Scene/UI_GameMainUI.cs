using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameMainUI : UI_Scene
{
    enum E_Button //각종 버튼들을 관리한다.
    {
        NextStageBtn,
        SettingsBtn
    }

    [SerializeField] private GameObject _setting;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));

        GetButton((int)E_Button.NextStageBtn).onClick.AddListener(() => SceneLoad());
        GetButton((int)E_Button.SettingsBtn).onClick.AddListener(() => OpenSetting());
        return true;
    }
    private void SceneLoad()
    {
        SceneManager.LoadScene("02_Selector");
    }
    private void OpenSetting()
    {
        Instantiate(_setting);
    }
}
