using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipments : MonoBehaviour
{
    private Inventory _inventory;
    
    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    private void EquipCheck()
    {
        for (int i = 0; i < _inventory.BaseSlot.Length; i++)
        {
            ItemSlot slot = _inventory.BaseSlot[i];
            if (slot.ItemData.KeyNumber != 0 && slot.ItemData.BaseType == E_BaseType.ArmorItem)
            {

            }
            else if (slot.ItemData.KeyNumber != 0 && slot.ItemData.BaseType == E_BaseType.WeaponItem)
            {
                      
            }
        }
    }
}
