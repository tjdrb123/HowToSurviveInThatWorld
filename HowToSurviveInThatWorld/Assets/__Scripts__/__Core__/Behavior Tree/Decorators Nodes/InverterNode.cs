using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterNode : Decorator
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        switch (child.Update())
        {
            case E_NodeState.Running:
                return E_NodeState.Running;
            case E_NodeState.Success:
                return E_NodeState.Failure;
            case E_NodeState.Failure:
                return E_NodeState.Success;
        }

        return E_NodeState.Failure;
    }
}
