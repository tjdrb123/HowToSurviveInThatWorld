
using UnityEngine;

public interface IDamage
{
    #region Properties

    // 크리티컬이 적용 됐는지 확인
    bool IsCriticalHit { get; }
    // 실질적인 데미지량
    float Magnitude { get; }
    // 제공자 (Provider) 플레이어가 적을 공격했다면 (Player)
    GameObject Instigator { get; }
    // Magnitude에 값을 대입할 Origin Source
    Object Source { get; }

    #endregion
}