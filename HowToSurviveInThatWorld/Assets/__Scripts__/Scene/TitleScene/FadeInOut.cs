using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _touchScreenText;
    
    private readonly float _fadeTime = 2f;

    private void Awake()
    {
        _touchScreenText = transform.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color tempColor = _touchScreenText.color;
        tempColor.a = 0f;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeTime;
            _touchScreenText.color = tempColor;

            if (tempColor.a >= 1f)
            {
                tempColor.a = 1f;
            }
            yield return null;
        }

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        Color tempColor = _touchScreenText.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / _fadeTime;
            _touchScreenText.color = tempColor;

            if (tempColor.a <= 0f)
            {
                tempColor.a = 0f;
            }

            yield return null;
        }

        StartCoroutine(FadeOut());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
// 60 ~ 255, 2초 간격으로 Fade In Out