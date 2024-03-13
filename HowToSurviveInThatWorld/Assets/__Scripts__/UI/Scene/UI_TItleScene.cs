using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_TItleScene : UI_Scene
{
    enum E_Button //���� ��ư���� �����Ѵ�.
    {
        Button
    }
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));

        GetButton((int)E_Button.Button).onClick.AddListener(() => SceneLoad());
        return true;
    }

    private void SceneLoad()
    {
        SceneManager.LoadScene("02_Selector");
    }
}
