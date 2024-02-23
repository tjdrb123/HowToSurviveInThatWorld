using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : Decorator
{
    private const string _DEATH_ANIM_TRIGGER_NAME = "IsDeath";
    private static readonly int IsDeath = Animator.StringToHash(_DEATH_ANIM_TRIGGER_NAME);
    
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.enemy.Health <= 0)
            Death();
        else 
            child.Update();

        return E_NodeState.Running;
    }

    private E_NodeState Death()
    {
        zombieData.animator.SetTrigger(IsDeath);
        zombieData.DeadComponents(zombieData.gameObject);
        return E_NodeState.Failure;
    }
}
