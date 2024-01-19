
using UnityEngine;
using Object = UnityEngine.Object;

public class SceneBase : MonoBehaviour
{
    #region Fields
    
    // Default = false
    private bool _isInitialize;

    /* Properties */
    // Addressable Assets Load Label
    // 상속받는 씬에서 필요한 레이블
    //   - ex) LobbyScene => Literals.LABEL_SCENE_LOBBY;
    //   - 위와 같은 형태로 오버라이드 시켜 줘야만 함.
    protected virtual string AddressableLabel => Literals.LABEL_EMPTY;
    
    // Scene UI
    public UI_Scene SceneUI { get; protected set; }

    #endregion



    #region Unity Behavior

    private void Awake()
    {
        // Addressable Manager Init이 안됐을 경우
        if (!Managers.Addressable.IsInitialize)
        {
            InitializeAddressable();
            LoadAddressableCommon();
            LoadAddressable();
        }
        // 아닐 경우 (초기화가 되어 있을 경우)
        //   - 해당 씬에 맞는 로케이션으로 초기화
        //   - 해당 씬에 맞는 에셋들 캐싱
        else
        {
            LoadAddressableCommon();
            LoadAddressable();
        }
    }

    #endregion



    #region Addressables

    /// <summary>
    /// # 어드레서블 초기화 메서드
    /// </summary>
    private async void InitializeAddressable()
    {
        var initResult = await Managers.Addressable.InitializeAsync();
        if (!initResult.IsSucceeded)
        {
            DebugLogger.LogError("Failed to initialize Addressable Manager.");
        }
    }

    private async void LoadAddressableCommon()
    {
        if (string.IsNullOrEmpty(AddressableLabel))
        {
            DebugLogger.LogError("Label is Null or Empty");
            return;
        }
        
        // 리소스 로케이션 정보 로드
        var locationResult = await Managers.Addressable.LoadLocationAsync(Literals.LABEL_COMMON);
        if (!locationResult.IsSucceeded)
        {
            DebugLogger.LogError($"Failed to load location with label {Literals.LABEL_COMMON}");
            return;
        }
        
        // 실제 자산(Assets) 캐싱 작업
        var assetsResult = await Managers.Addressable.LoadAllAssetAsync<Object>(Literals.LABEL_COMMON);
        if (!assetsResult.IsSucceeded)
        {
            DebugLogger.LogError($"Failed to load location with label {Literals.LABEL_COMMON}");
        }
    }

    private async void LoadAddressable()
    {
        if (string.IsNullOrEmpty(AddressableLabel))
        {
            DebugLogger.LogError("Label is Null or Empty");
            _isInitialize = false;
        }

        // 리소스 로케이션 정보 로드
        var locationResult = await Managers.Addressable.LoadLocationAsync(AddressableLabel);
        if (!locationResult.IsSucceeded)
        {
            DebugLogger.LogError($"Failed to load location with label {AddressableLabel}");
            _isInitialize = false;
        }
        
        // 실제 자산(Assets) 캐싱 작업
        var assetsResult = await Managers.Addressable.LoadAllAssetAsync<Object>(AddressableLabel);
        if (!assetsResult.IsSucceeded)
        {
            DebugLogger.LogError($"Failed to load location with label {AddressableLabel}");
            _isInitialize = false;
        }
        else
        {
            _isInitialize = true;
        }

        Initialize();
    }

    #endregion



    #region Virtual (override)

    /// <summary>
    /// # 반드시 오버라이드를 해서 사용해야함.
    ///   - Awake나 Start를 사용하는 것이 아닌 Initialize() 메서드를 사용.
    /// </summary>
    /// <returns></returns>
    protected virtual bool Initialize()
    {
        return _isInitialize;
    }

    #endregion
}
