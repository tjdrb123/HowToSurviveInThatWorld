
using UnityEngine;

[CreateAssetMenu(fileName = "MovementApplyAction", menuName = "State Machine/Actions/Movement Apply Action")]
public class MovementApplyActionSO : FiniteStateActionSO<MovementApplyAction> { }

public class MovementApplyAction : FiniteStateAction
{
    #region Fields

    private Player _playerScript;
    private CharacterController _characterController;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerScript = finiteStateMachine.GetComponent<Player>();
        _characterController = finiteStateMachine.GetComponent<CharacterController>();
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
    }
    
    public override void FiniteStateFixedUpdate()
    {
        _characterController.Move(_playerScript.MovementVector * Time.fixedDeltaTime);
        _playerScript.MovementVector = _characterController.velocity;
    }

    #endregion
}