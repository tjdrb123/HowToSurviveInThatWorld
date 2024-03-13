using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanelEffect : MonoBehaviour
{
    [SerializeField] private Image _backGround;

    private readonly float _fadeTime = 5f;


    private void OnEnable()
    {
        StartCoroutine(FadeOut(_backGround));
    }

    private IEnumerator FadeOut(Image image)
    {
        Color tempColor = image.color;
        tempColor.a = 0f;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeTime;
            image.color = tempColor;

            if (tempColor.a >= 1f)
            {
                tempColor.a = 1f;
            }
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

