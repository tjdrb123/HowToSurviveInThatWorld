
using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Fields

    // Input Reader
    [SerializeField] private InputReader _inputReader;

    private LayerMask _interactableLayer;
    private LayerMask _wallMask;
    [NonSerialized] public Collider[] _hitColliders;
    private Transform _transform;
    private Animator _animator;
    public Image _chargingImg;
    public Image _interactBtnImg;
    [HideInInspector]public Player player;

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
        _animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        _interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
        _wallMask = 1 << LayerMask.NameToLayer("Wall");
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
        _inputReader.OnInteractEvent += Interact;
        _inputReader.OnInteractCanceledEvent += CanceledInteract;
        _inputReader.OnRunEvent += Run;
    }

    private void UnregisterEventsForInput()
    {
        _inputReader.OnMoveEvent -= Movement;
        _inputReader.OnCrouchEvent -= Crouch;
        _inputReader.OnAttackEvent -= Attack;
        _inputReader.OnAttackCanceledEvent -= CanceledAttack;
        _inputReader.OnInteractEvent -= Interact;
        _inputReader.OnInteractCanceledEvent -= CanceledInteract;
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

        if (IsRunning && IsCrouching)
            IsCrouching = false;
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

    private void Update()
    {
        _hitColliders = Physics.OverlapBox(_transform.position + Vector3.up * 1f,  new Vector3(0.5f, 1f, 0.5f), Quaternion.identity, _interactableLayer | _wallMask);
        if (_hitColliders.Length > 0)
            _interactBtnImg.color = Color.white;
        else
        {
            _interactBtnImg.color = Color.black;
            IsInteracting = false;
        }
    }

    private void Interact()
    {
        if (_hitColliders.Length > 0)
        {
            _hitColliders = _hitColliders.OrderBy(colider => Vector3.Distance(_transform.position, colider.transform.position)).ToArray();
            IsInteracting = Manager_Inventory.Instance.InventoryMaxCheck();
            if (_hitColliders[0].GetComponent<Looting>())
                _hitColliders[0].GetComponent<Looting>().ChargingImg = _chargingImg;
            Vector3 directionToLookAt = _hitColliders[0].transform.position - _transform.position;
            directionToLookAt.y = 0;
            Quaternion rotation = Quaternion.LookRotation(directionToLookAt);
            _transform.rotation = rotation;
        }
    }

    private void CanceledInteract()
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
