using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTracking : LeafAction
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {

    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.detectedPlayer != null)
        {
            zombieData.NavMeshAgentTrackingSetting();
            zombieData.agent.SetDestination(zombieData.detectedPlayer.position);
        }

        return E_NodeState.Failure;
    }
}
