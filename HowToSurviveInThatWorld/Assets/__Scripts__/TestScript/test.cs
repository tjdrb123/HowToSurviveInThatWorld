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

    public ItemData[] itemDatas;
    public static test Instans;

    private void Awake()
    {
        Instans = this;
        item.name = "image";
        item.keyNumber = 1;
        item.stack = 1;
        item.maxStack = 5;
        item.itemBaseType = 1;
        itemDatas = RandomItem();
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
    private ItemData[] RandomItem()
    {
        int randomLength = Random.Range(1, 5);
        itemDatas = new ItemData[15];
        int i = 0;
        for (; i < randomLength; i++)
        {
            itemDatas[i] = new ItemData();
            itemDatas[i].name = "image";
            itemDatas[i].keyNumber = 1;
            itemDatas[i].stack = 1;
            itemDatas[i].maxStack = 5;
            itemDatas[i].itemBaseType = 1;
        }
        for (; i < 15; i++)
        {
            itemDatas[i] = new ItemData();
        }
        return itemDatas;
    }
    private void OnTriggerEnter(Collider other)
    {
        Manager_Inventory.Instance.ChestItemDatas = itemDatas;
        button.gameObject.SetActive(true);
        //이벤트를 아이템에 대한 
    }
    private void OnTriggerExit(Collider other)
    {
        button.gameObject.SetActive(false);
    }
}
