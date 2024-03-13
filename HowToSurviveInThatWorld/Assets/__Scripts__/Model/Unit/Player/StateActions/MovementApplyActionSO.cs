using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "MovementApplyAction", menuName = "State Machine/Actions/Movement Apply Action")]
public class MovementApplyActionSO : FiniteStateActionSO<MovementApplyAction>
{
}

public class MovementApplyAction : FiniteStateAction
{
    #region Fields

    private PlayerController _playerController;
    private CharacterController _characterController;
    private Animator _animator;
    private bool _isMoving;

    #endregion


    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
        _characterController = finiteStateMachine.GetComponent<CharacterController>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }

    public override void FiniteStateUpdate()
    {
        // None
    }

    public override void FiniteStateFixedUpdate()
    {
        _characterController.Move(_playerController.MovementVector * Time.fixedDeltaTime);
        _playerController.MovementVector = _characterController.velocity;
        _isMoving = _playerController.MovementVector != Vector3.zero;

        _animator.SetBool("IsWalking", _isMoving);
        _animator.SetBool("IsRunning", _isMoving && _playerController.IsRunning);
    }

    #endregion
}