using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest : MonoBehaviour
{
    public GameObject enemy;
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            player.ApplyDamage(this, enemy);
        }
    }
}