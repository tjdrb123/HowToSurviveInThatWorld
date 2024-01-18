
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

/// <summary>
/// # UniTask Methods
///   - 유니티 비동기를 최대한 이용하기 위한 메서드들
///   - InitializeAddressable
///   - LoadLocations
///   - LoadAsset(s)
/// </summary>
public partial class Manager_Addressable
{
    #region Initialize Addressable

    public async UniTask<OperationResult<IResourceLocator>> InitializeAsync(bool autoReleaseHandle = true)
    {
        ClearFields();

        try
        {
            // Addressable 수동 초기화
            // 런타임 데이터 설정을 담당하는 예비 작업
            var operation = Addressables.InitializeAsync(false);
            await operation;

            // 해당 오퍼레이션이 끝난후 컴플리트 콜백 메서드 호출
            OnInitializeCompleted(operation, OnSucceededByInit);

            // Operation Result (핸들 - Constructor)로 result 생성
            var result = new OperationResult<IResourceLocator>(operation);

            if (autoReleaseHandle)
            {
                Addressables.Release(operation);
            }

            return result;
        }
        catch (Exception exception)
        {
            DebugLogger.LogError(exception.Message);
            return new OperationResult<IResourceLocator>(false, default, default);
        }
    }

    #endregion



    #region Load Location Async

    /// <summary>
    /// # 모든 리소스 위치를 가져오는 메서드
    ///   - 실제 리소스를 로드하지 않음
    ///   - 해당 리소스가 있는 위치 정보만을 제공
    ///   - 실제 로드 전 필요한 메타데이터를 얻는데 사용
    /// </summary>
    /// <param name="key">레이블</param>
    /// <param name="type">로드하고자 하는 리소스 클래스 타입 (GameObject, Texture2D 등)</param>
    /// <returns></returns>
    public async UniTask<OperationResult<IList<IResourceLocation>>> LoadLocationAsync(object key, Type type = null)
    {
        if (key == null)
        {
            DebugLogger.LogError(new InvalidKeyException((object)null).Message);
            return new OperationResult<IList<IResourceLocation>>(false, null, default);
        }

        try
        {
            // 키에 해당하는 모든 리소스 위치를 비동기적으로 로드.
            var operation = Addressables.LoadResourceLocationsAsync(key, type);
            await operation;
            
            // 리소스 위치 로드 완료 콜백 메서드
            OnLoadLocationCompleted(operation, key);
            
            return new OperationResult<IList<IResourceLocation>>(key, operation);
        }
        catch (Exception exception)
        {
            DebugLogger.LogError(exception.Message);
            return new OperationResult<IList<IResourceLocation>>(false, key, default);
        }
    }
    
    #endregion



    #region Load Asset (Singly Asset)

    /// <summary>
    /// # 실제 단일 자산을 가져오는 메서드
    ///   - 완료되면 _asset 딕셔너리에 추가 되고 실제 메모리에 적재 됨.
    /// </summary>
    /// <param name="key">리소스 어드레서블 네임</param>
    /// <typeparam name="T">UnityEngine.Object Type (GameObject, Texture2D 등)</typeparam>
    public async UniTask<OperationResult<T>> LoadAssetAsync<T>(string key) where T : Object
    {
        if (!IsInvertValidKey(key, out key))
        {
            DebugLogger.LogError(new InvalidKeyException(key).Message);
            return new OperationResult<T>(false, key, default);
        }

        // 해당 딕셔너리에 키가 존재한다면 그대로 반환
        if (_assets.TryGetValue(key, out var outAsset))
        {
            if (outAsset is T asset)
            {
                return new OperationResult<T>(true, key, asset);
            }

            return new OperationResult<T>(false, key, default);
        }

        // 초기화 리소스 로케이션에 해당 정보가 있는지 검증
        // 리소스 로케이션(레이블)에 존재하지 않는다면 해당하는 에셋이 아님.
        if (!IsInvalidLocation(key))
        {
            DebugLogger.LogWarning(AddressableException.LocationNotFound(key));
            return new OperationResult<T>(false, key, default);
        }

        try
        {
            // 비동기 핸들을 통해 LoadAssetAsync (T 타입에 해당하는 리소스를 실제로 로드함)
            var operation = Addressables.LoadAssetAsync<T>(key);
            await operation;
            
            // 완료 콜백 메서드
            OnLoadAssetCompleted(operation, key, false);
            
            return new OperationResult<T>(key, operation);
        }
        catch (Exception exception)
        {
            DebugLogger.LogError(exception.Message);
            return new OperationResult<T>(false, key, default);
        }
    }

    /// <summary>
    /// # 실제 단일 자산을 가져오는 메서드 (Asset Reference)
    ///   - 완료 되면 _asset 딕셔너리에 추가(캐싱) 되고 메모리 적재
    /// </summary>
    /// <param name="reference">리소스 에셋 레퍼런스 타입(GameObject, Texture2D 등)</param>
    public async UniTask<OperationResult<T>> LoadAssetAsync<T>(AssetReferenceT<T> reference) where T : Object
    {
        if (!IsInvertValidKey(reference, out var key))
        {
            DebugLogger.LogError(new InvalidKeyException(key).Message);
            return new OperationResult<T>(false, reference, default);
        }
        
        // 해당 딕셔너리에 키가 존재한다면 그대로 반환
        if (_assets.TryGetValue(key, value: out var outAsset))
        {
            if (outAsset is T asset)
            {
                return new OperationResult<T>(true, reference, asset);
            }

            return new OperationResult<T>(false, reference, default);
        }
        
        // 초기화 리소스 로케이션에 해당 정보가 있는지 검증
        // 리소스 로케이션(레이블)에 존재하지 않는다면 해당하는 에셋이 아님.
        if (!IsInvalidLocation(key))
        {
            DebugLogger.LogWarning(AddressableException.LocationNotFound(key));
            return new OperationResult<T>(false, key, default);
        }
        
        try
        {
            // AssetReference를 통해 LoadAssetAsync (T 타입에 해당하는 리소스를 실제로 로드함)
            var operation = reference.LoadAssetAsync<T>();
            await operation;
            
            // 완료 콜백 메서드
            OnLoadAssetCompleted(operation, key, true);
            
            return new OperationResult<T>(reference, operation);
        }
        catch (Exception exception)
        {
            DebugLogger.LogError(exception.Message);
            return new OperationResult<T>(false, reference, default);
        }
    }
    
    #endregion

    

    #region Load All Assets (Multiple Asset Load)
    
    public async UniTask<OperationResult<T>> LoadAllAssetAsync<T>(string label) where T : Object
    {
        if (!IsInvertValidKey(label, out label))
        {
            DebugLogger.LogError(new InvalidKeyException(label).Message);
            return new OperationResult<T>(false, label, default);
        }

        // 해당 레이블이 존재하지 않는다면
        if (!_locations.ContainsKey(label))
        {
            DebugLogger.LogError(AddressableException.LocationForLabelNotFound(label));
            return new OperationResult<T>(false, label, default);
        }

        try
        {
            // var operation = Addressables.LoadAssetsAsync<T>(_locations[label], null);
            //     // obj => { DebugLogger.Log(obj.name); });
            // await operation;
            
            foreach (var location in _locations[label])
            {
                var operation = LoadAssetAsync<T>(location.PrimaryKey);
                await operation;
            }

            return new OperationResult<T>(true, label, default);
        }
        catch (Exception exception)
        {
            DebugLogger.LogError(exception.Message);
            return new OperationResult<T>(false, label, default);
        }
    }

    #endregion
}