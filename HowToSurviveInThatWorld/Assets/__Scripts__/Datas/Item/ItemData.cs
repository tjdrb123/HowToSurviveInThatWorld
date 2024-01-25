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
    public int KeyNumber;
    public string Name;
    public string Description;
    public int ItmeBaseType;
    
    public string GetKey()
    {
        return Name;
    }
}
