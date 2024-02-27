using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingPopUp : UI_Popup
{
    enum E_Button
    {
        Close_Button,
        ON_Button,
        OFF_Button,
    }
    enum E_Object
    {
        ONCheckImage,
        OFFCheckImage,
        BGMSlider,
        SFXSlider,
    }
    private Slider _bgmSlider;
    private Slider _sfxSlider;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(E_Button));
        BindObject(typeof(E_Object));
        AudioMute(Manager_Sound.instance.IsMute);
        //닫기 버튼
        GetButton((int)E_Button.Close_Button).onClick.AddListener(() => ClosePopup());
        //음소거 버튼
        GetButton((int)E_Button.ON_Button).onClick.AddListener(() => AudioMute(false)); 
        GetButton((int)E_Button.OFF_Button).onClick.AddListener(() => AudioMute(true));
        //슬라이더
        _bgmSlider = GetObject((int)E_Object.BGMSlider).GetComponent<Slider>();
        _sfxSlider = GetObject((int)E_Object.SFXSlider).GetComponent<Slider>();
        _bgmSlider.value = Manager_Sound.instance.BGMVolume;
        _sfxSlider.value = Manager_Sound.instance.SFXVolume;
        _bgmSlider.onValueChanged.AddListener((volume) => AudioVolume(volume, 1));
        _sfxSlider.onValueChanged.AddListener((volume) => AudioVolume(volume, 2));
        return true;
    }

    private void AudioMute(bool isMute)
    {
        GetObject((int)E_Object.ONCheckImage).SetActive(!isMute);
        GetObject((int)E_Object.OFFCheckImage).SetActive(isMute);
        Manager_Sound.instance.AudioMute(isMute);
    }
    private void AudioVolume(float volume, int num)
    {
        Manager_Sound.instance.AudioVolume(volume, num);
    }
}
