using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TestDebugLogNode : LeafAction
{
    public string Test;
    
    protected override void OnStart()
    {
        Debug.Log($"OnStart{Test}");
    }

    protected override void OnStop()
    {
        Debug.Log($"OnStop{Test}");
    }

    protected override E_NodeState OnUpdate()
    {
        Debug.Log($"OnUpdate{Test}");
        return E_NodeState.Success;
    }
}
