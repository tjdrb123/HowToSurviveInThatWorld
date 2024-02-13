using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : LeafAction
{
    public float duration = 1;
    private float _startTime;
    
    protected override void OnStart()
    {
        _startTime = Time.time;
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (Time.time - _startTime > duration)
        {
            return E_NodeState.Success;
        }

        return E_NodeState.Running;
    }
}
