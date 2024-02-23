using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적지에 도착했는지 체크.
public class CheckArrivalAtDestination : LeafAction
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.agent.pathPending)
        {
            return E_NodeState.Running;
        }
        
        if (zombieData.agent.remainingDistance < zombieData.agent.stoppingDistance)
        {
            return E_NodeState.Success;
        }
        
        return E_NodeState.Failure;
    }
}
