using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        Manager_Sound.instance.AudioPlay(gameObject, "Sounds/BGM/TitleSound", true, true);
    }
}
