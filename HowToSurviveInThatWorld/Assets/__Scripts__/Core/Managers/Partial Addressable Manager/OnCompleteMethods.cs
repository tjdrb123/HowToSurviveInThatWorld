
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

/// <summary>
/// # OnCompleteMethods
///   - 완료 되었을 때 실행될 콜백 메서드 모음
/// </summary>
public partial class Manager_Addressable
{
    /// <summary>
    /// # 어드레서블 초기화 완료 콜백 메서드
    /// </summary>
    /// <param name="handle">비동기 핸들</param>
    /// <param name="onSucceeded">완료됐을 때 실행 될 콜백</param>
    /// <param name="onFailed">실패했을 때 실행 될 콜백</param>
    private void OnInitializeCompleted(
        AsyncOperationHandle<IResourceLocator> handle,
        Action onSucceeded = null,
        Action onFailed = null)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            onFailed?.Invoke();
            return;
        }

        _keys.AddRange(handle.Result.Keys);
        onSucceeded?.Invoke();
    }

    /// <summary>
    /// # 어드레서블 리소스 위치 비동기 작업 완료 콜백 메서드
    /// </summary>
    /// <param name="handle">비동기 핸들</param>
    /// <param name="key">오브젝트 키(어드레서블 네임 또는 레이블)</param>
    /// <param name="onSucceeded">완료됐을 때 실행 될 콜백</param>
    /// <param name="onFailed">실패했을 때 실행 될 콜백</param>
    private void OnLoadLocationCompleted(
        AsyncOperationHandle<IList<IResourceLocation>> handle,
        object key,
        Action<object> onSucceeded = null,
        Action<object> onFailed = null)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            onFailed?.Invoke(key);
            return;
        }

        bool isSucceeded = false;

        foreach (var location in handle.Result)
        {
            if(!IsInvertValidKey(key.ToString(), out var label)) continue;
            
            // _location Dictionary에 해당 키가 존재하지 않다면? 새 리스트를 생성
            if (!_locations.ContainsKey(label))
            {
                _locations.Add(label, new List<IResourceLocation>());
            }

            // _location Value (List<IResourceLocation>) 캐싱 하여 찾는 작업
            // var list는 _location의 Value로 캐싱
            var list = _locations[label];
            var index = list.FindIndex(x => string.Equals(x.InternalId, location.InternalId));

            // Equals 존재하지 않는 다면 -1을 반환, 해당 location을 Value에 추가
            if (index < 0)
            {
                list.Add(location);
                isSucceeded = true;
            }
        }

        if (isSucceeded)
        {
            onSucceeded?.Invoke(key);
        }
    }

    /// <summary>
    /// # 어드레서블 단일 자산 에셋을 완료 콜백 메서드
    ///   - 예외처리 (비동기 핸들 검증, 결과 값 검증, 키 중복 검사)
    /// </summary>
    /// <param name="handle">비동기 핸들</param>
    /// <param name="key">리소스 어드레서블 네임</param>
    /// <param name="useReference">AssetReference를 사용하는지에 대한 여부</param>
    /// <param name="onSucceeded">완료됐을 때 실행 될 콜백</param>
    /// <param name="onFailed">실패했을 때 실행 될 콜백</param>
    private void OnLoadAssetCompleted<T>(
        AsyncOperationHandle<T> handle,
        string key,
        bool useReference,
        Action<string, T> onSucceeded = null,
        Action<string> onFailed = null)
        where T : Object
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            onFailed?.Invoke(key);
            return;
        }

        if (!handle.Result)
        {
            DebugLogger.LogError(useReference
                ? AddressableException.CannotLoadAssetReference<T>(key)
                : AddressableException.CannotLoadAssetKey<T>(key));

            onFailed?.Invoke(key);
            return;
        }

        if (_assets.ContainsKey(key))
        {
            if (!(_assets[key] is T))
            {
                DebugLogger.LogError(useReference
                    ? AddressableException.AssetReferenceExist(_assets[key].GetType(), key)
                    : AddressableException.AssetKeyExist(_assets[key].GetType(), key));
                
                onFailed?.Invoke(key);
                return;
            }
        }
        else
        {
            // 위 예외에서 하나도 걸리지 않았다면 _assets에 캐싱 (메모리 적재)
            _assets.Add(key, handle.Result);
        }

        onSucceeded?.Invoke(key, handle.Result);
    }

    private void OnLoadAssetsCompleted<T>(
        AsyncOperationHandle<IList<T>> handle,
        string label,
        Action<string, List<T>> onSucceeded = null,
        Action<string> onFailed = null)
        where T : Object
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            onFailed?.Invoke(label);
            return;
        }

        var results = handle.Result;
        if (results == null || results.Count != _locations[label].Count)
        {
            DebugLogger.LogError(AddressableException.CannotLoadAssetsLabel<T>(label));

            onFailed?.Invoke(label);
            return;
        }
    }
}
