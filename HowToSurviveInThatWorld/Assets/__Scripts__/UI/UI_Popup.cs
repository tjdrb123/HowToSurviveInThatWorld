using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    protected Canvas _canvas;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        this.SetCanvas();//캔버스 초기셋팅
        _canvas = this.GetComponent<Canvas>();

        return true;
    }
    protected override void SetOrder() => _canvas.sortingOrder = 10;
    public virtual void ClosePopup() => Managers.UI.ClosePopup(this);
}
