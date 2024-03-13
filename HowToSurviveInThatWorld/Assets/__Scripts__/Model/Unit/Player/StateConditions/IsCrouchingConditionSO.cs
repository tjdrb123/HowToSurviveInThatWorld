
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

    private PlayerController _playerController;

    // Property (Origin SO)
    private new IsCrouchingConditionSO OriginSO => base.OriginSO as IsCrouchingConditionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
    }
    
    protected override bool Statement()
    {
        return _playerController.IsCrouching;
    }

    #endregion
}