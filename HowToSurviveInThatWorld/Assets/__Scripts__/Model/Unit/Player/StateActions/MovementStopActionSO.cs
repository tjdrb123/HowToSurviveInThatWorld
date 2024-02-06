
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

    // Property (Origin SO)
    private new MovementStopActionSO OriginSO => base.OriginSO as MovementStopActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerScript = finiteStateMachine.GetComponent<Player>();
    }
    
    public override void FiniteStateEnter()
    {
        // 애니메이션 WeaponType 파라미터를 플레이어에 맞게 초기화 해야함
        _playerScript.MovementVector = Vector3.zero;
    }

    public override void FiniteStateExit()
    {
        _playerScript.MovementVector = Vector3.zero;
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