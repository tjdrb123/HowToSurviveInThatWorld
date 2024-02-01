
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatDatabase", menuName = "Stat System/Stat Database", order = 0)]
public class StatTableSO : ScriptableObject
{
    #region Fields

    public List<StatDefinitionSO> Stats;

    #endregion
}