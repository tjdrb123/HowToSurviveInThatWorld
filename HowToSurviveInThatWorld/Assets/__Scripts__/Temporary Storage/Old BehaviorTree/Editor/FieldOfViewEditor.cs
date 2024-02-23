using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(BehaviorTreeRunner))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        #if DEBUG_MODE
        BehaviorTreeRunner enemyFOV = (BehaviorTreeRunner)target;
        Vector3 enemyForward = enemyFOV.transform.forward;
        
        Handles.color = Color.black;
        Handles.DrawWireArc // Radius = _detectDistance 인 원을 그린다.
            (enemyFOV.transform.position, Vector3.up, Vector3.forward, 360, enemyFOV._basicZombieData.detectDistance);

        float vie = Mathf.Acos(0.9f) * Mathf.Rad2Deg;
        Vector3 leftViewDirection = Quaternion.Euler(0f, -vie, 0) * enemyForward;
        Vector3 rightViewDirection = Quaternion.Euler(0f, vie, 0f) * enemyForward;
        Handles.DrawLine    // 좀비 정면으로부터 내적 0.9에 해당하는 선
            (enemyFOV.transform.position, enemyFOV.transform.position + leftViewDirection * enemyFOV._basicZombieData.detectDistance);
        Handles.DrawLine
            (enemyFOV.transform.position, enemyFOV.transform.position + rightViewDirection * enemyFOV._basicZombieData.detectDistance);
        
        if (enemyFOV._basicZombieData.detectedPlayer != null)
        {
            Handles.color = Color.red;
            Handles.DrawLine(enemyFOV.transform.position, enemyFOV._basicZombieData.detectedPlayer.position);
        }
        #endif
    }
    
    // 좀비가 공격중에 detectDistance 원 밖으로 나간뒤에, 다시 만나면 그 때 자리로 순간인동
    // 
}
