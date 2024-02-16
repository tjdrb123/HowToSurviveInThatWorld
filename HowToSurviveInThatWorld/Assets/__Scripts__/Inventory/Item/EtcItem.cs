using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtcItem : ItemDataSo
{
    public int MaxStack; //얼마정도로 쌓을것인지
    public EtcItem(EtcItem etcItem) : base(etcItem)
    {
        MaxStack = etcItem.MaxStack;
    }
}
