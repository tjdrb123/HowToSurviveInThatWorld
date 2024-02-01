
using UnityEngine;

[CreateAssetMenu(fileName = "StatDefinition", menuName = "Stat System/Stat Definition", order = 0)]
public class StatDefinitionSO : ScriptableObject
{
    #region Fields

    // 실질적으로 SO에서 관리하게 될 Value (값)
    [SerializeField] private float _BaseValue;
    [SerializeField] private float _Capacity = Literals.NONE_F; // 음수 값으로 초기화 하여 제한이 없도록 기본 값 지정
    
    /* Property (Getter) */
    // 기본 값 (HP로 가정하면 HpMax)
    public float BaseValue => _BaseValue;
    // 최대 값 (한계치)
    public float Capacity => _Capacity;

    #endregion
}
