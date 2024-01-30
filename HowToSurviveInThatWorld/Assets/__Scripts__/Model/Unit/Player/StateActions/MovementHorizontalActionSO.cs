
using UnityEngine;

[CreateAssetMenu(fileName = "MovementHorizontalAction", menuName = "State Machine/Actions/Movement Horizontal Action")]
public class MovementHorizontalActionSO : FiniteStateActionSO<MovementHorizontalAction>
{
    #region Field

    public float MovementSpeed = 8f;

    #endregion
}

public class MovementHorizontalAction : FiniteStateAction
{
    #region Fields

    private Player _playerScript;

    // Property (Origin SO)
    private new MovementHorizontalActionSO OriginSO => base.OriginSO as MovementHorizontalActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerScript = finiteStateMachine.GetComponent<Player>();
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
    }
    
    public override void FiniteStateFixedUpdate()
    {
        _playerScript.MovementVector.x = _playerScript.MovementInput.x * OriginSO.MovementSpeed;
        _playerScript.MovementVector.z = _playerScript.MovementInput.z * OriginSO.MovementSpeed;
    }

    #endregion
}