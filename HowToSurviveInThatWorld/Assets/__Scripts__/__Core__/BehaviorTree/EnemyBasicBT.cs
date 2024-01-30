using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyBasicBT : MonoBehaviour
{
    #region Global Variable

    private Animator _animator;
    public Transform _detectedPlayer;
    private NavMeshAgent _agent;
    
    private DataContext _enemyData;
    private BehaviorTreeRunner _btRunner;
    
    /// <summary>
    /// 테스트를 위해 SerializeField 사용. 추후 각 데이터 컨택스트로 옮기고 클래스별로 분류 예정.
    /// </summary>
    [Header("Distance")] 
    [SerializeField] 
    public float _detectDistance = 10f;
    [SerializeField] 
    private float _detectViewAngle = 45;
    [SerializeField]
    private float _attackDistance = 1f;
    
    [Header("PatrolPosition")]
    [SerializeField]
    private Vector2 _patrolMinPos = Vector2.one * -20;
    [SerializeField]
    private Vector2 _patrolMaxPos = Vector2.one * 20;
    [SerializeField] 
    private Vector3 _correctPos;
    [SerializeField] 
    private bool _patrolRandomPosCheck = true;

    [Header("IdleTime")]
    [SerializeField]
    private float _idleDurationTime;
    [SerializeField]
    private float _idleStartTime;
    [SerializeField]
    private bool _idleWaitCheck;

    [Header("Animations")]
    private const string _WALK_ANIM_BOOL_NAME = "IsWalk";
    private const string _RUN_ANIM_BOOL_NAME = "IsRun";
    private const string _ATTACK_ANIM_BOOL_NAME = "IsAttack";
    private const string _ATTACK_ANIM_STATE_NAME = "Attack";

    [Header("LayerMask")] 
    private readonly LayerMask _PLAYER_LAYER_MASK = 1 << 6;
    private readonly LayerMask _ENEMY_LAYER_MASK = 1 << 7;

    [Header("NavMeshAgent")]
    [SerializeField]
    private float _agentSpeed = 0.1f;
    [SerializeField] 
    private float _agentTrackingSpeed = 3f;
    [SerializeField]
    private float _agentStoppingDistance = 0.5f; // 이동 중지 거리 (Mathf.Epsilon은 너무 거리가 짧아 애니메이션이 고장남)
    [SerializeField]
    private bool _agentUpdateRotation = true; // 자동 방향전환 여부
    [SerializeField]
    private float _agentacceleartion = 50f; // 가속도
    [SerializeField]
    private float _agentCorrectionDistance = 1f; // 보정 거리 (버그 방지) 
    [SerializeField] 
    private float _agentAngularSpeed = 400f;  // Angular Speed : Agent 회전 속도 (프로퍼티)(회전 속도 : degree/sec)
    
    #endregion
    
    

    #region Unity Event Method

    private void Awake()
    {
        InitializeData();
        InitializeAgent();

    }

    private void Update()
    {
        _btRunner.Operate(); // BT 순회
    }

    #endregion
    
    

    # region Setting BT
    
    private INode SettingBT()
    {
        return new SelectorNode
        (
            new List<INode>()
            {
                new Inverter
                (
                    new SelectorNode    // ## 적 감지 & 순찰 분기 노드
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckDetectPlayer), // 범위 안에 적이 있는가?
                            new SelectorNode    // ## 순찰 판정 분기 노드
                            (
                                new List<INode>()
                                {
                                    new ActionNode(RandomPositionAssignment), // 랜덤 목적지 부여 여부
                                    new ActionNode(CorrectPathCheck), // 목적지 까지의 경로가 유효한가?
                                    new SequenceNode
                                    (
                                        new List<INode>()
                                        {
                                            new ActionNode(CheckArrivalAtDestination), // 목적지에 도착헸는가?
                                            new ActionNode(IdleWaitTimeCheck) // 목적지에 도착했는가?
                                        }
                                    )
                                }
                            )
                        }
                    )
                ),
                new SequenceNode    // ## 공격실행 판정 분기 노드
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckAttacking), // 공격중인가?
                        new ActionNode(CheckPlayerWithineAttackDistance), // 공격범위 안에 플레이어가 있는가?
                        new ActionNode(DoAttack) // 공격
                    }
                ),
                new ActionNode(DoTracking) // 추적
                
            }
        );
    }
    
    #endregion
    
    

    # region Action(Leaf) Nodes

    #region Detect Player Node
    
    /// <summary>
    /// 범위 거리 체크할 Distance,범위 안에 있다면 플레이어의 위치 정보를 가져올 Transform
    /// 이 쪽에서 플레이어가 존재시에 Transform에 플레이어 정보를 저장한다.
    /// 만약 Transform != null 이라면 할당하지 않고, null 일때에만 할당한다.
    /// 플레이어가 감지 범위 밖으로 나간다면 Transform을 null으로 만들어준다.
    /// 이런식으로 최대한 Physics2D.OverlapSphere 를 적게 사용해야 불필요한 연산을 줄일 수 있다.
    /// </summary>

    // 범위안에 적이 있는가? 
    private INode.E_NodeState CheckDetectPlayer()
    {
        var overlapColliders =
            Physics.OverlapSphere(transform.position, _detectDistance, _PLAYER_LAYER_MASK);
        
        // 추후 범위 안에 있으면 한번만 할당하게 변경
        if (overlapColliders != null & overlapColliders.Length> 0)
        {
            // FOV (Field Of View)
            // 범위 안에 플레이어가 있다면, 그 플레이어가 FOV 범위 안에 있는지 확인한다.
            // FOV 범위 안에 있다면, 플레이어한테 Ray를 쏜다.
            // 레이 사이에 장애물이 감지되지 않고 플레이어가 맞는다면, 추적을 한다.
            // FOV 범위 밖에 있다면, 감지가 되지 않는다.

            Transform undefinedPlayer = overlapColliders[0].transform;
            Vector3 directionToPlayer = (undefinedPlayer.position - transform.position).normalized;
            
            /*===========================================================================================*/
            
            if (Vector3.Dot(transform.forward, directionToPlayer) > 0.9f)
            {
                float distanceToTarget = Vector3.Distance(undefinedPlayer.position ,transform.position);

                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToTarget, _ENEMY_LAYER_MASK))
                {
                    // 순찰 노드 최초확인 bool값 초기화.
                    _patrolRandomPosCheck = true;
                    _idleWaitCheck = true;
                    
                    _detectedPlayer = undefinedPlayer;
                }
            }
            
            /*===========================================================================================*/

            return INode.E_NodeState.ENS_Success;
        }

        // 추후 범위 밖으로 나가면 한번만 비우게 변경
        _detectedPlayer = null;
        
        return INode.E_NodeState.ENS_Failure;
    }

    #endregion

    #region Correct Destination Check & Patrol/Idle Node
    
    /*================================================================================================*/

    // 목적지가 있는가? (_correctPos의 좌표에 랜덤값 할당 & 목적지 셋팅)
    private INode.E_NodeState RandomPositionAssignment()
    {
        // 랜덤 목적지 배정
        // ##이전 랜덤 포지션에서 너무 가까운 거리는 다시 찍지 못하도록 리팩토링 필요.##
        if (_detectedPlayer == null & _patrolRandomPosCheck == true)
        {
            _correctPos.x = Random.Range(_patrolMinPos.x, _patrolMaxPos.x);
            _correctPos.z = Random.Range(_patrolMinPos.y, _patrolMaxPos.y);

            NavMeshAgentPatrolSetting();
        }
   
        return INode.E_NodeState.ENS_Failure;
    }
    
    /*================================================================================================*/
    
    // 목적지 까지의 경로가 유효한가?
    private INode.E_NodeState CorrectPathCheck()
    {
        // 경로가 유요하지 않거나 초기화되지 않은지 체크
        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            DebugLogger.LogError("Agent Path is Invalid");
            
            _patrolRandomPosCheck = true;
            return INode.E_NodeState.ENS_Running;
        }
        
        return INode.E_NodeState.ENS_Failure;
    }
    
    /*================================================================================================*/
    
    // 목적지 도착했는가? ( 남은거리 체크 -> agent.remainingDistance < _agentCorrectionDistance)
    private INode.E_NodeState CheckArrivalAtDestination()
    {
        if (_agent.pathPending)
        {
            return INode.E_NodeState.ENS_Running;
        }
        
        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            return INode.E_NodeState.ENS_Success;
        }
        
        return INode.E_NodeState.ENS_Failure;
    }
    
    /*================================================================================================*/
    
    // 대기시간이 남아있는가? ( 대기시간 체크 -> Coroutine or Time.time ) Running , End -> _patrolRandomPosCheck = true;
    private INode.E_NodeState IdleWaitTimeCheck()
    {
        // 최초 한번 시간 할당.
        if (_idleWaitCheck == true)
        {
            IsAnimationIdleCheck();
            _idleDurationTime = Random.Range(3f, 5f);
            _idleStartTime = Time.time;
            _idleWaitCheck = false;
        }

        if (Time.time - _idleStartTime > _idleDurationTime)
        {
            _patrolRandomPosCheck = true;
            _idleWaitCheck = true;

            return INode.E_NodeState.ENS_Failure;
        }

        return INode.E_NodeState.ENS_Running;
    }
    
    /*================================================================================================*/

    #endregion

    #region Attack Check/Excute Node
    
    /*================================================================================================*/
    
    // 공격중인가?
    private INode.E_NodeState CheckAttacking()
    {
        if (IsAnimationRunning(_ATTACK_ANIM_STATE_NAME))
        {
            _agent.speed = 0;
            return INode.E_NodeState.ENS_Running;
        }

        return INode.E_NodeState.ENS_Success;
    }
    
    /*================================================================================================*/
    
    // 공격범위 안에 적이 있는가?
    private INode.E_NodeState CheckPlayerWithineAttackDistance()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) <
                (_attackDistance * _attackDistance))
            {
                NavMeshAgentAttackSetting();
                return INode.E_NodeState.ENS_Success;
            }
        }
        
        return INode.E_NodeState.ENS_Failure;
    }
    
    /*================================================================================================*/
    
    // 공격 실행
    private INode.E_NodeState DoAttack()
    {
        if (_detectedPlayer != null)
        {
            IsAnimationAttackCheck();
            return INode.E_NodeState.ENS_Success;
        }

        return INode.E_NodeState.ENS_Failure;
    }
    
    /*================================================================================================*/

    #endregion

    #region Tracking Node
    
    /*================================================================================================*/
    
    // 적 발견시 Agent의 목적지를 Player로 할당.    
    // 추적 로직
    private INode.E_NodeState DoTracking()
    {
        if (_detectedPlayer != null)
        {
            NavMeshAgentTrackingSetting();
            _agent.SetDestination(_detectedPlayer.position);
        }

        return INode.E_NodeState.ENS_Failure;
    }
    
    /*================================================================================================*/

    #endregion


    # endregion



    #region Action Inside Logics

    # region Animations Logic

    private bool IsAnimationRunning(string animationName)
    {
        if (_animator != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName((animationName)))
            {
                var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                return normalizedTime != 0 && normalizedTime < 1f;
            }
        }
        
        return false;
    }
    
    private void IsAnimationWalkCheck()
    {
        if (!_animator.GetBool(_WALK_ANIM_BOOL_NAME) || _animator.GetBool(_RUN_ANIM_BOOL_NAME))
        {
            _animator.SetBool(_ATTACK_ANIM_BOOL_NAME, false);
            _animator.SetBool(_RUN_ANIM_BOOL_NAME, false);
            _animator.SetBool(_WALK_ANIM_BOOL_NAME, true);
        }
    }

    private void IsAnimationIdleCheck()
    {
        if (_animator.GetBool(_WALK_ANIM_BOOL_NAME) || _animator.GetBool(_RUN_ANIM_BOOL_NAME))
        {
            _animator.SetBool(_RUN_ANIM_BOOL_NAME, false);
            _animator.SetBool(_WALK_ANIM_BOOL_NAME, false);
        }
    }

    private void IsAnimationRunCheck()
    {
        if (!_animator.GetBool(_RUN_ANIM_BOOL_NAME))
        {
            _animator.SetBool(_RUN_ANIM_BOOL_NAME, true);
        }

        if (_animator.GetBool(_ATTACK_ANIM_BOOL_NAME))
        {
            _animator.SetBool(_ATTACK_ANIM_BOOL_NAME, false);
        }
    }

    private void IsAnimationAttackCheck()
    {
        if (!_animator.GetBool(_ATTACK_ANIM_BOOL_NAME))
        {
            _animator.SetBool(_ATTACK_ANIM_BOOL_NAME, true);
        }
    }

    # endregion

    # region NavMeshAgent Setting

    private void NavMeshAgentAttackSetting()
    {
        IsAnimationAttackCheck();
        
        _animator.applyRootMotion = false;
        _agent.isStopped = true;
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.velocity = Vector3.zero;
    }

    private void NavMeshAgentTrackingSetting()
    {
        IsAnimationRunCheck();

        _animator.applyRootMotion = false;
        _agent.speed = _agentTrackingSpeed;
        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
    }

    private void NavMeshAgentPatrolSetting()
    {
        IsAnimationWalkCheck();
        
        _animator.applyRootMotion = true;
        _agent.speed = _agentSpeed;
        _agent.SetDestination(_correctPos);
        _patrolRandomPosCheck = false;
        
        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
    }

    # endregion



    #endregion
    
    
    
    #region Initializer

    private void InitializeData()
    {
        //_enemyData = DataContext.CreatDataContext(this.gameObject);
        _btRunner = new BehaviorTreeRunner(SettingBT());
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _idleDurationTime = Random.Range(1f, 3f);
        _idleWaitCheck = true;
    }

    private void InitializeAgent()
    {
        _agent.stoppingDistance = _agentStoppingDistance;   // 정지 거리
        _agent.speed = _agentSpeed; // 이동 속도
        _agent.destination = _correctPos; // 목적지
        _agent.updateRotation = _agentUpdateRotation; // 회전 유무
        _agent.acceleration = _agentacceleartion; // 가속도
        _agent.angularSpeed = _agentAngularSpeed; // 회전 속도
    }

    #endregion
    
/*
    private void OnDrawGizmos()
    {
        private Vector3 leftViewAngle;
        private Vector3 rightViewAngle;

        private float myVecMag;
        private float youVecMag;
    
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _detectDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _attackDistance);

        Gizmos.color = Color.blue;
        leftViewAngle = new Vector3
            (Mathf.Sin(-_detectViewAngle * 0.5f * Mathf.Deg2Rad), 0, Mathf.Cos(-_detectViewAngle * 0.5f * Mathf.Deg2Rad));
        rightViewAngle = new Vector3
            (Mathf.Sin(_detectViewAngle * 0.5f * Mathf.Deg2Rad), 0, Mathf.Cos(_detectViewAngle * 0.5f * Mathf.Deg2Rad));
        Gizmos.DrawLine(transform.position, transform.position + leftViewAngle * 10f);
        Gizmos.DrawLine(transform.position, transform.position + rightViewAngle * 10f);
        
        Gizmos.color = Color.black;
        float vie = Mathf.Acos(0.9f);
        Vector3 leftView = new Vector3
            (Mathf.Cos(-vie * 0.5f), 0, Mathf.Sin(-vie * 0.5f));
        Vector3 rightView = new Vector3
            (Mathf.Cos(vie * 0.5f), 0, Mathf.Sin(vie * 0.5f));
        Gizmos.DrawLine(transform.position, transform.position + leftView * 10f);
        Gizmos.DrawLine(transform.position, transform.position + rightView * 10f);

    }
    */
}
