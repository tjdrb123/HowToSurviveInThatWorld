using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class item : MonoBehaviour
{
    public EtcItem useItem;
    public void ItemPlus()
    {
        Manager_Inventory.Instance.Additem(useItem, 20);
    }
}
