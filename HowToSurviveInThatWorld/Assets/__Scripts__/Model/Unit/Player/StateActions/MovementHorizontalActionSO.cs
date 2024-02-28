
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

    private PlayerController _playerController;

    // Property (Origin SO)
    private new MovementHorizontalActionSO OriginSO => base.OriginSO as MovementHorizontalActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
        if (OriginSO.MovementSpeed > 2.0f)
        {
            Manager_UnitEvent.Instance.OnMoveSoundEnemy(); // 이동 소리 이벤트
        }
        else
        {
            Manager_UnitEvent.Instance.OutMoveSoundEnemy();
        }
    }
    
    public override void FiniteStateFixedUpdate()
    {
        OriginSO.MovementSpeed = _playerController.IsRunning ? 4.2f : (_playerController.IsCrouching ? 1.5f : 2.7f);
        _playerController.MovementVector.x = _playerController.MovementInput.x * OriginSO.MovementSpeed;
        _playerController.MovementVector.z = _playerController.MovementInput.z * OriginSO.MovementSpeed;
    }

    #endregion
}