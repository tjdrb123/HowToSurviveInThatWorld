using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Camera : MonoBehaviour
{
    public Transform player; //카메라가 찾을 플레이어
    public float transparency = 0.3f; // 장애물 투명도
    public float fadeTime = 0.5f; // 투명해지는데 걸리는 시간
    private Dictionary<GameObject, Coroutine> fadingObstacles = new Dictionary<GameObject, Coroutine>();// 현재 페이드 중인 장애물과 그에 해당하는 코루틴을 저장하는 딕셔너리
                                                                                                        
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void LateUpdate()
    {
        RaycastHit[] hits;
        Vector3 direction = player.position - transform.position;
        hits = Physics.RaycastAll(transform.position + new Vector3(0, 1, 0), direction, direction.magnitude);

        HashSet<GameObject> hitObstacles = new HashSet<GameObject>();
        foreach (RaycastHit hit in hits)
        {
            GameObject obstacle = hit.collider.gameObject;
            if (obstacle != player.gameObject) 
            {
                hitObstacles.Add(obstacle);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall")) //장애물만 인식 그리고 플레이어 X
                {
                    if (!fadingObstacles.ContainsKey(obstacle)) // hits에 저장된 오브젝트가 Dictonary에 저장되어 있지 않으면 작동
                    {
                        fadingObstacles[obstacle] = StartCoroutine(FadeObstacle(obstacle, true)); //벗어난 오브젝트 Corutine시작
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Roof") && obstacle != player.gameObject)
                {
                    SetTransparency(hit.collider.gameObject, true);
                    fadingObstacles[obstacle] = null; // "Water" 레이어에 속하는 오브젝트를 딕셔너리에 추가
                }
            }
        }
        // 이전에 투명하게 했던 장애물 중 이번에 레이캐스트에 걸리지 않은 장애물을 찾음
        List<GameObject> obstaclesToUnfade = new List<GameObject>();
        foreach (GameObject fadingObstacle in fadingObstacles.Keys)
        {
            if (!hitObstacles.Contains(fadingObstacle))
            {
                obstaclesToUnfade.Add(fadingObstacle);
            }
        }

        // 투명하게 만든 장애물 중 레이캐스트에 걸리지 않는 장애물을 원래 상태로 돌
        foreach (GameObject obstacleToUnfade in obstaclesToUnfade)
        {
            if (obstacleToUnfade.layer == LayerMask.NameToLayer("Roof"))
            {
                SetTransparency(obstacleToUnfade, false);
            }
            else if (obstacleToUnfade.layer == LayerMask.NameToLayer("Wall"))
            {
                StopCoroutine(fadingObstacles[obstacleToUnfade]); // 투명하게 만드는 코루틴 중지
                fadingObstacles.Remove(obstacleToUnfade); // 딕셔너리에서 제거
                StartCoroutine(FadeObstacle(obstacleToUnfade, false)); // 원래 상태로 돌리는 코루틴 시작    
            }
        }
    }
    private void SetTransparency(GameObject obj, bool isFadingIn)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(); // 천장 오브젝트의 모든 Renderer 컴포넌트를 가져옴
        foreach (Renderer renderer in renderers) // 각 Renderer에 대하여
        {
            Material material = renderer.material;
            Color color = material.color;
            color.a = isFadingIn ? transparency : 1f; // 투명도 조절
            material.color = color;
        }
    }
    IEnumerator FadeObstacle(GameObject obstacle, bool isFadingIn)
    {
        Color originalColor = obstacle.GetComponentInChildren<Renderer>().material.color; //원래 색상
        Color targetColor = originalColor; // 목표 색상
        targetColor.a = isFadingIn ? transparency : 1f; // 투명하게 만들 경우 목표 알파값은 transparency, 원래 상태로 돌리는 경우 목표 알파값은 1

        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeTime;
            obstacle.GetComponentInChildren<Renderer>().material.color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
        }
    }
}