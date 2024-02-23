using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoAttack : LeafAction
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
            zombieData.IsAnimationAttackCheck();
            return E_NodeState.Success;
        }

        return E_NodeState.Failure;
    }
}
