using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 랜덤 목적지 할당.
public class RandomPositionAssignment : LeafAction
{
    protected override void OnStart()
    {
        // 삭제 예정
        dataContext.patrolMinPos = zombieData.patrolMaxPos;
        dataContext.patrolMaxPos = zombieData.patrolMinPos;
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.detectedPlayer == null & zombieData.patrolRandomPosCheck == true)
        {
            zombieData.correctPos.x = Random.Range(zombieData.patrolMinPos.x, zombieData.patrolMaxPos.x);
            zombieData.correctPos.z = Random.Range(zombieData.patrolMinPos.y, zombieData.patrolMaxPos.y);

            zombieData.NavMeshAgentPatrolSetting();
            inspectorViewData(zombieData.correctPos.x, zombieData.correctPos.z);
        }
        
        return E_NodeState.Failure;
    }

    private void inspectorViewData(float x, float z)
    {
        dataContext.moveToPosition = new Vector3(x, zombieData.transform.position.y, z);
    }
}
