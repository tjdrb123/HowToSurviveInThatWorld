
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

public class InputUIButton : InputUIBase
{
    #region Override Fields

    [InputControl(layout = "Button")] [SerializeField]
    private string _ControlPath;

    protected override string ControlPath
    {
        get => _ControlPath;
        set => _ControlPath = value;
    }

    #endregion
    
    protected override void Initialize()
    {
        
    }

    protected override void PointerDownInteraction(PointerEventData eventData)
    {
        
    }

    protected override void PointerUpInteraction(PointerEventData eventData)
    {
        
    }
}