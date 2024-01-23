
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class InputJoystickBase : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    #region Fields (OnScreenControl)

    /// <summary>
    /// # 오버라이드(재정의)할 컨트롤 인풋
    ///   - Ex) Keyboard or Gamepad(Left Stick) 같은 것을 의미
    /// </summary>
    [InputControl(layout = "Vector2")] [SerializeField] private string _ControlPath;
    
    /* Property */
    protected override string controlPathInternal
    {
        get => _ControlPath;
        set => _ControlPath = value;
    }
    
    #endregion



    #region Fields (Joystick)

    [SerializeField] private float _HandleRange = Literals.ONE_F;
    [SerializeField] private float _DeadZone = Literals.ZERO_F;

    protected RectTransform _backGroundRect;
    private RectTransform _handleRect;
    protected RectTransform _areaRect;

    private Canvas _canvas;
    private Camera _uiCamera;
    
    /* Property */
    public float HandleRange
    {
        get => _HandleRange;
        set => _HandleRange = Mathf.Abs(value);
    }

    public float DeadZone
    {
        get => _DeadZone;
        set => _DeadZone = Mathf.Abs(value);
    }

    #endregion



    #region Unity Behavior

    private void Awake()
    {
        InitializeAwake();
    }

    private void Start()
    {
        InitializeStart();
    }

    #endregion



    #region Initialize

    /// <summary>
    /// # Awake 이벤트 메서드에서 실행될 메서드
    /// </summary>
    protected virtual void InitializeAwake()
    {
        InitComponentsRectCanvas();
        InitComponentsJoystick();
    }

    private void InitComponentsRectCanvas()
    {
        _areaRect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        
        if (_canvas == null)
        {
            DebugLogger.LogError("Joystick is not placed inside a canvas.");
        }
    }

    private void InitComponentsJoystick()
    {
        const int ZERO = (int)Literals.ZERO_F;
        if (transform.childCount == ZERO)
        {
            DebugLogger.LogError($"{transform.name} don't have Children.");
            return;
        }

        Transform joystickBg = transform.GetChild(ZERO);
        Transform joystickHandle = null;

        if (joystickBg.childCount != ZERO)
        {
            joystickHandle = joystickBg.GetChild(ZERO);
        }

        _backGroundRect = joystickBg.GetComponent<RectTransform>();
        
        if (joystickHandle != null)
        {
            _handleRect = joystickHandle.GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// # Start 이벤트 메서드에서 실행될 메서드
    /// </summary>
    protected virtual void InitializeStart()
    {
        Vector2 center = new Vector2(Literals.HALF_F, Literals.HALF_F);

        HandleRange = _HandleRange;
        DeadZone = _DeadZone;
        
        _backGroundRect.pivot = center;
        
        // 핸들이 존재하지 않을 경우 다음 사항을 할 필요가 없음.
        if (_handleRect == null) return;
        
        _handleRect.anchorMin = center;
        _handleRect.anchorMax = center;
        _handleRect.pivot = center;
        _handleRect.anchoredPosition = Vector2.zero;
    }

    #endregion



    #region Interface Implement Methods

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }
        
        BeginInteraction(eventData.position);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        EndInteraction();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }
        
        MoveStick(eventData.position);
    }

    #endregion



    #region Interaction

    private void BeginInteraction(Vector2 pointerPosition)
    {
        MoveStick(pointerPosition);
    }

    /// <summary>
    /// # 사용자 인터랙션이 동작중일 때
    ///   - 실질적으로 Input값을 도출하고,
    ///   - Handle 또는 Background를 움직인다.
    /// </summary>
    /// <param name="pointerPosition"></param>
    private void MoveStick(Vector2 pointerPosition)
    {
        _uiCamera = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            _uiCamera = _canvas.worldCamera;
        }

        Vector2 position = RectTransformUtility.WorldToScreenPoint(_uiCamera, _backGroundRect.position);
        Vector2 radius = _backGroundRect.sizeDelta / Literals.TWO_F;
        Vector2 input = (pointerPosition - position) / (radius * _canvas.scaleFactor);
        
        HandleInput(ref input);
        
        // 실질적인 핸들에 목표 포지션을 변경 (포지션으로 핸들이 이동 되는 연출)
        if (_handleRect != null)
        {
            _handleRect.anchoredPosition = input * radius * _HandleRange;
        }

        SendValueToControl(input);
    }

    /// <summary>
    /// # Pointer Up
    ///   - 사용자 입력 인터랙션이 종료 됐을 경우
    /// </summary>
    private void EndInteraction()
    {
        if (_handleRect != null)
        {
            _handleRect.anchoredPosition = Vector2.zero;
        }
        else
        {
            _backGroundRect.anchoredPosition = Vector2.zero;
        }

        SendValueToControl(Vector2.zero);
    }

    #endregion



    #region Handle Joystick

    /// <summary>
    /// # Input 값에 대한 정규화 처리 메서드 (ref 처리함으로써 참조를 보냄)
    ///   - Magnitude가 설정한 DeadZone보다 크고,
    ///   - 1f(Literals.ONE_F)보다 클 경우 정규화 처리
    ///   - Magnitude가 데드존보다 작거나 같을 경우는 Vector2.zero 값을 지니게 함
    /// </summary>
    private void HandleInput(ref Vector2 input)
    {
        float magnitude = input.magnitude;
        
        if (magnitude > _DeadZone)
        {
            if (magnitude > Literals.ONE_F)
            {
                input = input.normalized;
            }
        }
        else
        {
            input = Vector2.zero;
        }
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _areaRect, screenPosition, _uiCamera, out var localPosition))
        {
            Vector2 sizeDelta;
            Vector2 pivotOffset = _areaRect.pivot * (sizeDelta = _areaRect.sizeDelta);

            return localPosition - (_backGroundRect.anchorMax * sizeDelta) + pivotOffset;
        }

        return Vector2.zero;
    }

    #endregion
}