using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBasicBT : MonoBehaviour
{
    #region Global Variable

    private Animator _animator;
    private Transform _detectedPlayer;
    private NavMeshAgent _agent;
    
    private DataContext _enemyData;
    private BehaviorTreeRunner _btRunner;

    [Header("Distance")] 
    [SerializeField] 
    private float _detectDistance = 10f;
    
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
    private float _idleStartTime;
    private bool _idleWaitCheck;

    [Header("Animations")]
    private const string _WALK_ANIM_BOOL_NAME = "IsWalk";

    [Header("NavMeshAgent")]
    [SerializeField]
    private float _agentSpeed = 0.5f;
    [SerializeField]
    private float _agentStoppingDistance = Mathf.Epsilon; // 이동 중지 거리
    [SerializeField]
    private bool _agentUpdateRotation = true; // 자동 방향전환 여부
    [SerializeField]
    private float _agentacceleartion = 50f; // 가속도
    [SerializeField]
    private float _agentCorrectionDistance = 1f; // 보정 거리 (버그 방지) 
    [SerializeField] 
    private float _angularSpeed = 400f;  // Angular Speed : Agent 회전 속도 (프로퍼티)(회전 속도 : degree/sec)
    
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
                                    new ActionNode(CorrectPositionCheck), // 올바른 목적지가 있는가?
                                    new SequenceNode
                                    (
                                        new List<INode>()
                                        {
                                            new ActionNode(CheckArrivalAtDestination), // 목적지에 도착헸는가?
                                            new ActionNode(IdleWaitTimeCheck) // 목적지에 도착했는가?
                                        }
                                    ),
                                    new ActionNode(MoveToPosition) // 이동
                                }
                            )
                        }
                    )
                ),
                /* 순찰&대기 실험을 위해 잠시 주석처리.
                new SequenceNode    // ## 공격실행 판정 분기 노드
                (
                    new List<INode>()
                    {
                        new ActionNode(Test1),
                        new ActionNode(Test2),
                        new ActionNode(Test3)
                    }
                ),
                new ActionNode(Test1) // 추적
                */
            }
        );
    }
    
    #endregion
    
    

    # region Action(Leaf) Nodes

    #region Detect Player Node

    // 범위안에 적이 있는가? 
    // 범위 거리 체크할 Distance,범위 안에 있다면 플레이어의 위치 정보를 가져올 Transform
    // 이 쪽에서 플레이어가 존재시에 Transform에 플레이어 정보를 저장한다.
    // 만약 Transform != null 이라면 할당하지 않고, null 일때에만 할당한다.
    // 플레이어가 감지 범위 밖으로 나간다면 Transform을 null으로 만들어준다.
    // 이런식으로 최대한 Physics2D.OverlapSphere 를 적게 사용해야 불필요한 연산을 줄일 수 있다.
    private INode.E_NodeState CheckDetectPlayer()
    {
        var overlapColliders =
            Physics.OverlapSphere(transform.position, _detectDistance, LayerMask.GetMask("Player"));
        
        // 추후 범위 안에 있으면 한번만 할당하게 변경
        if (overlapColliders != null & overlapColliders.Length> 0)
        {
            _detectedPlayer = overlapColliders[0].transform;

            return INode.E_NodeState.ENS_Success;
        }
        
        // 추후 범위 밖으로 나가면 한번만 비우게 변경
        _detectedPlayer = null;
        
        return INode.E_NodeState.ENS_Failure;
    }

    #endregion

    #region Correct Destination Check & Patrol/Idle Node

    // 올바른 목적지 체크 (랜덤 목적지 부여)
    private INode.E_NodeState CorrectPositionCheck()
    {
        // 랜덤 목적지 배정
        // ##이전 랜덤 포지션에서 너무 가까운 거리는 다시 찍지 못하도록 리팩토링 필요.##
        if (_detectedPlayer == null & _patrolRandomPosCheck == true)
        {
            _correctPos.x = Random.Range(_patrolMinPos.x, _patrolMaxPos.x);
            _correctPos.z = Random.Range(_patrolMinPos.y, _patrolMaxPos.y);

            _patrolRandomPosCheck = false;
            return INode.E_NodeState.ENS_Success;
        }
        
        /* ####노드를 따로 분리할지 고민중.#### */
        // 경로가 유요하지 않거나 초기화되지 않은지 체크
        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            DebugLogger.LogError("Agent Path is Invalid");
            
            _patrolRandomPosCheck = true;
            return INode.E_NodeState.ENS_Running;
        }
        
        return INode.E_NodeState.ENS_Failure;
    }
    
    // 목적지 도착했는가? ( 남은거리 체크 -> agent.remainingDistance < _agentCorrectionDistance)
    private INode.E_NodeState CheckArrivalAtDestination()
    {
        if (_agent.pathPending)
        {
            return INode.E_NodeState.ENS_Running;
        }
        
        if (_agent.remainingDistance < Mathf.Epsilon)
        {
            return INode.E_NodeState.ENS_Success;
        }
        
        // ##Failure하여 이동로직으로 이동하여 불필요하게 SetDesstiation을 계속 찍어준다. 리팩토링 필요.##
        return INode.E_NodeState.ENS_Failure;
    }
    
    // 대기시간이 남아있는가? ( 대기시간 체크 -> Coroutine or Time.time ) Running , End -> _patrolRandomPosCheck = true;
    private INode.E_NodeState IdleWaitTimeCheck()
    {
        // 최초 한번 시간 할당.
        if (_idleWaitCheck == true)
        {
            AnimationIdleCheck();
            _idleDurationTime = Random.Range(0f, 3f);
            _idleStartTime = Time.time;
            _idleWaitCheck = false;
        }

        if (Time.time - _idleStartTime > _idleDurationTime)
        {
            _patrolRandomPosCheck = true;
            _idleWaitCheck = true;
            // ##Failure하여 이동로직으로 이동하여 불필요하게 SetDesstiation을 계속 찍어준다. 리팩토링 필요.##
            return INode.E_NodeState.ENS_Failure;
        }

        return INode.E_NodeState.ENS_Running;
    }
    
    // 이동로직 (agent에게 목적지 할당. -> _agent.destination = _correctPos;) Running
    private INode.E_NodeState MoveToPosition()
    {
        AnimationWalkCheck();
        _agent.SetDestination(_correctPos);
        
        return INode.E_NodeState.ENS_Running;
    }

    #endregion

    #region Attack Check/Excute Node
    
    // 공격중인가?
    // 공격범위 안에 적이 있는가?
    // 공격 실행

    #endregion

    #region Tracking Node
    
    // 적 발견시 Agent의 목적지를 Player로 할당.    
    // 추적 로직

    #endregion


    # endregion



    #region Action Logics

    private void TemporaryMethod()
    {
        
    }
    
    private INode.E_NodeState Test1()
    {
        return INode.E_NodeState.ENS_Running;
    }
    
    private INode.E_NodeState Test2()
    {
        return INode.E_NodeState.ENS_Running;
    }
    
    private INode.E_NodeState Test3()
    {
        return INode.E_NodeState.ENS_Running;
    }

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
        _agent.angularSpeed = _angularSpeed; // 회전 속도
    }

    #endregion

    private void AnimationWalkCheck()
    {
        if (!_animator.GetBool(_WALK_ANIM_BOOL_NAME))
        {
            _animator.SetBool(_WALK_ANIM_BOOL_NAME, true);
        }
    }

    private void AnimationIdleCheck()
    {
        if (_animator.GetBool(_WALK_ANIM_BOOL_NAME))
        {
            _animator.SetBool(_WALK_ANIM_BOOL_NAME, false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _detectDistance);
    }
}
