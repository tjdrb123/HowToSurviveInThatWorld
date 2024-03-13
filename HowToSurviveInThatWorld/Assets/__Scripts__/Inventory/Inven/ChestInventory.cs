using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ChestInventory : MonoBehaviour
{
    [SerializeField] private ItemDataSo[] _items;
    [SerializeField] private int RandomMax = 2;
    private bool isFrist = true;
    public void ChestData()
    {
        Manager_Inventory.Instance.ChestInventory = this;
        if (isFrist)
        {
            isFrist = false;
            ItemDataSo[] item = new ItemDataSo[15];
            int count = 0;
            for (int i = 0; i < _items.Length; i++)
            {
                int randomItem = UnityEngine.Random.Range(0, RandomMax);
                if (randomItem == 0)
                {
                    if (_items[i] != null)
                    {
                        if (_items[i].BaseType == E_BaseType.UseItem || _items[i].BaseType == E_BaseType.EtcItem)
                        {
                            int randomIndex = UnityEngine.Random.Range(1, 5);
                            var newItem = _items[i].BaseType == E_BaseType.UseItem ? new UseItem(_items[i] as UseItem) : new EtcItem(_items[i] as EtcItem);
                            newItem.CurrentAmont = randomIndex;
                            item[count] = newItem;
                        }
                        else if (_items[i].BaseType == E_BaseType.WeaponItem || _items[i].BaseType == E_BaseType.SubWeaponItem)
                        {
                            var newItem = new WeaponItem(_items[i] as WeaponItem);
                            item[count] = newItem;
                        }
                        else
                        {
                            var newItem = new ArmorItem(_items[i] as ArmorItem);
                            item[count] = newItem;
                        }
                        count++;
                    }
                }
            }
            for (int j = item.Length;  j < 15; j++)
            {
                item[count] = new ItemDataSo();
            }
            Manager_Inventory.Instance.ChestItemDatas = item;
            return;
        }
        Manager_Inventory.Instance.ChestItemDatas = _items;
    }
    public void ChestSetData()
    {
        _items = Manager_Inventory.Instance.ChestItemDatas;
    }
}
