using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Manager_UnitEvent : Singleton<Manager_UnitEvent>
{
    // 좀비가 피격을 입을 때 사용할 이벤트 (피격 애니메이션 및 파티클)
    public event Action<int, GameObject> OnDamaged;
    public event Action<GameObject, GameObject> OnHitProgress;
    public event Action<float> OnAttackSound;
    public event Action OnMoveSound;
    public event Action OutMoveSound;

    public int index = 0;

    // 피격 이벤트 (피격 파티클, 애니메이션)
    public void OnDamagedEnemy(GameObject enemy)
    {
        index = Random.Range(0, 10);
        OnDamaged?.Invoke(index, enemy);
    }

    // 피격 이벤트 (Enemy 방향 처리)
    public void OnHitEnemyAllocate(GameObject player, GameObject enemy)
    {
        OnHitProgress?.Invoke(player, enemy);
    }
    
    // 공격 소리 감지 이벤트
    public void OnAttackSoundEnemy(float weaponType)
    {
        OnAttackSound?.Invoke(weaponType);
    }
    
    // 움직임 소리(달리기, 걷기) 감지 이벤트
    public void OnMoveSoundEnemy()
    {
        OnMoveSound?.Invoke();
    }

    public void OutMoveSoundEnemy()
    {
        OutMoveSound?.Invoke();
    }
    
}
