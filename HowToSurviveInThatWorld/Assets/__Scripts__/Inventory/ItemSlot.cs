using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : UI_Popup
{
    enum E_Images
    {
        ItemImage,
    }
    enum E_DragSlot
    {
        DragSlot,
    }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindImage(typeof(E_Images));

        return true;
    }

    public void SetInfo()
    {
        Initialize();
        //this.Item = item;
        //GetImage((int)E_Images.ItemImage).sprite = 
    }
}
