
using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CreateAssetMenu(fileName = "MovementAttackAction", menuName = "State Machine/Actions/Movement Attack Action")]
public class MovementAttackActionSO : FiniteStateActionSO
{
    #region Property (Override)
    
    [SerializeField] private FiniteStateAction.SpecificMoment _moment;
    
    // Properties
    
    public FiniteStateAction.SpecificMoment Moment => _moment;
    protected override FiniteStateAction CreateAction() => new MovementAttackAction();
    
    #endregion
}

public class MovementAttackAction : FiniteStateAction
{
    #region Fields

    private PlayerController _playerController;
    private Animator _animator;
    
    /*  */
    
    
    private Collider[] _targetColliders;
    private Transform _enemy;
    private const float _attackDistance = 1f;
    private float _weaponDistance;
    private float _animationTime;
    private float _weaponAimTime;
    private bool _isAttack;
    private readonly LayerMask _ENEMY_LAYER_MASK = 1 << 7;
    private readonly LayerMask _OBSTACLE_LAYER_MASK = 1 << 9;
    private readonly string _ATTACK_ANIM_NAME = "Attack";

    private float _weaponType;
    /*  */
    
    // Property (Origin SO)
    private new MovementAttackActionSO OriginSO => base.OriginSO as MovementAttackActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }
    
    public override void FiniteStateEnter()
    {
        if (OriginSO.Moment == SpecificMoment.OnEnter)
        {
            _animator.SetTrigger("IsAttacking");
            
            _isAttack = true;
            _weaponType = WeaponTypeInfo(); // 현재 무기 타입 할당.
            WeaponTypeSetting(); // 무기 타입에 따른 셋팅 설정.
            _enemy = EnemyCheck(); // 가까운 Enemy 할당.
            TargetDirectionCompensate(); // Enemy 바라보기
            Manager_UnitEvent.Instance.OnAttackSoundEnemy(_weaponType); // 공격 사운드 이벤트
            // 웨폰 타입에 따라 공격 사거리, 공격력, 다르게 설정
        }
    }

    public override void FiniteStateExit()
    {
        _animator.ResetTrigger("IsAttacking");
        _playerController.IsCrouching = false;
        
    }
    
    public override void FiniteStateUpdate() 
    { 
        Debug.DrawRay(_playerController.transform.position + (Vector3.up * 1.0f),
            _playerController.transform.forward * (_attackDistance * _weaponDistance), Color.blue);
        // None
        // Player 와 Enemy 의 거리가 공격범위 안에 있다면
        if (PlayerToEnemyDistance())
        {
            // 애니메이션 진행도 체크
            if (AnimationProgress())
            {
                // 데미지 적용
                ApplyAttackDamage();
            }
        }
    }
    
    public override void FiniteStateFixedUpdate()
    {
        // None
    }
    
    // Dot 안에 있는 Enemy들 중 가장 가까운 Enemy 타겟
    // 그 Enemy 에게 Ray를 쏴서 장애물이 있는지 검사
    // 데미지 적용
    
    // WeaponType 셋팅
    private void WeaponTypeSetting()
    {
        switch (_weaponType)
        {
            case 0:
                _weaponDistance = 1.5f;
                _weaponAimTime = 0.35f;
                _playerController.player.weaponTypeDamage = 40;
                Manager_Sound.instance.AudioPlay(_playerController.gameObject, "Sounds/SFX/Player/punch", false, false);
                break;
            case 1:
                _weaponDistance = 2f;
                _weaponAimTime = 0.35f;
                _playerController.player.weaponTypeDamage = 10;
                Manager_Sound.instance.AudioPlay(_playerController.gameObject, "Sounds/SFX/Player/NearAttack", false, false);
                break;
            case 2:
                _weaponDistance = 10f;
                _weaponAimTime = 0.05f;
                _playerController.player.weaponTypeDamage = -20;
                Manager_Sound.instance.AudioPlay(_playerController.gameObject, "Sounds/SFX/Player/pistol", false, false);
                break;
            case 3:
                _weaponDistance = 15f;
                _weaponAimTime = 0.05f;
                _playerController.player.weaponTypeDamage = -40;
                Manager_Sound.instance.AudioPlay(_playerController.gameObject, "Sounds/SFX/Player/rifle", false, false);
                break;
        }
    }
    
    // 가장 가까운 Enemy 타겟팅
    private Transform EnemyCheck()
    {
        // 공격범위 안에 Enemy 서치
        var overlapColliders = Physics.OverlapSphere(_playerController.transform.position, _attackDistance * _weaponDistance
            , _ENEMY_LAYER_MASK);
        
        // 가장 가까운 Enemy 서치
        if (overlapColliders.Length > 0)
        {
            Collider collider = overlapColliders
                .OrderBy(c => (c.transform.position - _playerController.transform.position).sqrMagnitude)
                .FirstOrDefault();

            Vector3 directionToEnemy = (collider.transform.position - _playerController.transform.position).normalized;

            // Enemy 까지 장애물이 걸리는게 없는지 체크
            if (collider != null && !Physics.Raycast(_playerController.transform.position + (Vector3.up * 1.0f),
                    directionToEnemy, _attackDistance * _weaponDistance, _OBSTACLE_LAYER_MASK))
            {
                return collider.transform;
            }
            
        }

        return null;
    }
    
    // Enemy와 Player의 거리 체크
    private bool PlayerToEnemyDistance()
    {
        if (_enemy == null)
            return false;

        return Vector3.SqrMagnitude(_playerController.transform.position - _enemy.transform.position) <=
                             (_attackDistance * _attackDistance) * (_weaponDistance * _weaponDistance);
    }
    
    // 애니메이션 진행도에 따라 데미지 적용
    private bool AnimationProgress()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_ATTACK_ANIM_NAME))
        {
            _animationTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;

            return _animationTime > 0f && _animationTime < 0.99f;
        }

        return false;
    }

    private void ApplyAttackDamage()
    {
        if (_enemy != null && _animationTime > _weaponAimTime && _isAttack)
        {
            _playerController.player.ApplyDamage(_playerController.gameObject, _enemy.gameObject);
            EventMethodInvoke();
            _isAttack = false;
        }
    }
    
    // 타겟 Enemy 바라보기
    private void TargetDirectionCompensate()
    {
        if (_enemy == null)
            return;
        
        var directionToEnemy = (_enemy.transform.position - _playerController.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
        _playerController.transform.rotation = lookRotation;
        //_playerController.transform.rotation = Quaternion.Slerp(_playerController.transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private float WeaponTypeInfo()
    {
        return _animator.GetFloat("WeaponType");
    }
    
    /*=================================================================================================================*/

    private void EventMethodInvoke()
    {
        Manager_UnitEvent.Instance.OnDamagedEnemy(_enemy.gameObject); // 파티클, 애니메이션
        Manager_UnitEvent.Instance.OnHitEnemyAllocate(_playerController.gameObject, _enemy.gameObject); // 회전

        if (_weaponType >= 2 && _weaponType <= 3)
        {
            // Attack Event Invoke Logic
        }
    }
    
    #endregion
}