
using UnityEngine;

public class HealthStatModifier : StatModifier, IDamage
{
    #region Fields

    public bool IsCriticalHit { get; set; }

    public GameObject Instigator { get; set; }

    #endregion
}