using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : UI_Base
{
    enum Buttons
    {
        playButton,
        clearButton
    }
    public override bool Initialize()
    {
        if (!base.Initialize())
              return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.playButton).onClick.AddListener(SoundPlay);
        GetButton((int)Buttons.clearButton).onClick.AddListener(Clear);
        Manager_Sound.instance.AddClip("sound");
        return true;
    }

    private void SoundPlay()
    {
        Manager_Sound.instance.PlayBGM("sound");
    }
    private void Clear()
    {
        Manager_Sound.instance.AudioClear();
    }
}
