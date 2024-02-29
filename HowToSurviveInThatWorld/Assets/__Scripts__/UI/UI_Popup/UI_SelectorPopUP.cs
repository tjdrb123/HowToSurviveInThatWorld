using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_SelectorPopUP : UI_Popup
{
    enum E_Button //각종 버튼들을 관리한다.
    {
        PopUpBtn,
        NextSceneBtn,
        CancelBtn
    }
    enum E_Text
    {
        TextTitle,
        InformText,
    }
    enum E_Object
    {
        NextSceneBtn,
        CancelBtn,
        PopUpBtn
    }
    private bool _isBtnAtive;
    protected override void SetOrder() => _canvas.sortingOrder = 11;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        SetOrder();
        BindButton(typeof(E_Button));
        BindText(typeof(E_Text));
        BindObject(typeof(E_Object));

        GetButton((int)E_Button.NextSceneBtn).onClick.AddListener(() => SceneLoad());
        GetButton((int)E_Button.PopUpBtn).onClick.AddListener(() => ClosePopup());
        GetButton((int)E_Button.CancelBtn).onClick.AddListener(() => ClosePopup());
        return true;
    }
    private void BtnAtive()
    {
        GetObject((int)E_Object.CancelBtn).SetActive(!_isBtnAtive);
        GetObject((int)E_Object.NextSceneBtn).SetActive(!_isBtnAtive);
        GetObject((int)E_Object.PopUpBtn).SetActive(_isBtnAtive);
    }
    public void TextChange(string title, string inform, bool isBtn = true)
    {
        _isBtnAtive = isBtn;
        BtnAtive();
        GetText((int)E_Text.TextTitle).text = title;
        GetText((int)E_Text.InformText).text = inform;
    }
    private void SceneLoad()
    {
        SceneManager.LoadScene("06_Game_Main");
    }
}
