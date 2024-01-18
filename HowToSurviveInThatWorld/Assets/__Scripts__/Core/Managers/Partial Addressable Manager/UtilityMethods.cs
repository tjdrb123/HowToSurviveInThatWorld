
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

/// <summary>
/// # Addressable Manager - UtilityMethods
/// </summary>
public partial class Manager_Addressable
{
    #region Clear Method

    private void ClearFields()
    {
        _keys.Clear();
        _locations.Clear();
        _assets.Clear();
    }

    #endregion



    #region Contains

    public bool ContainsAsset(string key) => _assets.ContainsKey(key);
    public bool ContainsKey(object key) => _keys.Contains(key);

    #endregion
    
    
    
    #region Invalid Invert Key

    /// <summary>
    /// # 해당 키가 유효한지 검증하는 메서드
    /// </summary>
    /// <param name="key">검증할 키 값</param>
    /// <param name="result">검증이 된 키 값, 만약 `key`가 `null`이라면 빈 문자열 대체</param>
    /// <returns>`key`가 null이 아니고 빈 값이 아닐 경우 false, 유효하지 않으면 true</returns>
    private bool IsInvertValidKey(string key, out string result)
    {
        result = key ?? string.Empty;

        return !string.IsNullOrEmpty(result);
    }

    /// <summary>
    /// # 해당 키가 유효한지 검증하는 메서드 (Asset Reference)
    /// </summary>
    /// <param name="reference">검증할 키 값</param>
    /// <param name="result">검증이 된 키 값, 만약 `key`가 `null`이라면 빈 문자열 대체</param>
    /// <returns>`key`가 null이 아니고 빈 값이 아닐 경우 false, 유효하지 않으면 true</returns>
    private bool IsInvertValidKey(AssetReference reference, out string result)
    {
        if (reference == null)
        {
            DebugLogger.LogError(new ArgumentNullException(nameof(reference)).Message);

            result = string.Empty;
        }
        else
        {
            result = reference.RuntimeKey.ToString();
        }

        return !string.IsNullOrEmpty(result);
    }

    #endregion



    #region Getter

    /// <summary>
    /// # IResourceLocation을 반환하는 메서드
    ///   - 어드레서블 매니저 초기화 이후 자산 정보를 추적 가능한 로케이션을 반환
    /// </summary>
    public IReadOnlyList<IResourceLocation> GetLocation(string key)
    {
        if (!IsInvertValidKey(key, out key))
        {
            DebugLogger.LogError(new InvalidKeyException(key).Message);

            return _noLocation;
        }

        if (!_locations.TryGetValue(key, out var list))
        {
            return _noLocation;
        }

        return list;
    }

    public T GetAsset<T>(string key) where T : Object
    {
        if (!IsInvertValidKey(key, out key))
        {
            DebugLogger.LogError(new InvalidKeyException(key).Message);

            return default;
        }

        return GetAssetInternal<T>(key);
    }

    public T GetAsset<T>(AssetReference reference) where T : Object
    {
        if (!IsInvertValidKey(reference, out var key))
        {
            DebugLogger.LogError(new InvalidKeyException(key).Message);

            return default;
        }

        return GetAssetInternal<T>(key);
    }

    private T GetAssetInternal<T>(string key) where T : Object
    {
        if (!_assets.ContainsKey(key))
        {
            DebugLogger.LogWarning(AddressableException.CannotFindAssetByKey(key));

            return default;
        }

        if (_assets[key] is T asset)
        {
            return asset;
        }
        
        return default;
    }

    #endregion



    #region Release

    public void ReleaseAsset(string key)
    {
        if (!IsInvertValidKey(key, out key))
        {
            DebugLogger.LogError(new InvalidKeyException(key).Message);

            return;
        }

        if (!_assets.TryGetValue(key, out var asset))
            return;

        _assets.Remove(key);
        Addressables.Release(asset);
    }

    public void ReleaseAsset(AssetReference reference)
    {
        if (!IsInvertValidKey(reference, out var key))
        {
            DebugLogger.LogError(new InvalidKeyException(key).Message);

            return;
        }

        if (!_assets.ContainsKey(key))
            return;

        _assets.Remove(key);
        reference.ReleaseAsset();
    }

    #endregion



    #region Utils

    /// <summary>
    /// # 해당 로케이션(어드레서블 네임)이 존재하는지 검증 하는 메서드
    /// </summary>
    /// <param name="locationKey">어드레서블 네임</param>
    /// <returns>존재하면 True / 존재하지 않으면 False</returns>
    private bool IsInvalidLocation(string locationKey)
    {
        bool locationExist = _locations.Values.Any
            (locationList => locationList.Exists(location => location.PrimaryKey == locationKey));

        return locationExist;
    }

    #endregion
}
