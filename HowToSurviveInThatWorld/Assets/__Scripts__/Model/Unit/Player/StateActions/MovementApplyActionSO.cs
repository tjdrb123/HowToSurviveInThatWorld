using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "MovementApplyAction", menuName = "State Machine/Actions/Movement Apply Action")]
public class MovementApplyActionSO : FiniteStateActionSO<MovementApplyAction>
{
}

public class MovementApplyAction : FiniteStateAction
{
    #region Fields

    private Player _playerScript;
    private CharacterController _characterController;
    private Animator _animator;

    #endregion


    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerScript = finiteStateMachine.GetComponent<Player>();
        _characterController = finiteStateMachine.GetComponent<CharacterController>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }

    public override void FiniteStateUpdate()
    {
        // None
    }

    public override void FiniteStateFixedUpdate()
    {
        _characterController.Move(_playerScript.MovementVector * Time.fixedDeltaTime);
        _playerScript.MovementVector = _characterController.velocity;

        //Debug.Log(_playerScript.MovementInput.x); //-> 범위 -1 ~ 1
        //Debug.Log(_playerScript.MovementInput.z); //-> 범위 -1 ~ 1
        
        _animator.SetFloat("Horizontal", _playerScript.MovementInput.x);
        _animator.SetFloat("Vertical", _playerScript.MovementInput.z);

        // -0.5 에서 0.5 범위에서는 walk애니메이션이 돌아가고,
        // -1.0 에서 1.0 범위에서는 run 애니메이션이 돌아가게 ..
    }

    #endregion
}