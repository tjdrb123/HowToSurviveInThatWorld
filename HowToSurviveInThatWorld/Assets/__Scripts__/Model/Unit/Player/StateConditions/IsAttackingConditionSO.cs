using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsAttackingCondition", menuName = "State Machine/Conditions/Is Attacking Condition")]
public class IsAttackingConditionSO : FiniteStateConditionSO
{
    #region Property (Override)

    protected override FiniteStateCondition CreateCondition() => new IsAttackingCondition();

    #endregion
}

public class IsAttackingCondition : FiniteStateCondition
{
    #region Fields

    private PlayerController _playerController;
    private Animator _animator;
    private bool _isAttacking;
    private AnimatorStateInfo _stateInfo;

    // Property (Origin SO)
    private new IsAttackingConditionSO OriginSO => base.OriginSO as IsAttackingConditionSO;

    #endregion


    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }

    protected override bool Statement()
    {
        _isAttacking = _playerController.IsAttacking;
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        CheckAnimationState("Attack");

        return _isAttacking;
    }
    
    private void CheckAnimationState(string animationName)
    {
        if (_stateInfo.IsName(animationName) && _stateInfo.normalizedTime <= 1.0)
        {
            _isAttacking = true;
            _playerController.MovementVector = Vector3.zero;
        }
        else if (_stateInfo.IsName(animationName) && _stateInfo.normalizedTime > 1.0)
        {
            _isAttacking = false;
        }
    }

    #endregion
}