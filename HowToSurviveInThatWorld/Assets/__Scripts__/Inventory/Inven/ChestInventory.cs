using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ChestInventory : MonoBehaviour
{
    [SerializeField] private ItemDataSo[] _items = new ItemDataSo[15];
    private bool isFrist = true;
    private void Start()
    {
        if (isFrist)
        {
            isFrist = false;
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] != null)
                {
                    if (_items[i].BaseType != E_BaseType.WeaponItem && _items[i].BaseType != E_BaseType.SubWeaponItem)
                    {
                        int randomIndex = Random.Range(1, 5);
                        var newItem = _items[i].BaseType == E_BaseType.UseItem ? new UseItem(_items[i] as UseItem) : new EtcItem(_items[i] as EtcItem);
                        newItem.CurrentAmont = randomIndex;
                        _items[i] = newItem;
                    }
                    else
                    {
                        var newItem = new WeaponItem(_items[i] as WeaponItem);
                        _items[i] = newItem;
                    }
                }
            }
            Manager_Inventory.Instance.ChestItemDatas = _items;
        }
    }
}
