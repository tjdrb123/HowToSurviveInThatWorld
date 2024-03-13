using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적지 까지 경로가 유효한지 체크.
public class CorrectPathCheck : LeafAction
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            DebugLogger.LogError("Agent Path is Invalid");
            
            zombieData.patrolRandomPosCheck = true;
            return E_NodeState.Running;
        }
        
        return E_NodeState.Failure;
    }
}
