
using System;
using UnityEngine;

/// <summary>
/// # Current/Max가 존재하는 속성을 위한 스탯
///   - Hp, Mp, Exp같은 속성들
/// </summary>
public class Attribute : Stat
{
    #region Fields

    /* Member */
    protected float _currentValue;
    
    /* Property */
    public float CurrentValue => _currentValue;
    
    /* Events */
    // 현재 값(CurrentValue)가 바꼈을 때 호출할 이벤트
    public event Action OnCurrentValueChanged;
    // 수정자가 적용 되었을 때 호출할 이벤트
    public event Action<StatModifier> OnAppliedModifier;

    #endregion

    public void RawHungry(float index)
    {
        _currentValue -= index;
    }
    
    
    
    #region Contructor

    public Attribute(StatDefinitionSO definitionSO) : base(definitionSO)
    {
        // Current 값을 Max 값에 대응하게 대입.
        // TODO. 추후 이 부분은 변경될 수 있음 (Save/Load)가 추가 될 경우
        _currentValue = _value;
    }

    #endregion



    #region Apply

    /// <summary>
    /// # CurrentValue 값에 수정자를 적용하는 메서드
    /// </summary>
    public virtual void ApplyModifier(StatModifier modifier)
    {
        float newValue = _currentValue;

        newValue = modifier.Type switch
        {
            E_StatModifier_OperatorType.Additive => newValue + modifier.Magnitude,
            E_StatModifier_OperatorType.Multiplicative => newValue * modifier.Magnitude,
            E_StatModifier_OperatorType.Override => modifier.Magnitude,
            _ => throw new ArgumentOutOfRangeException()
        };

        // finalValue 값은 0을 넘어서도 또는 최대 값을 넘어서도 안된다.
        newValue = Mathf.Clamp(newValue, Literals.ZERO_F, _value);

        // Epsilon 부동 소수점 동등(== || !=) 비교
        // 해당 값이 다를 경우 CurrentValue를 newValue로 변경
        if (Mathf.Approximately(newValue, _currentValue)) return;
        
        _currentValue = newValue;
        OnCurrentValueChanged?.Invoke();
        OnAppliedModifier?.Invoke(modifier);
    }

    #endregion
}
