
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MovementAttackAction", menuName = "State Machine/Actions/Movement Attack Action")]
public class MovementAttackActionSO : FiniteStateActionSO
{
    #region Property (Override)
    
    [SerializeField] private FiniteStateAction.SpecificMoment _moment;
    
    // Properties
    
    public FiniteStateAction.SpecificMoment Moment => _moment;
    protected override FiniteStateAction CreateAction() => new MovementAttackAction();
    
    #endregion
}

public class MovementAttackAction : FiniteStateAction
{
    #region Fields

    private PlayerController _playerController;
    private Animator _animator;
    
    // Property (Origin SO)
    private new MovementAttackActionSO OriginSO => base.OriginSO as MovementAttackActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }
    
    public override void FiniteStateEnter()
    {
        if (OriginSO.Moment == SpecificMoment.OnEnter)
        {
            _animator.SetTrigger("IsAttacking");
        }
    }

    public override void FiniteStateExit()
    {
        _animator.ResetTrigger("IsAttacking");
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
    }
    
    public override void FiniteStateFixedUpdate()
    {
        // None
    }
    
    #endregion
}