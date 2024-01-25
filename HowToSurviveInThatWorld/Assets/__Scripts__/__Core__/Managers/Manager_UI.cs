using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Manager_UI : MonoBehaviour 
{
    private static readonly int InitialPopupOrder = 10; //Pop_UP UI의 sortingOrder 기본 값입니다.

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

    #region SceneUI

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene //신에 기본으로 떠있는 UI를 생성합니다.
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject obj = Resources.Load($"{name}") as GameObject; //스크립트의 이름과 동일하게 Prefab의 이름을 만들어줘 불러온다.
        Instantiate(obj);
        obj.transform.SetParent(Root.transform);

        return obj.GetOrAddComponent<T>();
    }

    #endregion

    #region Popups

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup //이 함수를 통해 Pop_Up을 실행시킬 수 있습니다.
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        Debug.Log(name);

        GameObject obj = Resources.Load($"{name}") as GameObject; //스크립트의 이름과 동일하게 Prefab의 이름을 만들어줘 불러온다.
        Instantiate(obj);
        obj.transform.SetParent(Root.transform);
        T popup = obj.GetOrAddComponent<T>();

        return popup;
    }
    public void ClosePopup(UI_Popup popup) //이 함수를 통해 Pop_Up을 종료시킬 수 있습니다.
    {
        UnityEngine.Object.Destroy(popup.gameObject);
    }

    #endregion
}
