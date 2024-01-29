using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Inventory : MonoBehaviour
{
    public ItemData[] BaseSlotDatas;
    public ItemData[] BackPackSlotDatas;
    public ItemData[] EquipMentSlotDatas;
    public ItemData[] ChestItemDatas;

    public static Manager_Inventory Instance;

    private void Awake()
    {
        Instance = this;
        BaseSlotDatas = new ItemData[15];
        BackPackSlotDatas = new ItemData[15];
        ChestItemDatas = new ItemData[15];
        EquipMentSlotDatas = new ItemData[8];
        for (int i = 0; i < 15; i++)
        {
            BaseSlotDatas[i] = new ItemData();
            BackPackSlotDatas[i] = new ItemData();
            ChestItemDatas[i] = new ItemData();
            if (i < 8)
            {
                EquipMentSlotDatas[i] = new ItemData();
            }
        }
    }
}
