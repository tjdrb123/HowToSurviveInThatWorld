using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : Decorator
{
    private const string _DEATH_ANIM_TRIGGER_NAME = "IsDeath";
    private static readonly int IsDeath = Animator.StringToHash(_DEATH_ANIM_TRIGGER_NAME);

    private readonly bool _hitCheck = true;
    
    protected override void OnStart()
    {
        if (_hitCheck)
        {
            Manager_UnitEvent.Instance.OnDamaged += zombieData.IsHit;
            Manager_UnitEvent.Instance.OnDamaged += zombieData.effects.HitParticle;
            Manager_UnitEvent.Instance.OnHitProgress += zombieData.PlayerAllocate;
            Manager_UnitEvent.Instance.OnAttackSound += zombieData.SoundDistanceInPlayer;
            Manager_UnitEvent.Instance.OnMoveSound += zombieData.MoveSoundInPlayer;
            Manager_UnitEvent.Instance.OutMoveSound += zombieData.MoveSoundOutPlayer;

        }
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.enemy.Health <= 0)
        {
            Death();
            Manager_UnitEvent.Instance.OnDamaged -= zombieData.IsHit;
            Manager_UnitEvent.Instance.OnDamaged -= zombieData.effects.HitParticle;
            Manager_UnitEvent.Instance.OnHitProgress -= zombieData.PlayerAllocate;
            Manager_UnitEvent.Instance.OnAttackSound -= zombieData.SoundDistanceInPlayer;
            Manager_UnitEvent.Instance.OnMoveSound -= zombieData.MoveSoundInPlayer;
            Manager_UnitEvent.Instance.OutMoveSound -= zombieData.MoveSoundOutPlayer;
        }
        else 
            child.Update();

        return E_NodeState.Running;
    }

    private E_NodeState Death()
    {
        zombieData.animator.ResetTrigger("IsHit");
        zombieData.animator.SetTrigger(IsDeath);
        zombieData.DeadComponents(zombieData.gameObject); // Components OnDisable
        //zombieData.DeathLootingComponents(zombieData.gameObject); // Looting Component OnEnable
        zombieData.gameObject.layer = 8;
        return E_NodeState.Failure;
    }
}
