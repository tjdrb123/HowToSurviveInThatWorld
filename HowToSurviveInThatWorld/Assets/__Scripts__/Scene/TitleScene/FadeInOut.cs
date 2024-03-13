using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    private Graphic _graphic;

    private readonly float _fadeTime = 2f;

    private void Awake()
    {
        _graphic = GetComponent<Graphic>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color tempColor = _graphic.color;
        tempColor.a = 0f;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeTime;
            _graphic.color = tempColor;

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
        Color tempColor = _graphic.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / _fadeTime;
            _graphic.color = tempColor;

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