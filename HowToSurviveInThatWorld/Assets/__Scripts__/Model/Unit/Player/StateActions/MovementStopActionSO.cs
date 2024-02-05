
using UnityEngine;

[CreateAssetMenu(fileName = "MovementStopAction", menuName = "State Machine/Actions/Movement Stop Action")]
public class MovementStopActionSO : FiniteStateActionSO
{
    #region Property (Override)
    
    [SerializeField] private FiniteStateAction.SpecificMoment _moment;
    
    // Properties
    public FiniteStateAction.SpecificMoment Moment => _moment;
    
    protected override FiniteStateAction CreateAction() => new MovementStopAction();
    
    #endregion
}

public class MovementStopAction : FiniteStateAction
{
    #region Fields

    private Player _playerScript;
    private Animator _animator;

    // Property (Origin SO)
    private new MovementStopActionSO OriginSO => base.OriginSO as MovementStopActionSO;

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
            _playerScript.MovementVector = Vector3.zero;
            _animator.SetFloat("Horizontal", 0);
            _animator.SetFloat("Vertical", 0);
        }
    }

    public override void FiniteStateExit()
    {
        if (OriginSO.Moment == SpecificMoment.OnExit)
        {
            _playerScript.MovementVector = Vector3.zero;
            
        }
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
    }
    
    public override void FiniteStateFixedUpdate()
    {
        if (OriginSO.Moment == SpecificMoment.OnFixedUpdate)
        {
            _playerScript.MovementVector = Vector3.zero;
        }
    }
    
    #endregion
}