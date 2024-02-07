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

    private Player _playerScript;
    private Animator _animator;
    private bool _isAttacking;
    private AnimatorStateInfo _stateInfo;
    private int _attackType;
    private Dictionary<int, string> _attackAnimations;


    // Property (Origin SO)
    private new IsAttackingConditionSO OriginSO => base.OriginSO as IsAttackingConditionSO;

    #endregion


    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerScript = finiteStateMachine.GetComponent<Player>();
        _animator = finiteStateMachine.GetComponent<Animator>();
        _attackAnimations = new Dictionary<int, string>
        {
            {0, "Punch"},
            {1, "Recoil_Melee"},
            {2, "Recoil_Pistol"},
            {3, "Recoil_Rifle"}
        };
    }

    protected override bool Statement()
    {
        _isAttacking = _playerScript.IsAttacking;
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _attackType = _animator.GetInteger("AttackType");
        
        if(_attackAnimations.ContainsKey(_attackType))
            CheckAnimationState(_attackAnimations[_attackType]);

        return _isAttacking;
    }
    
    private void CheckAnimationState(string animationName)
    {
        if (_stateInfo.IsName(animationName) && _stateInfo.normalizedTime <= 1.0)
        {
            _isAttacking = true;
            _animator.SetFloat("Horizontal", 0);
            _animator.SetFloat("Vertical", 0);
        }
        else if (_stateInfo.IsName(animationName) && _stateInfo.normalizedTime > 1.0)
        {
            _isAttacking = false;
        }
    }

    #endregion
}