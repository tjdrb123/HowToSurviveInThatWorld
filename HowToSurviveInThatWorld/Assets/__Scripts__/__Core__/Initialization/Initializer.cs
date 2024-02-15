
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameSceneSO _ManagerScene;
    [SerializeField] private GameSceneSO _LoadTitleScene;

    [Header("Broadcasting")]
    [SerializeField] private AssetReference _LoadTitleSceneChannel;
    
    #endregion



    #region Unity Behavior

    private void Start()
    {
        InitializeAsync();
    }

    #endregion



    #region UniTask

    private async UniTaskVoid InitializeAsync()
    {
        await LoadToPersistentScene();
        await LoadToTitleScene();
    }

    private async UniTask LoadToPersistentScene()
    {
        try
        {
            var operation = _ManagerScene.SceneReference.LoadSceneAsync(
                LoadSceneMode.Additive, true).ToUniTask();
            await operation;
        }
        catch(Exception exception)
        {
            DebugLogger.LogError($"Load Scene Failed (Persistent) : {exception.Message}");
        }
    }

    private async UniTask LoadToTitleScene()
    {
        const string INIT_SCENE_NAME = "__Initialization__";
        try
        {
            var eventChannel = await _LoadTitleSceneChannel.LoadAssetAsync<LoadEventChannelSO>().ToUniTask();
            await eventChannel.RaiseEventAsync(_LoadTitleScene, true, true);
            await SceneManager.UnloadSceneAsync(INIT_SCENE_NAME).ToUniTask();
        }
        catch (Exception exception)
        {
            DebugLogger.LogError($"Load Scene Failed (Title) : {exception.Message}");
        }
    }

    #endregion
}
