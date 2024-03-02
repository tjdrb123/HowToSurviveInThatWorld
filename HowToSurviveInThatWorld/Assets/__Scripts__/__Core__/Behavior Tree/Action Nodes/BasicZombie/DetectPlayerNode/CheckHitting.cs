using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHitting : LeafAction
{
    private float _hitTime;
    protected override void OnStart()
    {
        if (zombieData.hitCheck)
        {
            zombieData.HitAnimation();
            zombieData.detectDistance = 20f; // 피격시 감지범위 상승
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        // 피격 진행중 다시 맞으면 다시 재진행
        if (zombieData.hitCheck)
            return E_NodeState.Failure;
            
        if (zombieData.IsAnimationRunning("Hit", ref _hitTime))
        {
            zombieData.NavMeshAgentHitSetting();
            return E_NodeState.Running;
        }
        
        return E_NodeState.Failure;
    }
    
    // 맞게되면 표적 플레이어로 할당.
    private void HitPlayerAllocate()
    {
        
    }
    
    // 플레이어를 바라보게 회전.
}
