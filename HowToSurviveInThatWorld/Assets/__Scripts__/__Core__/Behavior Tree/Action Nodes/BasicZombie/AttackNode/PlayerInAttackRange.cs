using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAttackRange : LeafAction
{
    private const float _rotationSpeed = 4f;
    private Vector3 _directionToPlayer;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.detectedPlayer == null)
            return E_NodeState.Failure;
            
        _directionToPlayer =
            (zombieData.detectedPlayer.position - zombieData.transform.position).normalized;
        
        if (TargetByCheck())
        {
            if (!TargetForwardCheck())
            {
                TargetDirectionCompensate();
            }

            zombieData.NavMeshAgentAttackSetting();
            return E_NodeState.Success;
            
        }

        return E_NodeState.Failure;
    }

    // 타겟이 공격 거리 안에 있는지 체크
    private bool TargetByCheck()
    {
        return Vector3.SqrMagnitude(zombieData.detectedPlayer.position - zombieData.transform.position) <= 
               (zombieData.attackDistance * zombieData.attackDistance) - 1.2f;
    }

    // 타겟이 내적 안에 있는지 확인
    private bool TargetForwardCheck()
    {
        return Vector3.Dot(zombieData.transform.forward, _directionToPlayer) >= zombieData.enemyDot;
    }

    //정면 회전 처리
    private void TargetDirectionCompensate()
    {
            // 타겟 방향으로의 회전을 계산.
            Quaternion lookRotation = Quaternion.LookRotation(_directionToPlayer);
            // 현재 방향에서 타겟 방향으로 점진적으로 회전.
            zombieData.transform.rotation = Quaternion.Slerp(zombieData.transform.rotation, lookRotation,
                Time.deltaTime * _rotationSpeed);
    }
    
}

