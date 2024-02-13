using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Composite
{
    private int _current;
    
    protected override void OnStart()
    {
        _current = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        for (int i = _current; i < children.Count; ++i) 
        {
            _current = i;
            var child = children[_current];

            switch (child.Update()) 
            {
                case E_NodeState.Running:
                    return E_NodeState.Running;
                case E_NodeState.Success:
                    continue;
                case E_NodeState.Failure:
                    return E_NodeState.Failure;
            }
        }

        return E_NodeState.Success;
        
        /*
            if (children == null || children.Count == 0)
                Debug.LogError("Missing Child");

            foreach (var child in children)
            {
                switch (child.Update())
                {
                    case E_NodeState.Running:
                        return E_NodeState.Running;
                    case E_NodeState.Success:
                        continue;
                    case E_NodeState.Failure:
                        return E_NodeState.Failure;
                }
            }

            return E_NodeState.Success;
            }
        */
    }
}
