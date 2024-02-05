
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class Player : Unit, ICombative
{
    #region Fields

    // Input Reader
    [SerializeField] private InputReader _InputReader;

    private Vector2 _inputVector;

    // Input Associated.
    [NonSerialized] public bool IsAttack;
    [NonSerialized] public bool IsRunning;
    [NonSerialized] public Vector3 MovementInput;
    [NonSerialized] public Vector3 MovementVector;

    public GameObject TestTarget;

    #endregion



    #region Input Events

    protected override void EntitySubscribeEvents()
    {
        // 추 후에 이벤트 구독 내용이 변경 될 경우 사용 예정
        // base.EntitySubscribeEvents();
        
        _InputReader.OnMoveEvent += Movement;
    }

    protected override void EntityDisposeEvents()
    {
        // 추 후에 이벤트 구독 내용이 변경 될 경우 사용 예정
        // base.EntityDisposeEvents();
        
        _InputReader.OnMoveEvent -= Movement;
    }

    #endregion



    #region Unity Behavior

    protected override void Awake()
    {
        // Base (Unit) Awake 초기화 진행
        base.Awake();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ApplyDamage(this, TestTarget);
        }
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



    #region Implement Combative

    /// <summary>
    /// # 타겟에 대해 데미지를 적용하는 메서드
    /// </summary>
    /// <param name="source">총알, 자신 등 컬라이더로 할 경우 해당 충돌체</param>
    /// <param name="target">타겟이 되는 게임오브젝트 (Enemy_Zombie(1)) 같은</param>
    public void ApplyDamage(Object source, GameObject target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        HealthStatModifier rawDamage = new HealthStatModifier
        {
            Instigator = gameObject,
            Type = E_StatModifier_OperatorType.Additive,
            Magnitude = Literals.NONE_F * _statController.GetStat(E_StatType.Damage).Value,
            Source = source,
            IsCriticalHit = false
        };

        DebugLogger.LogWarning("TEST");
        
        damageable.TakeDamage(rawDamage);
    }

    #endregion
}
