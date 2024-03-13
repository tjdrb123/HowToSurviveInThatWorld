using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoading : MonoBehaviour
{
    [SerializeField]
    private GameObject _touchScreen;
    
    private Image _progressBar;

    private void Awake()
    {
        _progressBar = transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(FakeLoadingBar());
    }

    private IEnumerator FakeLoadingBar()
    {
        float timer = 0f;
        yield return new WaitForSeconds(2f);

        while (timer < 1.0f)
        {
            yield return null;
            timer += Time.deltaTime;
            if (_progressBar.fillAmount < 0.9f)
            {
                _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, 0.9f, timer);
                if (_progressBar.fillAmount >= 0.9f)
                {
                    timer = 0f;
                }
            }
            else
            {
                _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, 1f, timer);
            }
        }
        
        _touchScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
