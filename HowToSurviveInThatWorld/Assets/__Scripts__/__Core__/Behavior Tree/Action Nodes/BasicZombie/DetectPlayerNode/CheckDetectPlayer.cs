using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// detectDistance 범위 안에 Player가 있는지 체크.
// 있다면, 시야각(FOV) 안에 있는지 체크.
// 있다면, Ray를 쏴서 Obstacle이 걸리지 않는지 체크.
public class CheckDetectPlayer : LeafAction
{
    private Collider[] _overlapColliders;
    private readonly float _DOT_PRODUCT_ = 0.9f;
    
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
        if (_overlapColliders != null & _overlapColliders.Length> 0)
        {

            Transform undefinedPlayer = _overlapColliders[0].transform;
            Vector3 directionToPlayer = (undefinedPlayer.position - zombieData.transform.position).normalized;
            
            if (Vector3.Dot(zombieData.transform.forward, directionToPlayer) > _DOT_PRODUCT_)
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
            
            return E_NodeState.Success;
        }

        InspectorViewData(); // 삭제 예정
        zombieData.detectedPlayer = null;
        
        return E_NodeState.Failure;
    }

    private void InspectorViewData()
    {
        dataContext.moveToTarget = zombieData.detectedPlayer;
    }
}
