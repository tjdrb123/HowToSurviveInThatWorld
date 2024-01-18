using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tets : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    private void Start()
    {
        
    }

    private async void Init()
    {
        var operation = await Managers.Addressable.InitializeAsync();

        if (operation.IsSucceeded)
        {
            var oper2 = await Managers.Addressable.LoadLocationAsync("Common");
            if (oper2.IsSucceeded)
            {
                var oper3 = await Managers.Addressable.LoadAllAssetAsync<UnityEngine.Object>("Common");
            }
        }
    }
}
