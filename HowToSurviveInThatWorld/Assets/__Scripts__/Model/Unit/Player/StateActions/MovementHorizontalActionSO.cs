
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
    private bool _currentRunCheck;
    private bool _currentCrouchCheck;
    private bool _stopMoveCheck;
    // Property (Origin SO)
    private new MovementHorizontalActionSO OriginSO => base.OriginSO as MovementHorizontalActionSO;

    #endregion

    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
    }
    public override void FiniteStateEnter()
    {
        PlayerMoveStateCheck();
    }
    public override void FiniteStateExit()
    {
        Manager_Sound.instance.AudioStop(_playerController.gameObject);
    }

    public override void FiniteStateUpdate()
    {
        PlayerStopSoundCheck();
        PlayerMoveSoundCheck();
        
        // Player Move Detected Enemy ( 걷기, 뛰기 각각 감지 범위 다르게 리팩토링 필요 )
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
    

    #region Default Method

    private void PlayerStopSoundCheck()
    {
        // Player Stop Sound Check
        if ((_playerController.MovementVector.x == 0 && _playerController.MovementVector.z == 0) && _stopMoveCheck)
        {
            Manager_Sound.instance.AudioStop(_playerController.gameObject);
            _stopMoveCheck = false;
        }
        else if ((_playerController.MovementVector.x != 0 && _playerController.MovementVector.z != 0) && !_stopMoveCheck)
        {
            PlayerMoveStateCheck();
            _stopMoveCheck = true;
        }
    }

    private void PlayerMoveSoundCheck()
    {
        // Player Move Sound Check
        if (_currentRunCheck != _playerController.IsRunning)
        {
            Manager_Sound.instance.AudioStop(_playerController.gameObject);
            PlayerMoveStateCheck();
        }
    }
    
    private void PlayerMoveStateCheck()
    {
        if (_playerController.IsRunning)
        {
            Manager_Sound.instance.AudioPlay(_playerController.gameObject, "Sounds/SFX/Player/Run", true, false);
        }
        else
        {
            Manager_Sound.instance.AudioPlay(_playerController.gameObject, "Sounds/SFX/Player/Walk", true, false);
        }
        _currentRunCheck = _playerController.IsRunning;
    }

    #endregion
}