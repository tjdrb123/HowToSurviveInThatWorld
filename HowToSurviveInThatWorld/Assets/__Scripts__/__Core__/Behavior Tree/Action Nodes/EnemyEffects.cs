using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEffects : MonoBehaviour
{
    public ParticleSystem hitPrefab;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        
    }
    
    // 좀비 피격 파티클 소환
    public void HitParticle(int index, GameObject gameObject)
    {
        if (gameObject == this.gameObject)
        {
            var position = transform.position;
            Vector3 hitPoint = _collider.ClosestPoint(position) + (Vector3.up * 0.8f);
            Vector3 hitDirection = (hitPoint - position).normalized;

            ParticleSystem hitParticle = Instantiate(hitPrefab, hitPoint, Quaternion.LookRotation(hitDirection));
            hitParticle.Play();
            Destroy(hitParticle.gameObject, 0.8f);
        }
    }

    private void OnDisable()
    {
        
    }
}
