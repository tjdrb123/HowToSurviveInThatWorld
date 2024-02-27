using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEffects : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        
    }
    
    // 좀비 피격 파티클 소환
    private void HitParticle()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
