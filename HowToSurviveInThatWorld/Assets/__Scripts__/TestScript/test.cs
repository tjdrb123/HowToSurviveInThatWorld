using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public ItemData item;
    [SerializeField] private Button button;
    [SerializeField] private Inventory inventory;
    [SerializeField] GameObject Prefabs;

    public static test Instans;

    private void Awake()
    {
        Instans = this;
        item.name = "image";
        item.keyNumber = 1;
        item.stack = 0;
        item.maxStack = 0;
        item.itemBaseType = 4;

        button.onClick.AddListener(OpenChest);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            inventory.CombineSlot(new ItemData(item));
        }
    }
    public void OpenChest()
    {
        Instantiate(Prefabs);
    }
    private void OnTriggerEnter(Collider other)
    {
        button.gameObject.SetActive(true);
    }
}
