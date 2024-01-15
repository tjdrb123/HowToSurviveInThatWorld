using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        //this.SetCanvas(); GameManager가 만들어지면 사용하여 초기화
        SetOrder();

        return true;
    }

    protected override void SetOrder() //신에 기본으로 깔리는 UI기 때문에 PopUp 보다 낮게 0으로 지정해준다.
    {
        this.GetComponent<Canvas>().sortingOrder = 0;
    }
}
