using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    protected Canvas _canvas;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        //this.SetCanvas();//캔버스 초기셋팅
        _canvas = this.GetComponent<Canvas>();

        return true;
    }
    protected override void SetOrder() => _canvas.sortingOrder = 10;

    //Close시 작동 할 로직 추가해야함
    //Pop_Up창을 어떤식으로 할지 논의 필요
}
