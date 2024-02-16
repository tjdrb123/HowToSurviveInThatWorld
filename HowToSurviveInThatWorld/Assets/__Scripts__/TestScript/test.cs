using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public ItemDataSo item;
    [SerializeField] private Button button;
    [SerializeField] private Inventory inventory;
    [SerializeField] GameObject Prefabs;

    public ItemDataSo[] itemDatas;
    public static test Instans;
    public ItemDataSo Carrot;

    private void Awake()
    {
        Instans = this;
        itemDatas = RandomItem();
        button.onClick.AddListener(OpenChest);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            if (Carrot.BaseType == E_BaseType.UseItem)
            {
                UseItem useitem = new UseItem(Carrot as UseItem);
                inventory.CombineSlot<UseItem>(useitem);
            }
            else if (Carrot.BaseType == E_BaseType.WeaponItem)
            {
                WeaponItem useitem = new WeaponItem(Carrot as WeaponItem);
                inventory.CombineSlot<WeaponItem>(useitem);
            }
            else if (Carrot.BaseType == E_BaseType.ArmorItem)
            {
                Debug.Log("작동");
                ArmorItem useitem = new ArmorItem(Carrot as ArmorItem);
                inventory.CombineSlot<ArmorItem>(useitem);
            }
        }
    }
    public void OpenChest()
    {
        Instantiate(Prefabs);
    }
    private ItemDataSo[] RandomItem()
    {
        int randomLength = Random.Range(1, 5);
        itemDatas = new ItemDataSo[15];
        int i = 0;
        for (; i < 15; i++)
        {
            itemDatas[i] = new ItemDataSo();
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
