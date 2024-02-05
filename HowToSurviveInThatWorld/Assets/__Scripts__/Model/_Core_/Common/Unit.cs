
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(StatController))]
public class Unit : Entity, IDamageable
{
    #region Fields

    // RequireComponents
    protected Animator _animator;
    protected Collider _collider;
    
    // Stat Controller
    protected StatController _statController;
    
    // Flag Init
    private bool _isInitialized;
    
    /* Events */
    public event Action OnHealthChanged;
    public event Action OnMaxHealthChanged;
    
    public event Action OnInitialize;
    public event Action OnWillUninitialize;
    public event Action OnDefeated;
    
    public event Action<float> OnHealed;
    public event Action<float, bool> OnDamaged;

    #endregion



    #region Properties

    public float Health => (_statController.GetStat<Attribute>(E_StatType.Health)).CurrentValue;
    public float MaxHealth => (_statController.GetStat<Attribute>(E_StatType.Health)).Value;
    public bool IsInitialized => _isInitialized;

    #endregion



    #region Unity Behavior With Override

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        // 컬라이더는 상속 받는 실제 유닛에서 (다운 캐스팅을 해야됌)
        _collider = GetComponent<Collider>();
        // 스탯 컨트롤러 또한 컬라이더와 같음 (필요 시 다운캐스팅 필요)
        _statController = GetComponent<StatController>();
    }
    
    /* Event Register and UnRegister */
    protected override void EntitySubscribeEvents()
    {
        _statController.OnInitialized += StatControllerInitialize;
        _statController.OnWillUninitialized += StatControllerUninitialize;

        if (_statController.IsInit)
        {
            StatControllerInitialize();
        }
    }

    protected override void EntityDisposeEvents()
    {
        _statController.OnInitialized -= StatControllerInitialize;
        _statController.OnWillUninitialized -= StatControllerUninitialize;

        if (_statController.IsInit)
        {
            StatControllerUninitialize();
        }
    }

    #endregion



    #region Event Methods

    private void StatControllerInitialize()
    {
        _statController.GetStat(E_StatType.Health).OnValueChanged += MaxHealthChanged;
        (_statController.GetStat<Attribute>(E_StatType.Health)).OnCurrentValueChanged += HealthChanged;
        (_statController.GetStat<Attribute>(E_StatType.Health)).OnAppliedModifier += AppliedModifier;
        _isInitialized = true;
        OnInitialize?.Invoke();
    }

    private void StatControllerUninitialize()
    {
        OnWillUninitialize?.Invoke();
        _statController.GetStat(E_StatType.Health).OnValueChanged -= MaxHealthChanged;
        (_statController.GetStat<Attribute>(E_StatType.Health)).OnCurrentValueChanged -= HealthChanged;
        (_statController.GetStat<Attribute>(E_StatType.Health)).OnAppliedModifier -= AppliedModifier;
    }

    private void HealthChanged()
    {
        OnHealthChanged?.Invoke();
    }

    private void MaxHealthChanged()
    {
        OnMaxHealthChanged?.Invoke();
    }

    private void AppliedModifier(StatModifier modifier)
    {
        // 음수 값 보다 큰 경우에는 힐로 인식
        if (modifier.Magnitude > Literals.ZERO_F)
        {
            OnHealed?.Invoke(modifier.Magnitude);
        }
        else
        {
            OnDamaged?.Invoke(modifier.Magnitude, ((HealthStatModifier)modifier).IsCriticalHit);
            var healthAttribute = _statController.GetStat<Attribute>(E_StatType.Health).CurrentValue;
            if (Mathf.Approximately(healthAttribute, Literals.ZERO_F))
            {
                OnDefeated?.Invoke();
            }
        }
    }

    #endregion



    #region Applied Stat

    public void TakeDamage(IDamage rawDamage)
    {
        _statController.GetStat<Attribute>(E_StatType.Health).ApplyModifier(new HealthStatModifier
        {
            Magnitude = rawDamage.Magnitude,
            Type = E_StatModifier_OperatorType.Additive,
            Source = rawDamage.Source,
            IsCriticalHit = rawDamage.IsCriticalHit,
            Instigator = rawDamage.Instigator
        });

        DebugLogger.LogWarning(
            $"Health : {_statController.GetStat<Attribute>(E_StatType.Health).CurrentValue} / {_statController.GetStat<Attribute>(E_StatType.Health).Value} ");
    }

    #endregion
}
