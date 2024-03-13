
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

public class InputUIJoystick : InputUIBase, IDragHandler
{
    #region Override Fields

    [InputControl(layout = "Vector2")] [SerializeField]
    private string _ControlPath;

    protected override string ControlPath
    {
        get => _ControlPath;
        set => _ControlPath = value;
    }

    #endregion



    #region Fields

    /* ======================= Enum ======================= */
    public enum E_JoystickType
    {
        Fixed,
        Floating
    }
    public enum E_JoystickModularName
    {
        Background,
        Direction,
        Handle
    }
    
    public enum E_JoystickHandleType
    {
        OnlyHandle,             // 이미지가 하나만 존재 할 경우
        HandleAndBg,            // 이미지가 두 개 존재할 경우 - 배경과 핸들
        DirectionAndBg,         // 이미지가 두 개 존재할 경우 - 배경과 디렉션
        HandleAndBgDirection    // 이미지가 세 개 존재할 경우
    }

    /* ======================= Parameters ======================= */
    // JoystickHandleType -> 자동으로 결정 됌
    // JoystickType -> 플레이어가 설정 가능한 조이스틱 타입
    public E_JoystickHandleType JoystickHandleType;
    public E_JoystickType JoystickType = E_JoystickType.Floating;
    
    // 1. 해당 모듈라 이름으로 지정된 오브젝트가 존재하는지 판단.
    // 2. 존재 하면 해당 모듈라 이름에 맞는 RectTransform을 딕셔너리에 삽입
    //   - 최대 3개를 지닐 수 있다. (Enum 개수만큼)
    //   - 1 ~ 3개에 따라 자동으로 JoystickHandleType을 결정하는 용도
    public Dictionary<E_JoystickModularName, RectTransform> RectTransforms;

    // Strategy Pattern을 사용. 각 HandleType에 맞는 내용을 실행함.
    private JoystickStrategy _joystick;
    
    // 디렉션 사용 여부 (자동으로 인식 됌, 건들 필요 없음)
    private bool _isUseDirection;
    
    // Anchored Finger
    public Vector2 AnchoredInitialFingerPosition;
    public Vector2 AnchoredFingerPosition;

    #endregion



    #region Initialize

    protected override void Initialize()
    {
        // Joystick Setup
        SetupJoystick();
        CreateJoystick();
        SetJoystickMode(JoystickType);
    }
    
    private void SetupJoystick()
    {
        SetupChildRectTransformComponents();
        SetupJoystickHandleType();
        SetJoystickSelfRect();
    }

    /// <summary>
    /// # 셋팅된 조이스틱에 맞는 타입에 조이스틱을 만드는 메서드
    ///   - JoystickStrategyFactory는 팩토리 패턴을 사용.
    /// </summary>
    private void CreateJoystick()
    {
        _joystick = JoystickStrategyFactory.CreateStrategy(JoystickHandleType, this);
    }

    #endregion



    #region Setup Joystick

    /// <summary>
    /// # E_JoystickModularName을 지닌 객체가 자식중에 존재하는지 확인
    ///   - 확인이 되면 해당 객체로부터 RectTransform을 들고옴.
    ///   - 들고온 RectTransform은 해당 `키`에 맞게 밸류로 저장
    /// </summary>
    private void SetupChildRectTransformComponents()
    {
        // RectTransforms가 초기화 되지 않았을 경우 초기화를 시켜줌
        RectTransforms ??= new Dictionary<E_JoystickModularName, RectTransform>();
        
        foreach (Transform child in SelfRect)
        {
            if (Enum.TryParse(child.gameObject.name, out E_JoystickModularName modularName))
            {
                RectTransforms[modularName] = child.GetComponent<RectTransform>();
            }
        }

        if (SelfRect.childCount > 3)
        {
            DebugLogger.LogWarning(
                "Joystick has more than three child objects. Only Background, Direction, and Handle are expected.");
        }
    }

    /// <summary>
    /// # Joystick Handle Type을 셋팅하는 메서드
    /// </summary>
    private void SetupJoystickHandleType()
    {
        bool hasHandle = RectTransforms.ContainsKey(E_JoystickModularName.Handle);
        bool hasBackground = RectTransforms.ContainsKey(E_JoystickModularName.Background);
        bool hasDirection = RectTransforms.ContainsKey(E_JoystickModularName.Direction);
        
        // Boolean Has 조합을 통해 HandleType을 결정
        JoystickHandleType = DetermineJoystickType(hasHandle, hasBackground, hasDirection);
        
        SetupIsUseDirection(JoystickHandleType is 
            E_JoystickHandleType.DirectionAndBg or E_JoystickHandleType.HandleAndBgDirection);
    }
    
    /// <summary>
    /// # Joystick Handle Type을 결정해서 반환해주는 메서드
    /// </summary>
    /// <param name="hasHandle">핸들이 존재하는지</param>
    /// <param name="hasBackground">백그라운드가 존재하는지</param>
    /// <param name="hasDirection">디렉션이 존재하는지</param>
    private E_JoystickHandleType DetermineJoystickType(bool hasHandle, bool hasBackground, bool hasDirection)
    {
        // Tuple로 결합하여 사용
        return (hasHandle, hasBackground, hasDirection) switch
        {
            (true, true, true) => E_JoystickHandleType.HandleAndBgDirection,
            (true, true, false) => E_JoystickHandleType.HandleAndBg,
            (false, true, true) => E_JoystickHandleType.DirectionAndBg,
            (true, false, false) => E_JoystickHandleType.OnlyHandle,
            _ => throw new InvalidOperationException("Invalid or Missing Joystick Components.")
        };
    }

    private void SetJoystickSelfRect()
    {
        SelfRect = JoystickType switch
        {
            E_JoystickType.Fixed => JoystickHandleType switch
            {
                E_JoystickHandleType.OnlyHandle => RectTransforms[E_JoystickModularName.Handle],
                _ => RectTransforms[E_JoystickModularName.Background]
            },
            E_JoystickType.Floating => RootRect,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void SetupIsUseDirection(bool isUsed)
    {
        _isUseDirection = isUsed;
    }

    #endregion



    #region Override And Implements
    
    /// <summary>
    /// # 이벤트 구독 메서드 (Initialize) 이후에 구독 처리
    /// </summary>
    protected override void SubscribeEvents()
    {
        _joystick.OnSendValueToControl += SendValueToControl;
    }

    /// <summary>
    /// # 구독 해제 메서드 (OnDestroy) 에서 처리 됌
    /// </summary>
    protected override void DisposeEvents()
    {
        _joystick.OnSendValueToControl -= SendValueToControl;
    }

    protected override void PointerDownInteraction(PointerEventData eventData)
    {
        BeginProcess(eventData);
        _joystick?.PointerDownInteraction(eventData);
    }

    protected override void PointerUpInteraction(PointerEventData eventData)
    {
        _joystick?.PointerUpInteraction(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UIState != E_InputUIState.Pressed || eventData.pointerId != FingerID) return;

        DragProcess(eventData);
        _joystick?.OnDragInteraction(eventData);
    }

    #endregion



    #region Process

    private void BeginProcess(PointerEventData eventData)
    {
        AnchoredInitialFingerPosition = ScreenPointToAnchoredPosition(eventData.position);
        AnchoredFingerPosition = AnchoredInitialFingerPosition;
    }

    private void DragProcess(PointerEventData eventData)
    {
        if (RootCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            UICamera = RootCanvas.worldCamera;
        }
        
        FingerPosition.x = eventData.position.x;
        FingerPosition.y = eventData.position.y;

        // 터치 존 (SelfRect)에 맞는 AnchoredPosition으로 AnchoredFingerPosition을 변환
        AnchoredFingerPosition = ScreenPointToAnchoredPosition(FingerPosition);
    }

    #endregion



    #region Utils

    public void SetJoystickMode(E_JoystickType joystickType)
    {
        _joystick.SetMode(joystickType, SetJoystickSelfRect);
    }

    #endregion
}
