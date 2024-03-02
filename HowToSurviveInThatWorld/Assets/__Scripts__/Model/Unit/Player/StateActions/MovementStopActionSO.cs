
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

    private PlayerController _playerController;
    private Animator _animator;

    // Property (Origin SO)
    private new MovementStopActionSO OriginSO => base.OriginSO as MovementStopActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }
    
    public override void FiniteStateEnter()
    {
        // 애니메이션 WeaponType, AttackType 파라미터를 플레이어에 맞게 초기화 해야함
        _playerController.MovementVector = Vector3.zero;
    }

    public override void FiniteStateExit()
    {
        _playerController.MovementVector = Vector3.zero;  
    }
    
    public override void FiniteStateUpdate() 
    {
        WeaponItem weaponItem = null;
        if (Manager_Inventory.Instance.EquipMentSlotDatas[6] != null)
            weaponItem = Manager_Inventory.Instance.EquipMentSlotDatas[6] as WeaponItem;
        _animator.SetFloat("WeaponType", weaponItem == null ? 0 : (int)weaponItem.WeaponType);
    }
    
    public override void FiniteStateFixedUpdate()
    {
        if (OriginSO.Moment == SpecificMoment.OnFixedUpdate)
        {
            _playerController.MovementVector = Vector3.zero;
        }
    }
    
    #endregion
}