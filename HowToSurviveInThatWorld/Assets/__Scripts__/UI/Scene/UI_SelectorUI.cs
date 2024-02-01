using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_SelectorUI : UI_Scene
{
    enum E_Button //°¢Á¾ ¹öÆ°µéÀ» °ü¸®ÇÑ´Ù.
    {
        FemaleBtn,
        maleBtn,
        SelectorBtn,
    }
    enum E_Text
    {
        PlayerNickName,
    }
    enum E_Object
    {
        InputField,
    }
    enum E_Image
    {
        MaleImage,
        FemaleImage,
    }
    [SerializeField] Animator _maleAnimation;
    [SerializeField] Animator _femaleAnimation;
    private GameObject _popUP;
    private TMP_InputField _inputField;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));
        BindText(typeof(E_Text));
        BindObject(typeof(E_Object));
        BindImage(typeof(E_Image));

        ChangeGender(0);
        GetButton((int)E_Button.FemaleBtn).onClick.AddListener(() => ChangeGender(0));
        GetButton((int)E_Button.maleBtn).onClick.AddListener(() => ChangeGender(1));
        GetButton((int)E_Button.SelectorBtn).onClick.AddListener(() => SelectorCharacter());

        _popUP = Resources.Load<GameObject>("UI_SelectorPopUp");
        _inputField = GetObject((int)E_Object.InputField).GetComponent<TMP_InputField>();
        _inputField.onValueChanged.AddListener(ValidateInput);
        return true;
    }

    private void ValidateInput(string arg0)
    {
        _inputField.text = Regex.Replace(arg0, @"[^a-zA-Z0-9°¡-ÆR]", "");
    }
    private void SelectorCharacter()
    {
        /*
         * Ä³¸¯ÅÍÀÇ ÀÌ¸§°ú ¼ºº°À» ¼±ÅÃÀ» ÇÏ¸é ÀÛµ¿ÇÒ ÇÔ¼ö
         * Á¶°Ç 1 (ÀÌ¸§ÀÌ 2±ÛÀÚ ¹Ì¸¸ 16±ÛÀÚ ÃÊ°ú ½Ã ÆË¾÷ ¿ÀÇÂ)
         * Á¶°Ç 3 (ÀÌ¸§ÀÌ Áßº¹µÈ °æ¿ì ÆË¾÷ ¿ÀÇÂ)
         * Á¶°ÇÀÌ ÃæÁ·µÇ¾úÀ» ¶§ (ÃÖÁ¾¼±ÅÃ ÆË¾÷ ¿ÀÇÂ)
         */
        string nickname = GetText((int)E_Text.PlayerNickName).text;
        GameObject popup = Instantiate(_popUP);
        if (nickname.Length <= 2 || nickname.Length >= 17)
        {
            popup.GetComponent<UI_SelectorPopUP>().TextChange("·Î±×ÀÎ ¿¡·¯", "±ÛÀÚ ¼ö´Â 2 ~ 16±ÛÀÚ \n »çÀÌ·Î ÇØÁÖ¼¼¿ä.");
        }
        else
        {
            popup.GetComponent<UI_SelectorPopUP>().TextChange("´Ğ³×ÀÓ »ı¼º ¿Ï·á", $"ÇØ´ç \"{nickname}\" ÀÌ¸§À¸·Î \n »ı¼ºÇÏ½Ã°Ú½À´Ï±î?", false);
        }
    }

    private void ChangeGender(int GenderIndex) //¼ºº°À» ¹Ù²Ù´Â ÇÔ¼ö
    {
        _femaleAnimation.SetFloat("sit", GenderIndex == 1 ? 1 : 0);
        _maleAnimation.SetFloat("sit", GenderIndex == 1 ? 0 : 1);
        ColorChange(GenderIndex);
        // ¼±ÅÃÇÑ Data¸¦ ³Ñ°ÜÁà¾ßÇÔ »ïÇ× ¿¬»êÀÚ¸¦ ÅëÇØ ¾î¶² ¼±ÅÃÀ» Çß´ÂÁö ³Ñ°ÜÁÖ±â 1 == ³²ÀÚ 0 == ¿©ÀÚ
    }
    private void ColorChange(int GenderIndex)
    {
        Color maleBtnColor = GetImage((int)E_Image.MaleImage).color;
        Color FemaleBtnColor = GetImage((int)E_Image.FemaleImage).color;
        maleBtnColor = (GenderIndex == 1) ? new Color(61 / 255f, 255 / 255f, 0 / 255f) : new Color(1, 1, 1);
        FemaleBtnColor = (GenderIndex != 1) ? new Color(61 / 255f, 255 / 255f, 0 / 255f) : new Color(1, 1, 1);
        if (GenderIndex == 1)
        {
            GetImage((int)E_Image.MaleImage).color = maleBtnColor;
            GetImage((int)E_Image.FemaleImage).color = FemaleBtnColor;
        }
        else
        {
            GetImage((int)E_Image.MaleImage).color = maleBtnColor;
            GetImage((int)E_Image.FemaleImage).color = FemaleBtnColor;
        }
    }
}
