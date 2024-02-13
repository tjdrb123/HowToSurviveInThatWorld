using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : Decorator
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        child.Update();

        return E_NodeState.Running;
    }
}
