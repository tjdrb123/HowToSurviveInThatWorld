
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
    private readonly StatDefinitionSO _statDefinition;
    // Value 값을 수정 하기 위한 '스탯 수정자'
    private List<StatModifier> _statModifiers = new();
    // 실질적인 Value (수정자에 모든 수정 값을 받고 최종 수정)
    protected float _value;
    
    /* Event */
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
        CalculatedValue();
    }

    #endregion



    #region Modifier Stat

    public void AddModifier(StatModifier modifier)
    {
        _statModifiers.Add(modifier);
        CalculatedValue();
    }

    public void RemoveModifierFromSource(Object source)
    {
        _statModifiers = _statModifiers.Where(mod =>
            mod.Source.GetInstanceID() != source.GetInstanceID()).ToList();
        CalculatedValue();
    }

    /// <summary>
    /// # Modifier를 통한 FinalValue( 최종 값 )를 수정
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected void CalculatedValue()
    {
        float newValue = BaseValue;
        
        _statModifiers.Sort((x, y) => x.Type.CompareTo(y.Type));

        newValue = _statModifiers.Aggregate(newValue, (current, modifier) => modifier.Type switch
        {
            E_StatModifier_OperationType.Additive => current + modifier.Magnitude,
            E_StatModifier_OperationType.Multiplicative => current * modifier.Magnitude,
            E_StatModifier_OperationType.Override => modifier.Magnitude,
            _ => throw new ArgumentOutOfRangeException()
        });

        // Capacity ( 최대 값 ) 설정 값이 0보다 크거나 같을 경우
        if (_statDefinition.Capacity >= Literals.ZERO_F)
        {
            // newValue는 Capacity보다 높아져서는 안된다.
            newValue = Mathf.Min(newValue, _statDefinition.Capacity);
        }

        // Epsilon 부동 소수점 동등(== || !=) 비교
        // 해당 값이 다를 경우 CurrentValue를 newValue로 변경
        if (Mathf.Approximately(newValue, _value)) return;
        
        _value = newValue;
        OnValueChanged?.Invoke();
    }

    #endregion
}
