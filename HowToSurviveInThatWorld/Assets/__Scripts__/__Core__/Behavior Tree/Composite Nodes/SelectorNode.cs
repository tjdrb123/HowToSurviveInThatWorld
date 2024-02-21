using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Composite
{
    private int index;
    
    protected override void OnStart()
    {
        index = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        for (int i = index; i < children.Count; ++i) {
            index = i;
            var child = children[index];

            switch (child.Update()) {
                case E_NodeState.Running:
                    return E_NodeState.Running;
                case E_NodeState.Success:
                    return E_NodeState.Success;
                case E_NodeState.Failure:
                    continue;
            }
        }

        return E_NodeState.Failure;
    }
}
