using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicZombieData
{
    private Animator _animator;
    public Transform _detectedPlayer;
    private NavMeshAgent _agent;
    
    private OldDataContext _enemyData;
    private OldBehaviorTreeRunner _btRunner;
    
    /// <summary>
    /// 테스트를 위해 SerializeField 사용. 추후 각 데이터 컨택스트로 옮기고 클래스별로 분류 예정.
    /// </summary>
    [Header("Distance")] 
    [SerializeField] 
    public float _detectDistance = 10f;
    //[SerializeField] 
    //private float _detectViewAngle = 45;
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
    //[SerializeField]
    //private float _agentCorrectionDistance = 1f; // 보정 거리 (버그 방지) 
    [SerializeField] 
    private float _agentAngularSpeed = 400f;  // Angular Speed : Agent 회전 속도 (프로퍼티)(회전 속도 : degree/sec)
}
