
using UnityEngine;

[CreateAssetMenu(fileName = "IsInteractingCondition", menuName = "State Machine/Conditions/Is Interacting Condition")]
public class IsInteractingConditionSO : FiniteStateConditionSO
{
    #region Property (Override)
    
    protected override FiniteStateCondition CreateCondition() => new IsInteractingCondition();
    
    #endregion
}

public class IsInteractingCondition : FiniteStateCondition
{
    #region Fields
    
    private PlayerController _playerController;
    private Animator _animator;

    // Property (Origin SO)
    private new IsInteractingConditionSO OriginSO => base.OriginSO as IsInteractingConditionSO;

    #endregion

    
    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }
    
    protected override bool Statement()
    {
        return _playerController.IsInteracting;
    }

    #endregion
}