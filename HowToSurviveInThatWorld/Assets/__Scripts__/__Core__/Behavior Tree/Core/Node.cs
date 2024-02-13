using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum E_NodeState
    {
        Running,
        Success,
        Failure
    }

    public E_NodeState state = E_NodeState.Running;
    public bool started = false;
    public string guid;
    public Vector2 position;

    public E_NodeState Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state != E_NodeState.Running)
        {
            OnStop();
            started = false;
        }
        
        return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract E_NodeState OnUpdate();
}
