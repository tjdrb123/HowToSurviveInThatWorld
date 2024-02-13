
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    // Input Reader
    [SerializeField] private InputReader _inputReader;

    // ========================================
    // # Input Associated.
    // ========================================
    [NonSerialized] public bool IsAttacking;
    [NonSerialized] public bool IsRunning;
    [NonSerialized] public bool IsCrouching;
    [NonSerialized] public bool IsInteracting;
    [NonSerialized] public Vector3 MovementInput;
    [NonSerialized] public Vector3 MovementVector;
    // Normalized Input Vector
    private Vector2 _inputVector;

    #endregion



    #region Unity Behavior

    private void OnEnable()
    {
        RegisterEventsForInput();
    }

    private void OnDisable()
    {
        UnregisterEventsForInput();
    }

    #endregion



    #region Events Register

    // ========================================
    // # Input Events
    // ========================================
    private void RegisterEventsForInput()
    {
        _inputReader.OnMoveEvent += Movement;
        _inputReader.OnCrouchEvent += Crouch;
        _inputReader.OnAttackEvent += Attack;
        _inputReader.OnAttackCanceledEvent += CanceledAttack;
        _inputReader.OnInteractEvent += Interaction;
        _inputReader.OnInteractCanceledEvent += CanceledInterAction;
        _inputReader.OnRunEvent += Run;
    }

    private void UnregisterEventsForInput()
    {
        _inputReader.OnMoveEvent -= Movement;
        _inputReader.OnCrouchEvent -= Crouch;
        _inputReader.OnAttackEvent -= Attack;
        _inputReader.OnAttackCanceledEvent -= CanceledAttack;
        _inputReader.OnInteractEvent -= Interaction;
        _inputReader.OnInteractCanceledEvent -= CanceledInterAction;
        _inputReader.OnRunEvent -= Run;
    }

    #endregion



    #region Input Event Listening
    
    private void Movement(Vector2 movementInput)
    {
        _inputVector = movementInput;
        MovementInput = new Vector3(_inputVector.x, 0f, _inputVector.y);
    }

    private void Run()
    {
        IsRunning = !IsRunning;
    }

    private void Crouch()
    {
        IsCrouching = !IsCrouching;
    }

    private void Attack()
    {
        IsAttacking = true;
    }

    private void CanceledAttack()
    {
        IsAttacking = false;
    }

    private void Interaction()
    {
        DebugLogger.Log("interaction performed");
    }

    private void CanceledInterAction()
    {
        DebugLogger.Log("interaction canceled");
    }

    #endregion
}
