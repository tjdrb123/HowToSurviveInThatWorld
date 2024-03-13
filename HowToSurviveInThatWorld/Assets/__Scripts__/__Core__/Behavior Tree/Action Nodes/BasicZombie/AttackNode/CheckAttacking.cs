using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttacking : LeafAction
{
    private const string ATTACK_ANIM_STATE_NAME = "Attack";
    
    private float _attackTime;
    private bool _isAttack;
    
    protected override void OnStart()
    {
        _attackTime = 0;
        _isAttack = true;
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.IsAnimationRunning(ATTACK_ANIM_STATE_NAME, ref _attackTime))
        {
            AttackCheck();
            zombieData.agent.speed = 0;
            return E_NodeState.Running;
        }
        
        return E_NodeState.Success;
    }

    private void AttackCheck()
    {
        if (zombieData.detectedPlayer != null && _attackTime > 0.35f && _isAttack)
        {
            if (Vector3.SqrMagnitude(zombieData.detectedPlayer.position - zombieData.transform.position) <=
                (zombieData.attackDistance * zombieData.attackDistance) + 0.5f)
            {
                Vector3 directionToPlayer =
                    (zombieData.detectedPlayer.position - zombieData.transform.position).normalized;
                if (Vector3.Dot(zombieData.transform.forward, directionToPlayer) >= zombieData.enemyDot)
                {
                    Manager_Sound.instance.AudioPlay(zombieData.detectedPlayer.gameObject, "Sounds/SFX/Zombie/MaleZombieHitSound", false, false);
                    zombieData.enemy.ApplyDamage(this, zombieData.detectedPlayer.gameObject);
                    Manager_WarScene.Instance.HealthProgressBar(); // 체력 UI 갱신

                    _isAttack = false;
                }
            }

        }
    }
}
