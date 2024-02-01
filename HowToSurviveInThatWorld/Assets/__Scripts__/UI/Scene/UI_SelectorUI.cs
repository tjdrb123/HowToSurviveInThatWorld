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
    enum E_Button //각종 버튼들을 관리한다.
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
        _inputField.text = Regex.Replace(arg0, @"[^a-zA-Z0-9가-힣]", "");
    }
    private void SelectorCharacter()
    {
        /*
         * 캐릭터의 이름과 성별을 선택을 하면 작동할 함수
         * 조건 1 (이름이 2글자 미만 16글자 초과 시 팝업 오픈)
         * 조건 3 (이름이 중복된 경우 팝업 오픈)
         * 조건이 충족되었을 때 (최종선택 팝업 오픈)
         */
        string nickname = GetText((int)E_Text.PlayerNickName).text;
        GameObject popup = Instantiate(_popUP);
        if (nickname.Length <= 2 || nickname.Length >= 17)
        {
            popup.GetComponent<UI_SelectorPopUP>().TextChange("로그인 에러", "글자 수는 2 ~ 16글자 \n 사이로 해주세요.");
        }
        else
        {
            popup.GetComponent<UI_SelectorPopUP>().TextChange("닉네임 생성 완료", $"해당 \"{nickname}\" 이름으로 \n 생성하시겠습니까?", false);
        }
    }

    private void ChangeGender(int GenderIndex) //성별을 바꾸는 함수
    {
        _femaleAnimation.SetFloat("sit", GenderIndex == 1 ? 1 : 0);
        _maleAnimation.SetFloat("sit", GenderIndex == 1 ? 0 : 1);
        ColorChange(GenderIndex);
        // 선택한 Data를 넘겨줘야함 삼항 연산자를 통해 어떤 선택을 했는지 넘겨주기 1 == 남자 0 == 여자
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
