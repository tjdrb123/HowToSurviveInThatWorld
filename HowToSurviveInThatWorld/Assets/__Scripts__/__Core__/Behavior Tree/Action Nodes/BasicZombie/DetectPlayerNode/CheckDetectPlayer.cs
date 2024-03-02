using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// detectDistance 범위 안에 Player가 있는지 체크.
// 있다면, 시야각(FOV) 안에 있는지 체크.
// 있다면, Ray를 쏴서 Obstacle이 걸리지 않는지 체크.
public class CheckDetectPlayer : LeafAction
{
    private Collider[] _overlapColliders;
    
    protected override void OnStart()
    {
        _overlapColliders =
            Physics.OverlapSphere(zombieData.transform.position, zombieData.detectDistance
                , zombieData.PLAYER_LAYER_MASK);
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.attackSoundCheck)
        {
            // Attack Sound Distance Check
            var overlapColliders = Physics.OverlapSphere(zombieData.transform.position, zombieData.attackSoundDistance
                , zombieData.PLAYER_LAYER_MASK);

            if (overlapColliders != null & overlapColliders.Length > 0)
            {
                zombieData.detectedPlayer = overlapColliders[0].transform;
                return E_NodeState.Success;
            }
            
            // Detect Distance Check
            if (_overlapColliders != null & _overlapColliders.Length> 0)
            {
                DetectDistanceCheck();
            
                return E_NodeState.Success;
            }
        }
        else if (zombieData.moveSoundCheck)
        {
            // Move Sound Distance Check
            var overlapColliders = Physics.OverlapSphere(zombieData.transform.position, zombieData.moveSoundDistance
                , zombieData.PLAYER_LAYER_MASK);

            if (overlapColliders != null & overlapColliders.Length > 0)
            {
                zombieData.detectedPlayer = overlapColliders[0].transform;
                return E_NodeState.Success;
            }
            
            // Detect Distance Check
            if (_overlapColliders != null & _overlapColliders.Length> 0)
            {
                DetectDistanceCheck();
                
                return E_NodeState.Success;
            }
        }
        else
        {
            if (_overlapColliders != null & _overlapColliders.Length> 0)
            {
                DetectDistanceCheck();
            
                return E_NodeState.Success;
            }
        }

        InspectorViewData(); // 삭제 예정
        zombieData.moveSoundCheck = false;
        zombieData.attackSoundCheck = false;
        zombieData.detectedPlayer = null;
        zombieData.detectDistance = 10f;
        
        return E_NodeState.Failure;
    }

    private void InspectorViewData()
    {
        dataContext.moveToTarget = zombieData.detectedPlayer;
    }

    private void DetectDistanceCheck()
    {
        Transform undefinedPlayer = _overlapColliders[0].transform;
        Vector3 directionToPlayer = (undefinedPlayer.position - zombieData.transform.position).normalized;
            
        if (Vector3.Dot(zombieData.transform.forward, directionToPlayer) > zombieData.enemyDot)
        {
            float distanceToTarget = Vector3.Distance(undefinedPlayer.position ,zombieData.transform.position);

            if (!Physics.Raycast(zombieData.transform.position, directionToPlayer, distanceToTarget, zombieData.ENEMY_LAYER_MASK))
            {
                // 순찰 노드 최초확인 bool값 초기화.
                zombieData.patrolRandomPosCheck = true;
                zombieData.idleWaitCheck = true;
                    
                zombieData.detectedPlayer = undefinedPlayer;
            }
        }

        InspectorViewData(); // 삭제 예정
    }
}
