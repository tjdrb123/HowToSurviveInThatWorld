
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatTable", menuName = "Stat System/Stat Table", order = 0)]
public class StatTableSO : ScriptableObject
{
    #region Fields

    // Damage, Armor와 같은 단일 속성 값들
    public List<StatDefinitionSO> Stats;
    // Hp, Exp 같은 Cur/Max 속성 값들
    public List<StatDefinitionSO> Attributes;
    // 퍽, 스킬, 힘 등 주 스탯영역
    public List<StatDefinitionSO> Primary;

    #endregion
}