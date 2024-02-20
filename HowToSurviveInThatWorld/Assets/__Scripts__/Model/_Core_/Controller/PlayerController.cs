
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Fields

    // Input Reader
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private LayerMask _interactableLayer;
    [NonSerialized] public Collider[] _hitColliders;
    private Transform _transform;
    public Image _chargingImg;

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

    #region Property



    #endregion



    #region Unity Behavior

    private void Awake()
    {
        _transform = transform;
    }

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
        Vector3 boxCenter = _transform.position + Vector3.up * 1f;
        Vector3 halfExtents = new Vector3(0.5f, 1f, 0.5f);

        _hitColliders = Physics.OverlapBox(boxCenter, halfExtents, Quaternion.identity, _interactableLayer);
        if (_hitColliders.Length > 0)
        {
            _hitColliders = _hitColliders.OrderBy(colider => Vector3.Distance(_transform.position, colider.transform.position)).ToArray();
            IsInteracting = true;
            
            Vector3 directionToLookAt = _hitColliders[0].transform.position - _transform.position;
            directionToLookAt.y = 0;
            Quaternion rotation = Quaternion.LookRotation(directionToLookAt);
            _transform.rotation = rotation;
        }
    }

    private void CanceledInterAction()
    {
        IsInteracting = false;
    }

    void OnDrawGizmos()
    {
        if (_transform == null)
        {
            _transform = transform;
        }
        Vector3 cubeCenter = _transform.position + Vector3.up * 1f;
        Vector3 cubeSize = new Vector3(1f, 2f, 1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(cubeCenter, cubeSize);
    }

    #endregion
}
