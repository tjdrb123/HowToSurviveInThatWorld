
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public abstract class InputUIBase : OnScreenControl,
    IPointerDownHandler,
    IPointerUpHandler
{
    #region Fields - OnScreenControls
    
    protected virtual string ControlPath { get; set; }
    
    /* Property Override */
    // Input Control Action 재정의
    // OnScreenControls로 보내기 위한 오버라이드 프로퍼티
    protected override string controlPathInternal
    {
        get => ControlPath;
        set => ControlPath = value;
    }

    #endregion



    #region Fields - Input Bases

    /* ======================= Enum ======================= */
    public enum E_InputUIState
    {
        Active,
        Inactive,
        Pressed
    }
    
    /* ======================= Config =======================*/
    public Canvas RootCanvas;
    public Camera UICamera;
    
    /* ======================= Parameters ======================= */
    public RectTransform RootRect { get; private set; }
    public RectTransform SelfRect;
    public E_InputUIState UIState;
    public float SelfRadius;
    public bool IsActive = true;
    // Fingers Info
    public bool IsFingerDown;
    public int FingerID = FINGER_ID_BASE;
    public Vector2 InitialFingerPosition;
    public Vector2 FingerPosition;
    
    // CONST
    private const int FINGER_ID_BASE = -99;

    #endregion



    #region Abstract & Virtual (No Base)

    /* Abstract */
    protected abstract void Initialize();
    protected abstract void PointerDownInteraction(PointerEventData eventData);
    protected abstract void PointerUpInteraction(PointerEventData eventData);
    
    /* Virtual (No Base) */
    protected virtual void SubscribeEvents() { }

    protected virtual void DisposeEvents() { }

    #endregion
    
    
    
    #region Unity Behavior

    private void Awake()
    {
        /* Setup */
        RootRect = GetComponent<RectTransform>();
        SelfRect = RootRect;
        RootCanvas = GetComponentInParent<Canvas>();

        if (RootCanvas == null)
        {
            DebugLogger.LogError("Canvas is not placed. Can't GetComponent.");
        }

        UpdateRectBoundary();
        UpdateState();
        Initialize();
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        DisposeEvents();
    }

    #endregion



    #region Interface Methods

    /// <summary>
    /// # 해당하는 RectTransform을 눌렀을 때
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // 버튼이 활성화 상태여야만 해당 동작을 하게끔 설정
        if (UIState != E_InputUIState.Active) return;
        
        // SelfRect 내부 터치가 아니라면 동작하지 않게끔 설정
        if (!RectTransformUtility.RectangleContainsScreenPoint(
                SelfRect, eventData.position, UICamera)) return;

        // 인터랙션이 시작 됐을 때 실행
        BeginInteraction(eventData);
        PointerDownInteraction(eventData);
    }

    /// <summary>
    /// # 손을 뗏을 때
    /// </summary>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // 버튼이 눌러져 있는 상태가 아니라면 리턴
        if (UIState != E_InputUIState.Pressed) return;

        EndInteraction();
        PointerUpInteraction(eventData);
    }

    #endregion



    #region Interaction

    private void BeginInteraction(PointerEventData eventData)
    {
        // 초기 터치를 했을 때 UI카메라 유/무에 따라 셋팅
        SetupUICamera();
        
        // Finger Setup
        IsFingerDown = true;
        FingerID = eventData.pointerId;
        InitialFingerPosition = eventData.position;
        FingerPosition = InitialFingerPosition;

        // 해당 버튼이 눌러짐으로 표시
        UIState = E_InputUIState.Pressed;
    }

    private void EndInteraction()
    {
        // Finger Setup
        IsFingerDown = false;
        FingerID = FINGER_ID_BASE;
        InitialFingerPosition = Vector2.zero;
        FingerPosition = InitialFingerPosition;
        
        // Pressed가 끝났으니 Active 또는 Inactive로 변경
        UpdateState();
    }

    #endregion



    #region Utils

    private void SetupUICamera()
    {
        UICamera = RootCanvas.renderMode switch
        {
            RenderMode.ScreenSpaceOverlay => null,
            RenderMode.ScreenSpaceCamera => RootCanvas.worldCamera,
            _ => UICamera
        };
    }

    public void SetActiveState(bool isActive)
    {
        IsActive = isActive;
        UpdateState();
    }

    private void UpdateState()
    {
        UIState = (IsActive) ? E_InputUIState.Active : E_InputUIState.Inactive;
    }

    protected virtual void UpdateRectBoundary()
    {
        SelfRadius = SelfRect.rect.width / Literals.TWO_F * RootCanvas.scaleFactor;
    }

    /// <summary>
    /// # 앵커 포지션을 구하는 메서드 (기본, SelfRect를 기준으로)
    /// </summary>
    /// <param name="screenPosition">화면을 터치한 좌표를 보내야됌 (ex. eventData.position)</param>
    /// <returns>SelfRect 기준의 앵커드 포지션 반환 아니면 Vector2.zero 반환</returns>
    public Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            SelfRect, screenPosition, UICamera, out var localPosition)
            ? localPosition 
            : Vector2.zero;
    }

    /// <summary>
    /// # 앵커 포지션을 구하는 메서드 (매개변수로 전달하는 root 기준)
    /// </summary>
    public Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition, RectTransform root)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            root, screenPosition, UICamera, out var localPosition) 
            ? localPosition 
            : Vector2.zero;
    }

    #endregion
}
