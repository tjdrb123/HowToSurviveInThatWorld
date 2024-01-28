using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public ItemData item;
    private void Awake()
    {
        item.name = "image";
        item.keyNumber = 1;
        item.stack = 2;
        item.maxStack = 5;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            Inventory.Instance.CombineSlot(new ItemData(item));
        }
    }
}
