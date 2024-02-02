
using UnityEngine;

[CreateAssetMenu(fileName = "IsCrouchingCondition", menuName = "State Machine/Conditions/Is Crouching Condition")]
public class IsCrouchingConditionSO : FiniteStateConditionSO
{
    #region Property (Override)
    
    protected override FiniteStateCondition CreateCondition() => new IsCrouchingCondition();
    
    #endregion
}

public class IsCrouchingCondition : FiniteStateCondition
{
    #region Fields

    private Player _playerScript;

    // Property (Origin SO)
    private new IsCrouchingConditionSO OriginSO => base.OriginSO as IsCrouchingConditionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerScript = finiteStateMachine.GetComponent<Player>();
    }
    
    protected override bool Statement()
    {
        return _playerScript.IsCrouching;
    }

    #endregion
}