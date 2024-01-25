
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick_DirectionAndBg : JoystickStrategy
{
    #region Fields
    
    private readonly RectTransform _Direction;
    private readonly RectTransform _Background;

    #endregion
    
    
    
    #region Constructor & Initialize
    
    public Joystick_DirectionAndBg(InputUIJoystick joystick) : base(joystick)
    {
        var rects = _Joystick.RectTransforms;
        
        // Direction
        if (!rects.TryGetValue(InputUIJoystick.E_JoystickModularName.Direction, out _Direction))
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
        _startingPosition = _Direction.anchoredPosition;
    }
    
    #endregion



    #region Override

    public override void PointerDownInteraction(PointerEventData eventData)
    {
        if (_Joystick.JoystickType != InputUIJoystick.E_JoystickType.Fixed)
        {
            _Background.anchoredPosition = _Joystick.AnchoredFingerPosition;
            _Direction.anchoredPosition = _Joystick.AnchoredFingerPosition;
            _Background.gameObject.SetActive(true);
            _Direction.gameObject.SetActive(true);
        }

        OnDragInteraction(eventData);
    }

    public override void PointerUpInteraction(PointerEventData eventData)
    {
        if (_Joystick.JoystickType != InputUIJoystick.E_JoystickType.Fixed)
        {
            _Background.gameObject.SetActive(false);
            _Direction.gameObject.SetActive(false);
        }

        _Background.anchoredPosition = _startingPosition;
        _Direction.anchoredPosition = _startingPosition;
        _Direction.up = Vector3.zero;
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
        RotateDirection(anchoredFingerPos);
    }

    private void FloatingMoveStick()
    {
        var anchoredFingerPos = _Joystick.AnchoredFingerPosition;
        RotateDirection(anchoredFingerPos);
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
        
        RaiseOnSendValueToControl(normalizedDirection);
    }

    #endregion



    #region Utils

    public override void SetMode(InputUIJoystick.E_JoystickType joystickType, Action SetSelfRectMethod)
    {
        base.SetMode(joystickType, SetSelfRectMethod);

        _Direction.up = Vector3.zero;

        switch (_Joystick.JoystickType)
        {
            case InputUIJoystick.E_JoystickType.Fixed:
                _Direction.anchoredPosition = _startingPosition;
                _Background.anchoredPosition = _startingPosition;
                _Direction.gameObject.SetActive(true);
                _Background.gameObject.SetActive(true);
                break;
            case InputUIJoystick.E_JoystickType.Floating:
                _Direction.gameObject.SetActive(false);
                _Background.gameObject.SetActive(false);
                break;
        }
    }

    #endregion
}
