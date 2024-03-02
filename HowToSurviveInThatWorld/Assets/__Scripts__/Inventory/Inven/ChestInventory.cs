using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ChestInventory : MonoBehaviour
{
    [SerializeField] private ItemDataSo[] _items = new ItemDataSo[15];
    [SerializeField] private int RandomMax = 2;
    private bool isFrist = true;
    public void ChestData()
    {
        if (isFrist)
        {
            isFrist = false;
            ItemDataSo[] item = new ItemDataSo[15];
            for (int i = 0; i < _items.Length; i++)
            {
                int randomItem = Random.Range(0, RandomMax);

                if (randomItem == 0)
                {
                    if (_items[i] != null)
                    {
                        if (_items[i].BaseType != E_BaseType.WeaponItem && _items[i].BaseType != E_BaseType.SubWeaponItem)
                        {
                            int randomIndex = Random.Range(1, 5);
                            var newItem = _items[i].BaseType == E_BaseType.UseItem ? new UseItem(_items[i] as UseItem) : new EtcItem(_items[i] as EtcItem);
                            newItem.CurrentAmont = randomIndex;
                            item[i] = newItem;
                        }
                        else
                        {
                            var newItem = new WeaponItem(_items[i] as WeaponItem);
                            item[i] = newItem;
                        }
                    }
                }
                else
                    item[i] = new ItemDataSo();
            }
            Manager_Inventory.Instance.ChestItemDatas = item;
        }
    }
}
