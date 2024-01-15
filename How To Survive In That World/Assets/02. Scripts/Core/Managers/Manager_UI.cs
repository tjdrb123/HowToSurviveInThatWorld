using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Manager_UI 
{
    private static readonly int InitialPopupOrder = 10; //Pop_UP UI의 sortingOrder 기본 값입니다.

    private List<UI_Popup> _popups = new();

    public GameObject Root //UI의 부모를 만들어서 이안에서 관리를 합니다.
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }
    public void SetCanvas(GameObject obj) //Canvas의 기본 정보를 최신화시킵니다.
    { 
        Canvas canvas = obj.GetOrAddComponent<Canvas>(); //UI의 최상위 오브젝트의 Canvas를 가져옴 Ex)UI_CursorSlot, UI_GameScene
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        CanvasScaler scaler = obj.GetOrAddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new(2560, 1440);
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup //이 함수를 통해 Pop_Up을 실행시킬 수 있습니다.
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject obj = new GameObject(name); //new GameObject가 아닌 Prefab을 넣어주면 됩니다.
        obj.transform.SetParent(Root.transform);
        T popup = obj.GetOrAddComponent<T>();
        _popups.Add(popup);

        return popup;
    }
    //public void ClosePopup(UI_Popup popup) //이 함수를 통해 Pop_Up을 종료시킬 수 있습니다.
    //{
    //    if (_popups.Count == 0) return;

    //    bool isLatest = _popups[_popups.Count - 1] == popup;

    //    _popups.Remove(popup);
    //    Main.Resource.Destroy(popup.gameObject);

    //    if (isLatest) _popupOrder--;
    //    else ReorderAllPopups();
    //}
}
