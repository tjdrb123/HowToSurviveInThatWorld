
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class JoystickStrategy
{
    #region Fields

    protected readonly InputUIJoystick _Joystick;
    protected readonly RectTransform _Self;
    protected Vector2 _startingPosition;

    public event Action<Vector2> OnSendValueToControl;

    #endregion



    #region Constructor

    protected JoystickStrategy(InputUIJoystick joystick)
    {
        if (joystick == null)
        {
            DebugLogger.LogError("Joystick Base is null.");
            return;
        }

        // Caching
        _Joystick = joystick;
        _Self = _Joystick.SelfRect;
    }

    #endregion



    #region Event Processing

    protected void RaiseOnSendValueToControl(Vector2 value)
    {
        OnSendValueToControl?.Invoke(value);
    }

    #endregion



    #region Abstract & Virtual

    public abstract void PointerDownInteraction(PointerEventData eventData);
    public abstract void PointerUpInteraction(PointerEventData eventData);
    public abstract void OnDragInteraction(PointerEventData eventData);

    public virtual void SetMode(InputUIJoystick.E_JoystickType joystickType, Action SetSelfRectMethod)
    {
        _Joystick.JoystickType = joystickType;

        // SelfRect Changed.
        SetSelfRectMethod.Invoke();
    }

    #endregion
}
