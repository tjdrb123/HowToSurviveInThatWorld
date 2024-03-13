using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    public void NextSceneLoad(int index)
    {
        Manager_LoadingScene.LoadScene(index);
    }
}
