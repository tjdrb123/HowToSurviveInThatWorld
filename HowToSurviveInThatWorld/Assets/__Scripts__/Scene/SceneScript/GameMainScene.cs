using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainScene : MonoBehaviour
{
    private void Start()
    {
        Manager_Sound.instance.AudioClear();
        Manager_Sound.instance.AudioPlay(gameObject, "Sounds/BGM/TitleSound", true, true);
    }
}
