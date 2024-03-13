using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class Enemy : Unit, ICombative
{
    public void ApplyDamage(Object source, GameObject target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>(); // 플레이어의 Unit을 가져옴

        HealthStatModifier rawDamage = new HealthStatModifier
        {

            Instigator = gameObject,

            Type = E_StatModifier_OperatorType.Additive,

            Magnitude = (-1) * _statController.GetStat(E_StatType.Damage).Value,

            Source = source,

            IsCriticalHit = false,
        };
        
        // 추가적 연산작업 (크리티컬, 랜덤 데미지 추가 등)
        damageable.TakeDamage(rawDamage);
    }
}
