using UnityEngine;

public class Level : BaseSingleAttribute
{
    // Constructor
    public Level(float setValue) : base(setValue) { }
    
    
    
    #region Abstract Methods

    protected override void PerformSetting(float amount)
    {
        // 최소치 ~ 최대치 예외 및 검증
        amount = Mathf.Clamp(amount, 0f, 200f);
        
        ValueChangedHandle(amount);
    }

    protected override void PerformAddition(float amount)
    {
        var newValue = Mathf.Min(_value + amount, 200f);
        
        ValueChangedHandle(newValue);
    }

    protected override void PerformSubtraction(float amount)
    {
        var newValue = Mathf.Max(_value - amount, 0f);
        
        ValueChangedHandle(newValue);
    }

    #endregion
}