
using UnityEngine;

[CreateAssetMenu(fileName = "StatDefinition", menuName = "Stat System/Stat Definition", order = 0)]
public class StatDefinition : ScriptableObject
{
    #region Fields

    [SerializeField] private int _BaseValue;
    [SerializeField] private int _Capacity;
    
    /* Property (Getter) */
    public int BaseValue => _BaseValue;
    public int Capacity => _Capacity;

    #endregion
}
