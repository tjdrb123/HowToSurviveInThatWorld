using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttacking : LeafAction
{
    private const string ATTACK_ANIM_STATE_NAME = "Attack";
    
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.IsAnimationRunning(ATTACK_ANIM_STATE_NAME))
        {
            zombieData.agent.speed = 0;
            return E_NodeState.Running;
        }

        return E_NodeState.Success;
    }
}
