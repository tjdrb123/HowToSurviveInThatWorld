
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick_HandleAndBg : JoystickStrategy
{
    #region Fields
    
    private readonly RectTransform _Handle;
    private readonly RectTransform _Background;

    public float HandleRange = 0.7f;
    public Vector2 Radius;

    #endregion



    #region Constructor

    public Joystick_HandleAndBg(InputUIJoystick joystick) : base(joystick)
    {
        var rects = _Joystick.RectTransforms;
        
        // Handle
        if (!rects.TryGetValue(InputUIJoystick.E_JoystickModularName.Handle, out _Handle))
        {
            DebugLogger.LogError("Handle not found in RectTransforms");
            return;
        }

        // Background
        if (!rects.TryGetValue(InputUIJoystick.E_JoystickModularName.Background, out _Background))
        {
            DebugLogger.LogError("Background not found in RectTransforms");
            return;
        }

        // Setup Starting Position
        _startingPosition = _Handle.anchoredPosition;
        
        // Setup Radius
        Radius = _Background.sizeDelta / Literals.TWO_F;
    }
    
    #endregion



    #region Override

    public override void PointerDownInteraction(PointerEventData eventData)
    {
        if (_Joystick.JoystickType != InputUIJoystick.E_JoystickType.Fixed)
        {
            _Background.anchoredPosition = _Joystick.AnchoredFingerPosition;
            _Handle.anchoredPosition = _Joystick.AnchoredFingerPosition;
            _Background.gameObject.SetActive(true);
            _Handle.gameObject.SetActive(true);
        }

        OnDragInteraction(eventData);
    }

    public override void PointerUpInteraction(PointerEventData eventData)
    {
        if (_Joystick.JoystickType != InputUIJoystick.E_JoystickType.Fixed)
        {
            _Background.gameObject.SetActive(false);
            _Handle.gameObject.SetActive(false);
        }

        _Background.anchoredPosition = _startingPosition;
        _Handle.anchoredPosition = _startingPosition;
        RaiseOnSendValueToControl(Vector2.zero);
    }

    public override void OnDragInteraction(PointerEventData eventData)
    {
        Action moveStickMethod = _Joystick.JoystickType switch
        {
            InputUIJoystick.E_JoystickType.Fixed => FixedMoveStick,
            InputUIJoystick.E_JoystickType.Floating => FloatingMoveStick,
            _ => throw new ArgumentOutOfRangeException(nameof(_Joystick.JoystickType))
        };

        moveStickMethod.Invoke();
    }

    #endregion
    
    
    
    #region Process

    private void FixedMoveStick()
    {
        var anchoredFingerPos = 
            _Joystick.ScreenPointToAnchoredPosition(_Joystick.FingerPosition, _Joystick.RootRect);
        var delta = anchoredFingerPos - _startingPosition;

        delta = Vector2.ClampMagnitude(delta, Radius.x * HandleRange);
        _Handle.anchoredPosition = _startingPosition + delta;

        SendToDirectionValue(delta);
    }

    private void FloatingMoveStick()
    {
        var anchoredFingerPos = _Joystick.AnchoredFingerPosition;
        var anchoredInitFingerPos = _Joystick.AnchoredInitialFingerPosition;
        var delta = anchoredFingerPos - anchoredInitFingerPos;

        delta = Vector2.ClampMagnitude(delta, Radius.x * HandleRange);
        _Handle.anchoredPosition = _Background.anchoredPosition + delta;

        SendToDirectionValue(delta);
    }

    private void SendToDirectionValue(Vector2 delta)
    {
        float radiusHandleX = (Radius.x * HandleRange);
        float radiusHandleY = (Radius.y * HandleRange);
        Vector2 direction = new Vector2(delta.x / radiusHandleX, delta.y / radiusHandleY).normalized;
        
        RaiseOnSendValueToControl(direction);
    }

    #endregion



    #region Utils

    public override void SetMode(InputUIJoystick.E_JoystickType joystickType, Action SetSelfRectMethod)
    {
        base.SetMode(joystickType, SetSelfRectMethod);

        switch (_Joystick.JoystickType)
        {
            case InputUIJoystick.E_JoystickType.Fixed:
                _Handle.anchoredPosition = _startingPosition;
                _Background.anchoredPosition = _startingPosition;
                _Handle.gameObject.SetActive(true);
                _Background.gameObject.SetActive(true);
                break;
            case InputUIJoystick.E_JoystickType.Floating:
                _Handle.gameObject.SetActive(false);
                _Background.gameObject.SetActive(false);
                break;
        }
    }

    #endregion
}
