using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// # 스탯 밸류에 직접적인 영향을 주는 '수정자' 클래스
/// </summary>
public class StatModifier
{
    #region Field

    public readonly float Value;
    public readonly E_StatModType Type;
    public readonly int Priority;

    #endregion



    #region Constructor

    public StatModifier(float value, E_StatModType type, int priority)
    {
        Value = value;
        Type = type;
        Priority = priority;
    }

    public StatModifier(float value, E_StatModType type) :
        this(value, type, (int)type) { }

    #endregion
}
