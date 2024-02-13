
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    #region Fields

    /* Input Variables */
    private GameInput _gameInput;

    /* Events */
    // 기본 값으로 아무 것도 하지 않는 delegate를 정의해줌으로써,
    // 비용이 큰 Null 연산을 하지 않고 그대로 사용하면 된다.
    // Player Inputs
    public event Action<Vector2> OnMoveEvent = delegate { };
    public event Action OnAttackEvent = delegate { };
    public event Action OnAttackCanceledEvent = delegate { };
    public event Action OnCrouchEvent = delegate { };
    public event Action OnPauseEvent = delegate { };
    public event Action OnInteractEvent = delegate { };
    public event Action OnInteractCanceledEvent = delegate { };
    public event Action OnRunEvent = delegate { };

// UI Associated Inputs
    public event Action OnResumeEvent = delegate { };
    
    #endregion



    #region Unity Behavior

    private void OnEnable()
    {
        // Input을 읽을 수 있는 인풋 시스템이 `Null`일 경우
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            // 콜백 메서드를 자기 자신으로 선언
            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);
        }
        
        // 초기에 사용할 `인풋`을 Gameplay로 설정
        SetGameplayInput();
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    #endregion



    #region Interface Implement Methods
    #region Gameplay Input

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                OnAttackEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                OnAttackCanceledEvent.Invoke();
                break;
        }
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnPauseEvent.Invoke();
            SetUIInput();
        }
    }
    
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnCrouchEvent.Invoke();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                OnInteractEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                OnInteractCanceledEvent.Invoke();
                break;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnRunEvent.Invoke();
        }
    }

    #endregion

    

    #region UI Input

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnResumeEvent.Invoke();
            SetGameplayInput();
        }
    }

    #endregion
    #endregion



    #region Setup Action Map

    public void SetGameplayInput()
    {
        _gameInput.Gameplay.Enable();
        _gameInput.UI.Disable();
        
        DebugLogger.Log("Input Enable : 'Gameplay'");
    }

    public void SetUIInput()
    {
        _gameInput.Gameplay.Disable();
        _gameInput.UI.Enable();
        
        DebugLogger.Log("Input Enable : 'UI'");
    }

    public void DisableAllInput()
    {
        _gameInput.Gameplay.Disable();
        _gameInput.UI.Disable();
    }

    #endregion
}
