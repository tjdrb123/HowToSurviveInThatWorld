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
        
        Debug.Log($"DataContext:{dataContext.moveToPosition}");

        dataContext.moveToPosition.x += 1;
        return E_NodeState.Success;
    }
}
