
using UnityEngine;

[CreateAssetMenu(fileName = "MovementRotationAction", menuName = "State Machine/Actions/Movement Rotation Action")]
public class MovementRotationActionSO : FiniteStateActionSO<MovementRotationAction>
{
    #region Field

    [Tooltip("이동 방향 벡터를 사용해 캐릭터를 회전 시킬 때의 Smooth 시간.")]
    public float TurnSmoothTime = 0.1f;

    #endregion
}

public class MovementRotationAction : FiniteStateAction
{
    #region Fields

    private PlayerController _playerController;
    private Player _player;

    private float _turnSmoothSpeed;
    private const float ROTATION_THRESHOLD = .02f;
    
    // Property (Origin SO)
    private new MovementRotationActionSO OriginSO => base.OriginSO as MovementRotationActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
        _player = finiteStateMachine.GetComponent<Player>();
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
    }
    
    public override void FiniteStateFixedUpdate()
    {
        _player.RotateSmoothByMovement(
            _playerController.MovementVector,
            OriginSO.TurnSmoothTime,
            ref _turnSmoothSpeed,
            ROTATION_THRESHOLD);
    }

    #endregion
}