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
            ZombieEventSubscribe();
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
            ZombieEventUnlock();
        }
        else 
            child.Update();

        return E_NodeState.Running;
    }

    private E_NodeState Death()
    {
        // Animation Setting
        zombieData.animator.ResetTrigger("IsHit");
        zombieData.animator.SetTrigger(IsDeath);
        
        // Components OnDisable 
        zombieData.DeadComponents(zombieData.gameObject);
        //zombieData.DeathLootingComponents(zombieData.gameObject); // Looting Component OnEnable
        
        // Change Layer
        zombieData.gameObject.layer = 8;
        
        // Kill Count Increase
        Manager_WarScene.Instance.killCount++;
        
        // Enemy Current Index Setting
        EnemySpawner.currentZombies--; // 좀비 최대 스폰을 확인하기 위한 Death Count
        
        // Death Sound
        Manager_Sound.instance.AudioStop(zombieData.gameObject);
        Manager_Sound.instance.AudioPlay(zombieData.gameObject, "Sounds/SFX/Zombie/ZombieDead", false, false);
        
        return E_NodeState.Failure;
    }

    #region Zombie Event 
    
    private void ZombieEventSubscribe()
    {
        Manager_UnitEvent.Instance.OnDamaged += zombieData.IsHit;
        Manager_UnitEvent.Instance.OnDamaged += zombieData.effects.HitParticle;
        Manager_UnitEvent.Instance.OnHitProgress += zombieData.PlayerAllocate;
        Manager_UnitEvent.Instance.OnAttackSound += zombieData.SoundDistanceInPlayer;
        Manager_UnitEvent.Instance.OnMoveSound += zombieData.MoveSoundInPlayer;
        Manager_UnitEvent.Instance.OutMoveSound += zombieData.MoveSoundOutPlayer;
    }

    private void ZombieEventUnlock()
    {
        Manager_UnitEvent.Instance.OnDamaged -= zombieData.IsHit;
        Manager_UnitEvent.Instance.OnDamaged -= zombieData.effects.HitParticle;
        Manager_UnitEvent.Instance.OnHitProgress -= zombieData.PlayerAllocate;
        Manager_UnitEvent.Instance.OnAttackSound -= zombieData.SoundDistanceInPlayer;
        Manager_UnitEvent.Instance.OnMoveSound -= zombieData.MoveSoundInPlayer;
        Manager_UnitEvent.Instance.OutMoveSound -= zombieData.MoveSoundOutPlayer;
    }
    
    #endregion
}
