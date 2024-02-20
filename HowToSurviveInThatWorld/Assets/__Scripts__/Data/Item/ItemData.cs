using System;
using System.Collections.Generic;
using static Unity.VisualScripting.Member;

public interface IKeyHolder
{
    string GetKey();
}
[Serializable]
public class ItemData : IKeyHolder
{
    public int keyNumber;
    public string name;
    public string description;
    public int itemBaseType;
    public int stack;
    public int maxStack;
    public ItemData() 
    {
        keyNumber = 0;
        name = "";
        description = "";
        itemBaseType = -1;
        stack = -1;
        maxStack = -1;
    }
    public ItemData(ItemData source)
    {
        keyNumber = source.keyNumber;
        name = source.name;
        description = source.description;
        itemBaseType = source.itemBaseType;
        stack = source.stack;
        maxStack = source.maxStack;
    }

    public string GetKey()
    {
        return name;
    }
}
