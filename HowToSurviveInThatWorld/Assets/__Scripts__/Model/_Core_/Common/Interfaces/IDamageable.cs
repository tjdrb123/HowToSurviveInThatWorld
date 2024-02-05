
using System;

public interface IDamageable
{
    #region Properties

    float Health { get; }
    float MaxHealth { get; }
    bool IsInitialized { get; }

    #endregion



    #region Events

    event Action OnHealthChanged;
    event Action OnMaxHealthChanged;

    event Action OnInitialize;
    event Action OnWillUninitialize;
    event Action OnDefeated;
    event Action<float> OnHealed;
    event Action<float, bool> OnDamaged;

    #endregion



    #region Method

    void TakeDamage(IDamage rawDamage);

    #endregion
}