
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick_All : JoystickStrategy
{
    #region Fields

    private readonly RectTransform _Handle;
    private readonly RectTransform _Direction;
    private readonly RectTransform _Background;

    public float HandleRange = 0.7f;
    public Vector2 Radius;

    #endregion
    public Joystick_All(InputUIJoystick joystick) : base(joystick)
    {
        var rects = _Joystick.RectTransforms;
        
        // Handle
        if (!rects.TryGetValue(InputUIJoystick.E_JoystickModularName.Handle, out _Handle))
        {
            DebugLogger.LogError("Handle not found in RectTransforms");
            return;
        }

        // Direction
        if (!rects.TryGetValue(InputUIJoystick.E_JoystickModularName.Direction, out _Direction))
        {
            DebugLogger.LogError("Background not found in RectTransforms");
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



    #region Override

    public override void PointerDownInteraction(PointerEventData eventData)
    {
        if (_Joystick.JoystickType != InputUIJoystick.E_JoystickType.Fixed)
        {
            SetAllAnchoredPosition(_Joystick.AnchoredFingerPosition);
            SetActiveAll(true);
        }
        
        OnDragInteraction(eventData);
    }

    public override void PointerUpInteraction(PointerEventData eventData)
    {
        if (_Joystick.JoystickType != InputUIJoystick.E_JoystickType.Fixed)
        {
            SetActiveAll(false);
        }
        
        _Direction.up = Vector3.zero;
        SetAllAnchoredPosition(_startingPosition);
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
        RotateDirection(anchoredFingerPos);

        delta = Vector2.ClampMagnitude(delta, Radius.x * HandleRange);
        _Handle.anchoredPosition = _startingPosition + delta;

        SendToDirectionValue(delta);
    }

    private void FloatingMoveStick()
    {
        var anchoredFingerPos = _Joystick.AnchoredFingerPosition;
        var anchoredInitFingerPos = _Joystick.AnchoredInitialFingerPosition;
        var delta = anchoredFingerPos - anchoredInitFingerPos;
        RotateDirection(anchoredFingerPos);

        delta = Vector2.ClampMagnitude(delta, Radius.x * HandleRange);
        _Handle.anchoredPosition = _Background.anchoredPosition + delta;

        SendToDirectionValue(delta);
    }
    
    private void RotateDirection(Vector2 anchoredFingerPos)
    {
        Vector2 directionVector;
        if (_Joystick.JoystickType == InputUIJoystick.E_JoystickType.Floating)
        {
            // Floating 모드에서는 Background를 기준으로 회전 계산
            directionVector = anchoredFingerPos - _Background.anchoredPosition;
        }
        else
        {
            // Fixed 모드에서는 기존 방식대로 계산
            directionVector = anchoredFingerPos - _startingPosition;
        }

        Vector2 normalizedDirection = directionVector.normalized;
        _Direction.up = normalizedDirection;
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

    private void SetActiveAll(bool isActive)
    {
        _Handle.gameObject.SetActive(isActive);
        _Direction.gameObject.SetActive(isActive);
        _Background.gameObject.SetActive(isActive);
    }

    private void SetAllAnchoredPosition(Vector2 anchoredPosition)
    {
        _Handle.anchoredPosition = anchoredPosition;
        _Direction.anchoredPosition = anchoredPosition;
        _Background.anchoredPosition = anchoredPosition;
    }

    public override void SetMode(InputUIJoystick.E_JoystickType joystickType, Action SetSelfRectMethod)
    {
        base.SetMode(joystickType, SetSelfRectMethod);

        _Direction.up = Vector3.zero;

        switch (_Joystick.JoystickType)
        {
            case InputUIJoystick.E_JoystickType.Fixed:
                SetAllAnchoredPosition(_startingPosition);
                SetActiveAll(true);
                break;
            case InputUIJoystick.E_JoystickType.Floating:
                SetActiveAll(false);
                break;
        }
    }

    #endregion
}
