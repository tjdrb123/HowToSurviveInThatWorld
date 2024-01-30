
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class Unit : Entity
{
    #region Fields

    // RequireComponents
    protected Animator _animator;
    protected Collider _collider;

    #endregion



    #region Unity Behavior With Override

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        
    }

    #endregion
}
