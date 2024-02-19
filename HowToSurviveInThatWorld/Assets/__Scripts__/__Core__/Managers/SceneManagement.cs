
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    #region Fields

    [Header("Game Mechanism")] 
    [SerializeField] private GameSceneSO _GamePlayScene;
    [SerializeField] private InputReader _InputReader;

    [Header("Listening to Event")]
    [SerializeField] private LoadEventChannelSO _LoadTitleSceneChannel;
    [SerializeField] private LoadEventChannelSO _LoadSelectorSceneChannel;
    [SerializeField] private LoadEventChannelSO _LoadLobbySceneChannel;
    [SerializeField] private LoadEventChannelSO _LoadGameLocationChannel;

    private SceneInstance _gamePlayInstance;
    private SceneInstance _loadingInstance;
    
    // Parameters
    private GameSceneSO _sceneToLoad;           // 로드 할 씬
    private GameSceneSO _currentLoadedScene;    // 로드 된 씬
    
    private bool _showLoadingScreen;
    private bool _isLoading;
    private float _fadeDuration = 0.5f;

    #endregion



    #region Unity Behavior

    private void OnEnable()
    {
        _LoadTitleSceneChannel.OnLoadingRequest += LoadTitleScene;
    }

    private void OnDisable()
    {
        _LoadTitleSceneChannel.OnLoadingRequest -= LoadTitleScene;
    }

    #endregion



    #region Load Scenes

    private async UniTask LoadTitleScene(GameSceneSO titleToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        if (_isLoading) return;

        _sceneToLoad = titleToLoad;
        _showLoadingScreen = showLoadingScreen;
        _isLoading = true;

        // _gamePlayInstance.Scene != null && _gamePlayInstance.Scene.isLoaded
        if (_gamePlayInstance.Scene is { isLoaded: true })
        {
            await Addressables.UnloadSceneAsync(_gamePlayInstance).ToUniTask();
        }

        await UnloadPreviousScene();
    }

    #endregion



    #region Utils

    private async UniTask UnloadPreviousScene()
    {
        _InputReader.DisableAllInput();
        // TODO. FadeOut

        // Fade Duration 만큼 대기
        await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration));

        if (_currentLoadedScene != null)
        {
            if (_currentLoadedScene.SceneReference.OperationHandle.IsValid())
            {
                await _currentLoadedScene.SceneReference.UnLoadScene().ToUniTask();
            }
        }

        await LoadNewScene();
    }

    private async UniTask LoadNewScene()
    {
        if (_showLoadingScreen)
        {
            // TODO. Loading Screen
        }

        var loadScene = await _sceneToLoad.SceneReference.LoadSceneAsync(
            LoadSceneMode.Additive, true, 0).ToUniTask();

        // Complete Callback
        NewSceneLoadedCallback(loadScene);
    }

    private void NewSceneLoadedCallback(SceneInstance loadScene)
    {
        _currentLoadedScene = _sceneToLoad;

        var scene = loadScene.Scene;
        SceneManager.SetActiveScene(scene);
        
        _isLoading = false;

        if (_showLoadingScreen)
        {
            // TODO.
        }
        
        // TODO.Fade

        StartGame();
    }

    private void StartGame()
    {
        // TODO. Event
    }

    #endregion
}