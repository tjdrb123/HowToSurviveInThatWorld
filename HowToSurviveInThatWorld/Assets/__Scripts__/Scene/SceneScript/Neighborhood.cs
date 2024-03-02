using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    private void Start()
    {
        Manager_Sound.instance.AudioPlay(gameObject, "Sounds/BGM/Neighborhood", true, true);
    }
}
