using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : UI_Popup
{
    enum E_Images
    {
        ItemImage,
    }
    enum E_QuantityText
    {
        ItemQuantity,
    }
    public ItemSlot slot;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        SetOrder();
        BindImage(typeof(E_Images));
        BindText(typeof(E_QuantityText));

        return true;
    }

    public void SetSlot(Image slotSprite, TextMeshProUGUI quantityText, ItemSlot slot)
    {
        GetImage((int)E_Images.ItemImage).sprite = slotSprite.sprite;
        GetText((int)E_QuantityText.ItemQuantity).text = quantityText.text;
        this.slot = slot;
    }
}
