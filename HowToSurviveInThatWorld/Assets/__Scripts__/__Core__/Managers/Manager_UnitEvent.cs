using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Manager_UnitEvent : Singleton<Manager_UnitEvent>
{
    // 좀비가 피격을 입을 때 사용할 이벤트 (피격 애니메이션 및 파티클)
    public event Action<int> OnDamaged;

    public int index = 0;

    public void OnDamagedEnemy()
    {
        index = Random.Range(0, 10);
        OnDamaged?.Invoke(index);
    }
}
