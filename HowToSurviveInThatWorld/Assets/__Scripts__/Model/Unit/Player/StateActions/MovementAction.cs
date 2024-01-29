
using UnityEngine;

[CreateAssetMenu(fileName = "MovementAction", menuName = "State Machine/Actions/Movement")]
public class MovementActionSO : FiniteStateActionSO<MovementAction> { }

public class MovementAction : FiniteStateAction
{
    #region Components

    private CharacterController _characterController;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _characterController = finiteStateMachine.GetComponent<CharacterController>();
    }

    #endregion
    
    public override void FiniteStateUpdate() { }
    public override void FiniteStateFixedUpdate()
    {
        // _characterController.Move()
    }
}
