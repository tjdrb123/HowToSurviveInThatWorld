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
        if (enemyFOV == null || enemyFOV._basicZombieData == null)
            return;
        
        Vector3 enemyForward = enemyFOV.transform.forward;
        
        Handles.color = Color.black;
        Handles.DrawWireArc // Radius = _detectDistance 인 원을 그린다.
            (enemyFOV.transform.position, Vector3.up, Vector3.forward, 360, enemyFOV._basicZombieData.detectDistance);
        Handles.DrawWireArc // Radius = attackDistance 인 원을 그린다.
            (enemyFOV.transform.position, Vector3.up, Vector3.forward, 360, enemyFOV._basicZombieData.attackDistance);
        Handles.color = Color.green;
        Handles.DrawWireArc // Radius = attackSoundDistance 인 원을 그린다.
            (enemyFOV.transform.position, Vector3.up, Vector3.forward, 360, enemyFOV._basicZombieData.attackSoundDistance);
        Handles.DrawWireArc // Radius = moveSoundDistance 인 원을 그린다.
            (enemyFOV.transform.position, Vector3.up, Vector3.forward, 360, enemyFOV._basicZombieData.moveSoundDistance);
        

        float vie = Mathf.Acos(enemyFOV._basicZombieData.enemyDot) * Mathf.Rad2Deg;
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
}
