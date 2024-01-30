
using System;
using UnityEngine;

public sealed class Player : Unit
{
    #region Fields

    // Input Reader
    [SerializeField] private InputReader _inputReader;

    private Vector2 _inputVector;

    // Input Associated.
    [NonSerialized] public bool IsAttack;
    [NonSerialized] public bool IsRunning;
    [NonSerialized] public Vector3 MovementInput;
    [NonSerialized] public Vector3 MovementVector;

    #endregion



    #region Input Events

    protected override void EntitySubscribeEvents()
    {
        // 추 후에 이벤트 구독 내용이 변경 될 경우 사용 예정
        // base.EntitySubscribeEvents();
        
        _inputReader.OnMoveEvent += Movement;
    }

    protected override void EntityDisposeEvents()
    {
        // 추 후에 이벤트 구독 내용이 변경 될 경우 사용 예정
        // base.EntityDisposeEvents();
        
        _inputReader.OnMoveEvent -= Movement;
    }

    #endregion



    #region Unity Behavior

    protected override void Awake()
    {
        // Base (Unit) Awake 초기화 진행
        base.Awake();
        
    }

    private void FixedUpdate()
    {
        
    }

    #endregion



    #region Recalculate Movement Input
    
    

    #endregion



    #region Input Event Listener
    // # 인풋 관련된 이벤트 리스너들이 추가 될 예정.
    
    private void Movement(Vector2 movementInput)
    {
        _inputVector = movementInput;
        MovementInput = new Vector3(_inputVector.x, 0f, _inputVector.y);
    }

    #endregion
}
