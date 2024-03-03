using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager_LoadingScene : MonoBehaviour
{
    public static int nextScene;

    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private Image _backGroundImage;
    [SerializeField] private Sprite[] _backGround;
    
    

    #region Unity Life Scyle

    private void Start()
    {
        RandomTipText();
        RandomBackGround();
        StartCoroutine(LoadScene());
    }

    #endregion



    #region RandomInfo

    private void RandomTipText()
    {
        int randomIndex = Random.Range(0, 3);
        
        switch (randomIndex)
        {
            case 0:
                _tipText.text = "TIP : 좀비는 소리를 감지합니다.";
                break;
            case 1:
                _tipText.text = "TIP : 좀비는 시야로 감지가 가능합니다.";
                break;
            case 2:
                _tipText.text = "TIP : 인벤토리 제작대에서 간단한 물품을 만들 수 있습니다.";
                break;
        }
    }

    private void RandomBackGround()
    {
        int randomIndex = Random.Range(0, 3);

        _backGroundImage.sprite = _backGround[randomIndex];
    }

    #endregion


    public static void LoadScene(int sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("04_Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float time = 0f;
        while (!op.isDone)
        {
            yield return null;
            
            time += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, op.progress, time);
                if (_progressBar.fillAmount >= op.progress)
                {
                    time = 0f;
                }
            } 
            else 
                _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, 1f, time);

            if (_progressBar.fillAmount == 1.0f)
            {
                op.allowSceneActivation = true; yield break;
            } 
        }
    }
}
