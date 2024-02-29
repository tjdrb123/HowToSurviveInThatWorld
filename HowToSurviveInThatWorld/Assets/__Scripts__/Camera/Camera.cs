using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Camera : MonoBehaviour
{
    public Transform player; //카메라가 찾을 플레이어
    public float transparency = 0.01f; // 장애물 투명도
    public float fadeTime = 0.8f; // 투명해지는데 걸리는 시간
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
        foreach (var item in renderers)
        {
            Material[] materials = item.materials; // 오브젝트의 모든 Material을 가져옴
            foreach (Material material in materials) // 각 Renderer에 대하여
            {
                Color color = material.color;
                color.a = isFadingIn ? transparency : 1f; // 투명도 조절
                material.color = color;
            }
        }
    }
    IEnumerator FadeObstacle(GameObject obstacle, bool isFadingIn)
    {
        Renderer renderer = obstacle.GetComponentInChildren<Renderer>();
        ChangeRenderer(renderer.material, false); // 먼저 Material의 Rendering Mode를 변경
        Material[] materials = renderer.materials; // 오브젝트의 모든 Material을 가져옴

        foreach (Material material in materials) // 각 Material에 대하여
        {
            Color originalColor = material.color; // 원래 색상
            Color targetColor = originalColor; // 목표 색상
            targetColor.a = isFadingIn ? transparency : 1f; // 투명하게 만들 경우 목표 알파값은 transparency, 원래 상태로 돌리는 경우 목표 알파값은 1

            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fadeTime;
                material.color = Color.Lerp(originalColor, targetColor, t);
                if (!isFadingIn && t >= 1.0f) // 알파값이 1로 돌아갔을 때 'Cutout' 모드로 변경
                {
                    ChangeRenderer(renderer.material, true);
                }
                yield return null;
            }
        }
    }
    private void ChangeRenderer(Material material,bool isChange) //Renderer Mode를 변경해줍니다
    {
        if (isChange)
        {
            material.SetFloat("_Mode", 1.0f);
            material.SetOverrideTag("RenderType", "Opaque");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.EnableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 2450;
            Color originalColor = material.color; //원래 색상
            originalColor.a = 2f; // 목표 색상
            material.color = originalColor; // 투명하게 만들 경우 목표 알파값은 transparency, 원래 상태로 돌리는 경우 목표 알파값은 1
        }
        else 
        {
            material.SetFloat("_Mode", 2.0f);
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }
}