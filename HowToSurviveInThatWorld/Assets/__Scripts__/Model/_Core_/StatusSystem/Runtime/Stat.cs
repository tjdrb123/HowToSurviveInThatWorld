
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class Stat
{
    #region Fields
    
    protected StatDefinition _statDefinition;
    protected int _value;
    
    // Modifier
    protected List<StatModifier> _statModifiers = new();
    
    // Event
    public event Action OnValueChanged;
    
    /* Property (Getter) */
    public int Value => _value;
    public virtual int BaseValue => _statDefinition.BaseValue;

    #endregion



    #region Constructor

    public Stat(StatDefinition definition)
    {
        _statDefinition = definition;
    }

    #endregion



    #region Modifier Stat

    public void AddModifier(StatModifier modifier)
    {
        _statModifiers.Add(modifier);
    }

    public void RemoveModifierFromSource(Object source)
    {
        _statModifiers = _statModifiers.Where(mod =>
            mod.Source.GetInstanceID() != source.GetInstanceID()).ToList();
    }

    protected void CalculatedValue()
    {
        int finalValue = BaseValue;
        
        _statModifiers.Sort((x, y) => x.Type.CompareTo(y.Type));

        for (int i = 0; i < _statModifiers.Count; ++i)
        {
            var modifier = _statModifiers[i];

            if (modifier.Type == E_StatModifier_OperationType.Additive)
            {
                finalValue += modifier.Magnitude;
            }
            else if (modifier.Type == E_StatModifier_OperationType.Multiplicative)
            {
                finalValue *= modifier.Magnitude;
            }
        }

        if (_statDefinition.Capacity >= 0)
        {
            finalValue = Mathf.Min(finalValue, _statDefinition.Capacity);
        }

        if (_value != finalValue)
        {
            _value = finalValue;
            OnValueChanged?.Invoke();
        }
    }

    #endregion
}
