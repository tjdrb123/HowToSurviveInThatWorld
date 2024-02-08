
using UnityEngine;

[CreateAssetMenu(fileName = "CrouchAction", menuName = "State Machine/Actions/Crouch Action")]
public class MovementCrouchActionSO : FiniteStateActionSO
{
    #region Property (Override)
    
    [SerializeField] private FiniteStateAction.SpecificMoment _moment;
    
    // Properties
    
    public FiniteStateAction.SpecificMoment Moment => _moment;
    protected override FiniteStateAction CreateAction() => new MovementCrouchAction();
    
    #endregion
}

public class MovementCrouchAction : FiniteStateAction
{
    #region Fields
    
    private Player _playerScript;
    private Animator _animator;

    // Property (Origin SO)
    private new MovementCrouchActionSO OriginSO => base.OriginSO as MovementCrouchActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerScript = finiteStateMachine.GetComponent<Player>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }
    
    public override void FiniteStateEnter()
    {
        if (OriginSO.Moment == SpecificMoment.OnEnter)
        {
            _animator.SetBool("IsCrouching", true);
        }
    }

    public override void FiniteStateExit()
    {
        _animator.SetBool("IsCrouchingWalk", false);
        _animator.SetBool("IsCrouching", false);
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
    }
    
    public override void FiniteStateFixedUpdate()
    {
        _animator.SetBool("IsCrouchingWalk", _playerScript.MovementInput.x != 0 || _playerScript.MovementInput.z != 0);
    }

    #endregion
}