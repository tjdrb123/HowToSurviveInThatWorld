using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 대기시간이 남아있는지 체크.
public class IdleWaitTimeCheck : LeafAction
{
    protected override void OnStart()
    {
        if (zombieData.idleWaitCheck == true)
        {
            zombieData.IsAnimationIdleCheck();
            zombieData.idleDurationTime = Random.Range(3f, 5f);
            zombieData.idleStartTime = Time.time;
            zombieData.idleWaitCheck = false;
        }
    }

    protected override void OnStop()
    {
        zombieData.patrolRandomPosCheck = true;
        zombieData.idleWaitCheck = true;
    }

    protected override E_NodeState OnUpdate()
    {
        if (Time.time - zombieData.idleStartTime > zombieData.idleDurationTime)
        {
            return E_NodeState.Failure;
        }

        return E_NodeState.Running;
    }
}
