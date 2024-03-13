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

    public static int currentZombies;
    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }
    private void Start()
    {
        InvokeRepeating("SpawnZombie", 0f, spawnInterval);
        currentZombies = 0;
    }

    private void SpawnZombie()
    {
        if (currentZombies >= maxZombies)
            return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        int index = GetZombieIndexBasedOnProbability();

        Instantiate(zombiePrefab[index], spawnPoint.position, spawnPoint.rotation);

        currentZombies++;
    }

    private int GetZombieIndexBasedOnProbability()
    {
        float randomValue = Random.value; // 0.0 ~ 1.0 사이의 랜덤한 값

        if (randomValue < 0.3f) // 30%의 확률로 가장 약한 좀비(0번째 인덱스 좀비)를 리턴합니다.
            return 0;
        else if (randomValue < 0.525f) // 22.5%의 확률로 1번째 인덱스 좀비를 리턴합니다.
            return 1;
        else if (randomValue < 0.75f) // 22.5%의 확률로 2번째 인덱스 좀비를 리턴합니다.
            return 2;
        else if (randomValue < 0.90f) // 15%의 확률로 그 다음 좀비를 리턴합니다.
            return 3;
        else if (randomValue < 0.97f) // 7%의 확률로 그 다음 좀비를 리턴합니다.
            return 4;
        else // 3%의 확률로 가장 강한 좀비를 리턴합니다. (100% - 96% = 4%)
            return 5;
    }
}
