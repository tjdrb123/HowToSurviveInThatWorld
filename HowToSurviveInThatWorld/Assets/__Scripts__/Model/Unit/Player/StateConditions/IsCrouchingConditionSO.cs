
using UnityEngine;

[CreateAssetMenu(fileName = "IsCrouchingCondition", menuName = "State Machine/Conditions/Is Crouching Condition")]
public class IsCrouchingConditionSO : FiniteStateConditionSO<IsCrouchingCondition>
{
    #region Property (Override)
    
    protected override FiniteStateCondition CreateCondition() => new IsCrouchingCondition();
    
    #endregion
}

public class IsCrouchingCondition : FiniteStateCondition
{
    #region Fields

    // Property (Origin SO)
    private new IsCrouchingConditionSO OriginSO => base.OriginSO as IsCrouchingConditionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        
    }
    
    protected override bool Statement()
    {
        return true;
    }
    
    public override void FiniteStateEnter()
    {
        
    }

    public override void FiniteStateExit()
    {
        
    }

    #endregion
}