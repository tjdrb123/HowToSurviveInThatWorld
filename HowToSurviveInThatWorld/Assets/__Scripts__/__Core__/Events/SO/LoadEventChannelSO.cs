
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Load Event Channel")]
public class LoadEventChannelSO : DescriptionBaseSO
{
    #region Event

    public Func<GameSceneSO, bool, bool, UniTask> OnLoadingRequest;

    #endregion



    #region Raise

    public async UniTask RaiseEventAsync(GameSceneSO sceneToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
    {
        if (OnLoadingRequest != null)
        {
            await OnLoadingRequest.Invoke(sceneToLoad, showLoadingScreen, fadeScreen);
        }
        else
        {
            // 씬 로딩이 정상적으로 될 수 없음.
            // 이벤트가 등록 되지 않았으므로 SceneManager를 확인 해봐야함.
            DebugLogger.LogWarning("Scene loading was requested. " +
                "Check SceneManager already present, " +
                "and make sure it's listening on this Load Event Channel");
        }
    }

    #endregion
}
