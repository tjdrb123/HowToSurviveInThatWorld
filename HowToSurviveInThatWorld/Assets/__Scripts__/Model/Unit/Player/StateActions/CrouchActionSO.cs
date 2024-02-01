
using UnityEngine;

[CreateAssetMenu(fileName = "CrouchAction", menuName = "State Machine/Actions/Crouch Action")]
public class CrouchActionSO : FiniteStateActionSO
{
    #region Property (Override)
    
    [SerializeField] private FiniteStateAction.SpecificMoment _moment;
    
    // Properties
    
    public FiniteStateAction.SpecificMoment Moment => _moment;
    protected override FiniteStateAction CreateAction() => new CrouchAction();
    
    #endregion
}

public class CrouchAction : FiniteStateAction
{
    #region Fields
    
    private Player _playerScript;
    private Animator _animator;

    // Property (Origin SO)
    private new CrouchActionSO OriginSO => base.OriginSO as CrouchActionSO;

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
        if (OriginSO.Moment == SpecificMoment.OnExit)
        {
            _animator.SetBool("IsCrouching", false);
        }
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