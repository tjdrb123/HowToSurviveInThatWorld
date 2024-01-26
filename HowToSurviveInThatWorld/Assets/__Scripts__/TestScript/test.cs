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
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            Debug.Log("¿€µø");
            Inventory.Instance.SlotAddItem(item);
        }
    }
}
