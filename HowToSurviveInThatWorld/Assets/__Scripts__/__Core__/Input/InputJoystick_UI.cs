
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[AddComponentMenu("Input/Input Joystick UI")]
public class InputJoystick_UI : InputJoystickBase
{
    #region Fields

    [SerializeField] private E_JoystickType _JoystickType = E_JoystickType.Floating;
    private Vector2 _fixedPosition = Vector2.zero;

    #endregion



    #region Override

    protected override void InitializeStart()
    {
        base.InitializeStart();
        
        _fixedPosition = _backGroundRect.anchoredPosition;
        SetJoystickMode(_JoystickType);
    }

    #endregion



    #region Setup Mode

    public void SetJoystickMode(E_JoystickType joystickType)
    {
        _JoystickType = joystickType;
        if (_JoystickType == E_JoystickType.Fixed)
        {
            _backGroundRect.anchoredPosition = _fixedPosition;
            _backGroundRect.gameObject.SetActive(true);
        }
        else
        {
            _backGroundRect.gameObject.SetActive(false);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_JoystickType != E_JoystickType.Fixed)
        {
            _backGroundRect.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            _backGroundRect.gameObject.SetActive(true);
        }
        
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (_JoystickType != E_JoystickType.Fixed)
        {
            _backGroundRect.gameObject.SetActive(false);
        }
        
        base.OnPointerUp(eventData);
    }

    #endregion
}
