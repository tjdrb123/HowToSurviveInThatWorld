using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class item : MonoBehaviour
{
    public UseItem weapon;
    public void ItemPlus()
    {
        Manager_Inventory.Instance.BaseSlotDatas[0] = weapon;
    }
}
