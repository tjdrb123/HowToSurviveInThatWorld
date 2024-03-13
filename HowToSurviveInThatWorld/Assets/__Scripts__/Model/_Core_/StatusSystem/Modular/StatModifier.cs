
using UnityEngine;

/// <summary>
/// # 스탯 수정자
///   - Stat에 밸류 값이 직접적으로 영향 받을 부분
/// </summary>
public class StatModifier
{
    #region Fields

    public Object Source { get; set; }
    // 얼만큼 수정이 될 것인지에 대한 밸류 값
    public float Magnitude { get; set; }
    // Magnitude를 어떤 타입에 형태로 (더할 것인지 곱할 것인지 등)
    public E_StatModifier_OperatorType Type { get; set; }

    #endregion
}