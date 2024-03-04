using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicZombieData
{
    public Animator animator;
    public GameObject gameObject;
    public Transform transform;
    public Transform detectedPlayer;
    public NavMeshAgent agent;
    
    [Header("Distance")] 
    public float detectDistance = 8f;
    public float attackSoundDistance = 0f;
    public float moveSoundDistance = 3f;
    public float attackDistance = 1.5f;
    
    [Header("PatrolPosition")]
    public Vector2 patrolMinPos = Vector2.one;
    public Vector2 patrolMaxPos = Vector2.one * 150;
    public Vector3 correctPos;
    public bool patrolRandomPosCheck = true;

    [Header("IdleTime")]
    public float idleDurationTime;
    public float idleStartTime;
    public bool idleWaitCheck;

    [Header("Animations")]
    public const string WALK_ANIM_BOOL_NAME = "IsWalk";
    public const string RUN_ANIM_BOOL_NAME = "IsRun";
    public const string ATTACK_ANIM_BOOL_NAME = "IsAttack";
    public const string ATTACK_ANIM_STATE_NAME = "Attack";

    [Header("LayerMask")] 
    public readonly LayerMask PLAYER_LAYER_MASK = 1 << 6;
    public readonly LayerMask ENEMY_LAYER_MASK = 1 << 7;
    public readonly LayerMask OBSTACLE_LAYER_MASK = 1 << 9;

    [Header("NavMeshAgent")]
    public float agentSpeed = 0.1f;
    public float agentTrackingSpeed = 3f;
    public float agentStoppingDistance = 1f; // 이동 중지 거리 (Mathf.Epsilon은 너무 거리가 짧아 애니메이션이 고장남)
    public bool agentUpdateRotation = true; // 자동 방향전환 여부
    public float agentacceleration = 50f; // 가속도
    public float agentAngularSpeed = 10800f;  // Angular Speed : Agent 회전 속도 (프로퍼티)(회전 속도 : degree/sec)

    public Enemy enemy;
    public readonly float enemyDot = 0.9f;
    public bool hitCheck;
    public bool attackSoundCheck;
    public bool moveSoundCheck;
    public EnemyEffects effects;

    public bool zombieSoundCheck = true;

    #region LifeSetting

    public static BasicZombieData CreateBasicZombieData(GameObject gameObject)
    {
        BasicZombieData basicZombieData = new BasicZombieData();
        basicZombieData.gameObject = gameObject;
        basicZombieData.transform = gameObject.transform;
        basicZombieData.animator = gameObject.GetComponent<Animator>();
        basicZombieData.agent = gameObject.GetComponent<NavMeshAgent>();
        basicZombieData.InitializeAgent();

        basicZombieData.enemy = gameObject.GetComponent<Enemy>();
        basicZombieData.effects = gameObject.GetComponent<EnemyEffects>();
        
        return basicZombieData;
    }
    
    private void InitializeAgent()
    {
        agent.stoppingDistance = agentStoppingDistance;   // 정지 거리
        agent.speed = agentSpeed; // 이동 속도
        agent.destination = correctPos; // 목적지
        agent.updateRotation = agentUpdateRotation; // 회전 유무
        agent.acceleration = agentacceleration; // 가속도
        agent.angularSpeed = agentAngularSpeed; // 회전 속도
    }

    public void DeadComponents(GameObject gameObject)
    {
        Component[] components = gameObject.GetComponents<Component>();
        
        foreach (var component in components)
        {
            if (component is MonoBehaviour)
            {
                (component as MonoBehaviour).enabled = false;
            }
            else if (component is Collider)
            {
                (component as Collider).isTrigger = true;
            }
            else if (component is NavMeshAgent)
            {
                (component as NavMeshAgent).enabled = false;
            }
        }
    }

    // 삭제 예정
    public void DeathLootingComponents(GameObject gameObject)
    {
        Component[] components = gameObject.GetComponents<Component>();

        foreach (var component in components)
        {
            if (component is ChestInventory)
            {
                (component as ChestInventory).enabled = true;
            }
            else if (component is Looting)
            {
                (component as Looting).enabled = false;
            }
        }
    }
    
    #endregion
    

    #region Animation
    
    public bool IsAnimationRunning(string animationName, ref float attackTime)
    {
        if (animator != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                attackTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                
                return attackTime >= 0f && attackTime < 0.9f;
            }
        }
        
        return false;
    }

    private void IsAnimationWalkCheck()
    {
        if (!animator.GetBool(WALK_ANIM_BOOL_NAME) || animator.GetBool(RUN_ANIM_BOOL_NAME))
        {
            animator.SetBool(ATTACK_ANIM_BOOL_NAME, false);
            animator.SetBool(RUN_ANIM_BOOL_NAME, false);
            animator.SetBool(WALK_ANIM_BOOL_NAME, true);
        }
    }
    
    public void IsAnimationIdleCheck()
    {
        if (animator.GetBool(WALK_ANIM_BOOL_NAME) || animator.GetBool(RUN_ANIM_BOOL_NAME))
        {
            animator.SetBool(RUN_ANIM_BOOL_NAME, false);
            animator.SetBool(WALK_ANIM_BOOL_NAME, false);
        }
        
    }
    
    public void IsAnimationRunCheck()
    {
        if (animator.GetBool(ATTACK_ANIM_BOOL_NAME))
        {
            animator.SetBool(ATTACK_ANIM_BOOL_NAME, false);
        }
        
        if (animator.GetBool(WALK_ANIM_BOOL_NAME))
        {
            animator.SetBool(WALK_ANIM_BOOL_NAME, false);
        }
        
        if (!animator.GetBool(RUN_ANIM_BOOL_NAME))
        {
            animator.SetBool(RUN_ANIM_BOOL_NAME, true);
        }
    }

    public void IsAnimationAttackCheck()
    {
        if (!animator.GetBool(ATTACK_ANIM_BOOL_NAME))
        {
            animator.SetBool(ATTACK_ANIM_BOOL_NAME, true);
        }
    }
    
    #endregion


    #region NavMeshSetting

    public void NavMeshAgentPatrolSetting()
    {
        IsAnimationWalkCheck();
        
        animator.applyRootMotion = true;
        agent.speed = agentSpeed;
        agent.SetDestination(correctPos);
        patrolRandomPosCheck = false;
        
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    public void NavMeshAgentHitSetting()
    {
        animator.applyRootMotion = true;
        agent.speed = 0f;
        patrolRandomPosCheck = false;
        
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }
    
    public void NavMeshAgentAttackSetting()
    {
        //IsAnimationAttackCheck();
        
        animator.applyRootMotion = false;
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.velocity = Vector3.zero;
    }
    
    public void NavMeshAgentTrackingSetting()
    {
        IsAnimationRunCheck();

        animator.applyRootMotion = false;
        agent.speed = agentTrackingSpeed;
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    #endregion
    
    // 피격시 이벤트 애니메이션
    public void HitAnimation()
    {
        animator.SetTrigger("IsHit");
        hitCheck = false;
    }

    public void IsHit(int index, GameObject gameObject)
    {
        if (gameObject == this.gameObject)
        {
            if (index <= 3)
            {
                animator.SetBool(ATTACK_ANIM_BOOL_NAME, false);
                hitCheck = true;
            }
            
        }
    }

    // 피격시 플레이어 바라보고 정보 할당.
    public void PlayerAllocate(GameObject player, GameObject enemys)
    {
        if (enemys.gameObject != this.gameObject)
            return;
        
        if (detectedPlayer == null)
        {
            var directionToPlayer = (player.transform.position - enemy.transform.position ).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            enemy.gameObject.transform.rotation = lookRotation;
            
            detectedPlayer = player.transform;
        }
    }
    
    // soundDistance 안에 있는지 확인.
    public void SoundDistanceInPlayer(float weaponType)
    {
        
        // 이벤트 발생.
        attackSoundCheck = true;
        
        // Weapon Type 에 따른 SoundDistance 거리 설정.
        switch (weaponType)
        {
            case 0:
            case 1:
                attackSoundDistance = 3f;
                break;
            case 2:
                attackSoundDistance = 10f;
                break;
            case 3:
                attackSoundDistance = 15f;
                break;
        }
        
        // SoundDistance 안에 있는지 확인.

        
        // 있다면 detectedPlayer 할당.
    }
    
    // MoveDistance 안에 있는지 확인.
    public void MoveSoundInPlayer()
    {
        if (!moveSoundCheck)
            moveSoundCheck = true;
    }

    public void MoveSoundOutPlayer()
    {
        if (moveSoundCheck)
            moveSoundCheck = false;
    }
    
}
