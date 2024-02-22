
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
    private bool _isInteracting;
    private AnimatorStateInfo _stateInfo;

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
        _isInteracting = _playerController.IsInteracting;
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        CheckAnimationState("PickingUp");
        CheckAnimationState("Dig");
        return _isInteracting;
    }

    private void CheckAnimationState(string animationName)
    {
        if (_stateInfo.IsName(animationName) && _stateInfo.normalizedTime <= 1.0)
        {
            _isInteracting = true;
            _playerController.MovementVector = Vector3.zero;
        }
        else if (_stateInfo.IsName(animationName) && _stateInfo.normalizedTime > 1.0)
        {
            _isInteracting = false;
        }
    }

    #endregion
}