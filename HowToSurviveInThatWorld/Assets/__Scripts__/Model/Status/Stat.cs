
using System;
using System.Collections.Generic;

public class Stat
{
    #region Fields

    /* Fields (Member) */

    public float BaseValue;
    
    private float _value;
    private bool _isChanged = true;

    private readonly List<StatModifier> _statModifiers;

    /* Property */
    public float Value
    {
        get
        {
            if (_isChanged)
            {
                _value = CalculateFinalValue();
                _isChanged = false;
            }

            return _value;
        }
    }
    
    #endregion



    #region Constructor

    public Stat(float value)
    {
        BaseValue = value;
        _statModifiers = new List<StatModifier>();
    }

    #endregion



    #region Modifier

    public void AddModifier(StatModifier modifier)
    {
        _isChanged = true;
        _statModifiers.Add(modifier);
        // Priority에 따라 안에 들어있는 List를 정렬
        _statModifiers.Sort(CompareModifierPriority);
    }

    public bool RemoveModifier(StatModifier modifier)
    {
        _isChanged = true;
        return _statModifiers.Remove(modifier);
    }

    private float CalculateFinalValue()
    {
        float finalValue = Value;

        foreach (var modifier in _statModifiers)
        {
            finalValue = modifier.Type switch
            {
                E_StatModType.Flat => finalValue + modifier.Value,
                E_StatModType.Percent => finalValue + (finalValue * modifier.Value / Literals.HUND_F)
            };
        }

        // 12.0001f와 12f는 다르기 때문에 Round(결과 값 반올림)로 반환
        // 소수점 4 번째 자리까지 반올림함.
        return (float)Math.Round(finalValue, 4);
    }

    #endregion



    #region Utils

    /// <summary>
    /// # 스탯 수정자에 대해 우선순위를 비교하여 반환하는 메서드
    /// </summary>
    /// <returns>
    /// <para> -1 : mod1이 mod2보다 작을 때 </para>
    /// <para> 0 : mod1과 mod2가 같을 때 </para>
    /// <para> 1 : mod1이 mod2보다 클 때 </para>
    /// </returns>
    private int CompareModifierPriority(StatModifier mod1, StatModifier mod2)
    {
        if (mod1.Priority < mod2.Priority) 
            return -1;
        
        return mod1.Priority > mod2.Priority ? 1 : 0;
    }

    #endregion
}
