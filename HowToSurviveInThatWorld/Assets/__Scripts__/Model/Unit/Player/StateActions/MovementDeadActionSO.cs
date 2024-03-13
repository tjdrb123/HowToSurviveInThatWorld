
using UnityEngine;

[CreateAssetMenu(fileName = "DeadAction", menuName = "State Machine/Actions/Dead Action")]
public class MovementDeadActionSO : FiniteStateActionSO
{
    #region Property (Override)

    [SerializeField] private FiniteStateAction.SpecificMoment _moment;
    
    // Properties

    public FiniteStateAction.SpecificMoment Moment => _moment;
    protected override FiniteStateAction CreateAction() => new DeadAction();
    
    #endregion
}

public class DeadAction : FiniteStateAction
{
    #region Fields

    private Animator _animator;

    // Property (Origin SO)
    private new MovementDeadActionSO OriginSO => base.OriginSO as MovementDeadActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _animator = finiteStateMachine.GetComponent<Animator>();
    }
    
    public override void FiniteStateEnter()
    {
        if (OriginSO.Moment == SpecificMoment.OnEnter)
        {
            _animator.SetTrigger("IsDead");
        }
    }

    public override void FiniteStateExit()
    {
        if (OriginSO.Moment == SpecificMoment.OnExit)
        {
            
        }
    }
    
    public override void FiniteStateUpdate() 
    { 
        
    }
    
    public override void FiniteStateFixedUpdate()
    {
        
    }

    #endregion
}