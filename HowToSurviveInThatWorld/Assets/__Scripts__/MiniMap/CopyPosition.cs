using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private bool x, y, z; // 이 값이 true  이면 target 좌표, fasle이면 현재 좌표를 그대로 사용
    [SerializeField] private Transform target; // 쫓아가야할 대상 Transform

    private void Update()
    {
        if (!target) return;

        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z)
        );
    }
}
