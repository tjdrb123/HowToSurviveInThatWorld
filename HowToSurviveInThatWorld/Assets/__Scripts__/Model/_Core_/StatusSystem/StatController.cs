
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    #region Fields

    [SerializeField] private StatTableSO _StatTableSO;
    protected readonly Dictionary<string, Stat> _stats = new(StringComparer.InvariantCulture);

    private bool _isInit;
    
    // Events
    public event Action OnInitialized;
    public event Action OnWillUninitialized;

    #endregion



    #region Properties

    public Dictionary<string, Stat> Stats => _stats;

    public bool IsInit => _isInit;

    #endregion



    #region Unity Behavior & Initialize

    protected virtual void Awake()
    {
        if (!_isInit)
        {
            Initialized();
            OnInitialized?.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnWillUninitialized?.Invoke();
    }

    public void Initialized()
    {
        foreach (var definition in _StatTableSO.Stats)
        {
            _stats.Add(definition.name, new Stat(definition));
        }

        foreach (var definition in _StatTableSO.Attributes)
        {
            _stats.Add(definition.name, new Attribute(definition));
        }
        
        foreach (var definition in _StatTableSO.Primary)
        {
            _stats.Add(definition.name, new Primary(definition));
        }
        
        _isInit = true;
    }

    #endregion
}
