
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class StatController : MonoBehaviour
{
    #region Private Enum

    private enum E_Category
    {
        Stat,
        Attribute,
        Primary
    }

    #endregion
    
    
    
    #region Fields

    [SerializeField] private StatTableSO _StatTableSO;
    
    protected readonly Dictionary<E_StatType, Stat> _stats = new();
    private IReadOnlyDictionary<E_StatType, Stat> _readOnlyStats;

    private bool _isInit;
    
    // Events
    public event Action OnInitialized;
    public event Action OnWillUninitialized;

    #endregion



    #region Properties

    public IReadOnlyDictionary<E_StatType, Stat> Stats =>
        // _readOnlYStats가 비어 있을 경우 동적할당 및 대입
        _readOnlyStats ??= new ReadOnlyDictionary<E_StatType, Stat>(_stats);

    public bool IsInit => _isInit;

    #endregion



    #region Unity Behavior & Initialize

    protected virtual void Awake()
    {
        if (!_isInit)
        {
            Initialize();
        }
    }

    private void OnDestroy()
    {
        OnWillUninitialized?.Invoke();
    }

    public void Initialize()
    {
        InitializeStats(_StatTableSO.Stats, E_Category.Stat);
        InitializeStats(_StatTableSO.Attributes, E_Category.Attribute);
        InitializeStats(_StatTableSO.Primary, E_Category.Primary);
        
        _isInit = true;
        OnInitialized?.Invoke();
    }

    #endregion



    #region Initialized Internal

    private void InitializeStats(IEnumerable<StatDefinitionSO> definitions, E_Category category)
    {
        foreach (var definition in definitions)
        {
            if (definition.Type == E_StatType.None)
            {
                DebugLogger.LogWarning($"StatDefinitionSO '{definition.name}' has 'None' type, will not be added.");
                continue;
            }

            if (_stats.ContainsKey(definition.Type))
            {
                DebugLogger.LogWarning($"Duplicate StatDefinitionSO type '{definition.Type}'");
                continue;
            }

            var newStat = category switch
            {
                E_Category.Stat => new Stat(definition),
                E_Category.Attribute => new Attribute(definition),
                E_Category.Primary => new Primary(definition)
            };
            
            _stats.Add(definition.Type, newStat);
        }
    }

    #endregion



    #region Get Function

    /// <summary>
    /// # GetStat
    ///   - Stat으로 반환 받는 메서드.
    ///   - Attribute거나 Primary일 경우 as를 통한 [형 변환] 필요
    /// </summary>
    public Stat GetStat(E_StatType statType)
    {
        if (_stats.TryGetValue(statType, out var stat))
        {
            return stat;
        }

        DebugLogger.LogWarning($"Stat of type {statType} not found. 'return null'");
        return null;
    }

    /// <summary>
    /// # GetStatT - 제네릭 타입
    ///   - Attribute, Primary, Stat등을 직접 제약 조건을 주어 받기 위함.
    ///   - where 제약 조건을 걸어 Stat과 상속자들로만 <T>를 설정할 수 있음
    /// </summary>
    /// <typeparam name="T">Stat, Attribute, Primary</typeparam>
    public T GetStat<T>(E_StatType statType) where T : Stat
    {
        if (_stats.TryGetValue(statType, out var stat) && stat is T typedStat)
        {
            return typedStat;
        }
        
        DebugLogger.LogWarning($"Stat of type {statType} not found. 'return null'");
        return null;
    }

    #endregion
}
