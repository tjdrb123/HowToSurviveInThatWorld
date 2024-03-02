using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] zombiePrefab;
    private Transform[] spawnPoints;
    [SerializeField] private int maxZombies = 30;
    [SerializeField] private float spawnInterval = 5f;

    public static int currentZombies = 0;
    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }
    private void Start()
    {
        InvokeRepeating("SpawnZombie", 0f, spawnInterval);
    }

    private void SpawnZombie()
    {
        if (currentZombies >= maxZombies)
            return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(zombiePrefab[Random.Range(0, zombiePrefab.Length)], spawnPoint.position, spawnPoint.rotation);

        currentZombies++;
    }
}
