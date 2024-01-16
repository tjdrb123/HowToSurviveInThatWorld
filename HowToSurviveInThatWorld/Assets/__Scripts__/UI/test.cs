using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : UI_Base
{
    enum Buttons
    {
        Button,
    }
    enum Sounds
    {
        testAudio,
    }
    public override bool Initialize()
    {
        if (!base.Initialize())
              return false;

        BindButton(typeof(Buttons));
        BindAudio(typeof(Sounds));

        GetButton((int)Buttons.Button).onClick.AddListener(SoundPlay);
        Manager_Sound.instance.AddClip("testSound");
        return true;
    }

    private void SoundPlay()
    {
        Manager_Sound.instance.PlaySFX(GetAudio((int)Sounds.testAudio), "testSound");
    }
}
