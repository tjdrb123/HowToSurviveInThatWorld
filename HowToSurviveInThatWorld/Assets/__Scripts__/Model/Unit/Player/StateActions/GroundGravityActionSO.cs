
using UnityEngine;

[CreateAssetMenu(fileName = "GroundGravityAction", menuName = "State Machine/Actions/Ground Gravity Action")]
public class GroundGravityActionSO : FiniteStateActionSO<GroundGravityAction>
{
    #region Field

    [Tooltip("수직 이동 수치, 중력 값.")] 
    public float VerticalPull = -9.8f;

    #endregion
}

public class GroundGravityAction : FiniteStateAction
{
    #region Fields

    private PlayerController _playerController;

    // Property (Origin SO)
    private new GroundGravityActionSO OriginSO => base.OriginSO as GroundGravityActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
    }
    
    public override void FiniteStateFixedUpdate()
    {
        _playerController.MovementVector.y = OriginSO.VerticalPull;
    }

    #endregion
}