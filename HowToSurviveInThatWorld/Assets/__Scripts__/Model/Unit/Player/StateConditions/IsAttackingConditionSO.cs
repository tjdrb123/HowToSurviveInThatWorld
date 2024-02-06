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


    // Property (Origin SO)
    private new IsAttackingConditionSO OriginSO => base.OriginSO as IsAttackingConditionSO;

    #endregion


    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerScript = finiteStateMachine.GetComponent<Player>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }

    protected override bool Statement()
    {
        _isAttacking = _playerScript.IsAttacking;
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_stateInfo.IsName("Punch") && _stateInfo.normalizedTime <= 1.0)
        {
            _isAttacking = true;
            _animator.SetFloat("Horizontal", 0);
            _animator.SetFloat("Vertical", 0);
        }
        else if (_stateInfo.IsName("Punch") && _stateInfo.normalizedTime > 1.0)
        {
            _isAttacking = false;
        }

        return _isAttacking;
    }

    #endregion
}