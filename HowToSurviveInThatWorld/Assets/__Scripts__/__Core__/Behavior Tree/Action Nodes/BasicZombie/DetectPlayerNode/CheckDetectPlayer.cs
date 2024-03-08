using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// detectDistance 범위 안에 Player가 있는지 체크.
// 있다면, 시야각(FOV) 안에 있는지 체크.
// 있다면, Ray를 쏴서 Obstacle이 걸리지 않는지 체크.
public class CheckDetectPlayer : LeafAction
{
    private Collider[] _overlapColliders;
    
    protected override void OnStart()
    {
        _overlapColliders =
            Physics.OverlapSphere(zombieData.transform.position, zombieData.detectDistance
                , zombieData.PLAYER_LAYER_MASK);
    }

    protected override void OnStop()
    {
        
    }

    protected override E_NodeState OnUpdate()
    {
        if (zombieData.attackSoundCheck)
        {
            // Attack Sound Distance Check
            DetectPlayerCheck();
        }
        else if (zombieData.moveSoundCheck)
        {
            // Move Sound Distance Check
            DetectPlayerCheck();
        }
        
        if (_overlapColliders != null & _overlapColliders.Length> 0)
        {
            DetectDistanceCheck();

            return E_NodeState.Success;
        }
        

        InspectorViewData(); // 삭제 예정
        zombieData.moveSoundCheck = false;
        zombieData.attackSoundCheck = false;
        zombieData.detectedPlayer = null;
        zombieData.detectDistance = 10f;

        zombieData.zombieSoundCheck = true;
        
        return E_NodeState.Failure;
    }

    private void InspectorViewData()
    {
        dataContext.moveToTarget = zombieData.detectedPlayer;
    }

    private E_NodeState DetectPlayerCheck()
    {
        var overlapColliders = Physics.OverlapSphere(zombieData.transform.position, zombieData.moveSoundDistance
            , zombieData.PLAYER_LAYER_MASK);

        if (overlapColliders != null & overlapColliders.Length > 0)
        {
            zombieData.detectedPlayer = overlapColliders[0].transform;
            RandomSound(); // 리팩토링 예정
            return E_NodeState.Success;
        }

        return E_NodeState.Running;
    }

    private void DetectDistanceCheck()
    {
        Transform undefinedPlayer = _overlapColliders[0].transform;
        Vector3 directionToPlayer = (undefinedPlayer.position - zombieData.transform.position).normalized;
            
        if (Vector3.Dot(zombieData.transform.forward, directionToPlayer) > zombieData.enemyDot)
        {
            float distanceToTarget = Vector3.Distance(undefinedPlayer.position ,zombieData.transform.position);

            if (!Physics.Raycast(zombieData.transform.position, directionToPlayer, distanceToTarget, zombieData.OBSTACLE_LAYER_MASK))
            {
                // 순찰 노드 최초확인 bool값 초기화.
                zombieData.patrolRandomPosCheck = true;
                zombieData.idleWaitCheck = true;
                    
                zombieData.detectedPlayer = undefinedPlayer;
                
                RandomSound(); // 리팩토링 예정
            }
        }

        InspectorViewData(); // 삭제 예정
    }

    // 리팩토링 예정
    private void RandomSound()
    {
        if (zombieData.zombieSoundCheck)
        {
            int randomIndex = Random.Range(0, 5);

            switch (randomIndex)
            {
                case 0:
                    Manager_Sound.instance.AudioPlay(zombieData.gameObject, "Sounds/SFX/Zombie/Zombie0", false, false);
                    break;
                case 1:
                    Manager_Sound.instance.AudioPlay(zombieData.gameObject, "Sounds/SFX/Zombie/Zombie1", false, false);
                    break;
                case 2:
                    Manager_Sound.instance.AudioPlay(zombieData.gameObject, "Sounds/SFX/Zombie/Zombie2", false, false);
                    break;
                case 3:
                    Manager_Sound.instance.AudioPlay(zombieData.gameObject, "Sounds/SFX/Zombie/Zombie3", false, false);
                    break;
                case 4:
                    Manager_Sound.instance.AudioPlay(zombieData.gameObject, "Sounds/SFX/Zombie/Zombie4", false, false);
                    break;
            }
            
            zombieData.zombieSoundCheck = false;
        }
    }
}
