using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Node : ScriptableObject
{
    [HideInInspector] public E_NodeState state = E_NodeState.Running;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public DataContext dataContext;
    [HideInInspector] public BasicZombieData zombieData;
    [TextArea] public string description;

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

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract E_NodeState OnUpdate();
}
