using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAttackRange : LeafAction
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
            if (Vector3.SqrMagnitude(zombieData.detectedPlayer.position - zombieData.transform.position) <
                (zombieData.attackDistance * zombieData.attackDistance))
            {
                zombieData.NavMeshAgentAttackSetting();
                return E_NodeState.Success;
            }
        }
        
        return E_NodeState.Failure;
    }
}
