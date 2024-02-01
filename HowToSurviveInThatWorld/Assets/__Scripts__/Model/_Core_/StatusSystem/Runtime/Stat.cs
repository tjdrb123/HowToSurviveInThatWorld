
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// # 모든 스탯에 기본이 되는 베이스 클래스
///   - 기본적인 스탯 (예, 데미지/방어력 등)
/// </summary>
public class Stat
{
    #region Fields
    
    // 베이스가 될 설정 값(SO) => 초기 데이터 모델은 이 것을 사용
    protected StatDefinitionSO _statDefinition;
    
    protected float _value;
    
    protected List<StatModifier> _statModifiers = new();
    
    // Event
    // 값이 변경 되었을 때 호출 될 이벤트
    public event Action OnValueChanged;

    #endregion



    #region Properties

    /* Property (Getter) */
    public float Value => _value;
    public virtual float BaseValue => _statDefinition.BaseValue;

    #endregion



    #region Constructor

    public Stat(StatDefinitionSO definitionSo)
    {
        _statDefinition = definitionSo;
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
        float finalValue = BaseValue;
        
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

        if (Math.Abs(_value - finalValue) > float.MinValue)
        {
            _value = finalValue;
            OnValueChanged?.Invoke();
        }
    }

    #endregion
}
