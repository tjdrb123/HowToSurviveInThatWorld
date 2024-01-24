using UnityEngine;

public class UserName
{
    public string Value { get; private set; }

    public UserName(string setValue)
    {
        Value = setValue;
    }
    
    public void StringValueChangedHandle(string newValue)
    {
        Value = newValue;
    }
}