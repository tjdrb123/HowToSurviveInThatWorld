
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatTable", menuName = "Stat System/Stat Table", order = 0)]
public class StatTableSO : ScriptableObject
{
    #region Fields

    public List<StatDefinitionSO> Stats;

    #endregion
}