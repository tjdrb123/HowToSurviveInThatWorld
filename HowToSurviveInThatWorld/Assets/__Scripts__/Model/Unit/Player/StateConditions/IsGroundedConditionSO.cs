
using UnityEngine;

[CreateAssetMenu(fileName = "IsGroundedCondition", menuName = "State Machine/Conditions/Is Grounded Condition")]
public class IsGroundedConditionSO : FiniteStateConditionSO
{
    #region Property (Override)
    
    protected override FiniteStateCondition CreateCondition() => new IsGroundedCondition();
    
    #endregion
}

public class IsGroundedCondition : FiniteStateCondition
{
    #region Fields

    private CharacterController _characterController;

    // Property (Origin SO)
    private new IsGroundedConditionSO OriginSO => base.OriginSO as IsGroundedConditionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _characterController = finiteStateMachine.GetComponent<CharacterController>();
    }

    protected override bool Statement() => _characterController.isGrounded;

    #endregion
}