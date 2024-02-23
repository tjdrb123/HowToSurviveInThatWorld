using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

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
            else if (Carrot.BaseType == E_BaseType.WeaponItem || Carrot.BaseType == E_BaseType.SubWeaponItem)
            {
                WeaponItem useitem = new WeaponItem(Carrot as WeaponItem);
                inventory.CombineSlot<WeaponItem>(useitem);
            }
            else if (Carrot.BaseType == E_BaseType.ArmorItem)
            {
                ArmorItem useitem = new ArmorItem(Carrot as ArmorItem);
                inventory.CombineSlot<ArmorItem>(useitem);
            }
            else
            {
                Manager_Inventory.Instance.Additem(Carrot, 2);
                //EtcItem etcItem = new EtcItem(Carrot as EtcItem);
                //inventory.CombineSlot<EtcItem>(etcItem);
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
    private void OnCollisionEnter(Collision collision)
    {
        Manager_Inventory.Instance.ChestItemDatas = itemDatas;
        button.gameObject.SetActive(true);
    }
    private void OnCollisionExit(Collision collision)
    {
        button.gameObject.SetActive(false);
    }
}
