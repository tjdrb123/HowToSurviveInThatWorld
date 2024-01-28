using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public ItemData item;
    [SerializeField ] private Inventory inventory;
    private void Awake()
    {
        item.name = "image";
        item.keyNumber = 1;
        item.stack = 3;
        item.maxStack = 20;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            inventory.CombineSlot(new ItemData(item));
        }
    }
}
