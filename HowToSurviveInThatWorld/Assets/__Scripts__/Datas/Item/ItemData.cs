using System;
using System.Collections.Generic;

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
    
    public string GetKey()
    {
        return name;
    }
}
