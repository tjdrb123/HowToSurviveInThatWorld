using System;
using System.Collections.Generic;

public interface IKeyHolder
{
    string GetKey();
}

public enum E_ItemType
{
    None = -1,
    Helmet,
    Shirt,
    Trousers,
    Boots,
    BackPack,
    Weapon,
    Sub_Weapon,
    Quick
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
