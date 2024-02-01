
using UnityEngine;

public class Primary : Stat
{
    #region Fields

    private float _baseValue;

    // Stat 클래스에 존재하는 DefinitionSO.BaseValue 값을 오버라이드
    public override float BaseValue => _baseValue;

    #endregion
    
    
    
    #region Constructor

    public Primary(StatDefinitionSO definitionSO) : base(definitionSO)
    {
        _baseValue = definitionSO.BaseValue;
        CalculatedValue();
    }

    #endregion



    #region Add / Sub

    public void Add(float amount)
    {
        _baseValue += amount;
        CalculatedValue();
    }

    public void Subtract(float amount)
    {
        _baseValue -= amount;
        CalculatedValue();
    }

    #endregion
}
