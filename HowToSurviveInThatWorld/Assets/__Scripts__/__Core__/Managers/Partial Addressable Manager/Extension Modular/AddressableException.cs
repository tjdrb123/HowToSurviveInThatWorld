
using System;
using UnityEngine.AddressableAssets;

public static class AddressableException
{
    #region Fields

    private const string CANNOT_FIND_ASSET_BY_KEY = "Cannot find any asset by key={0}.";
    
    private const string CANNOT_LOAD_ASSET_BY_KEY = "Cannot load any asset of type={0} by key={1}.";
    private const string CANNOT_LOAD_ASSET_BY_REFERENCE = "Cannot load any asset of type={0} by reference={1}.";
    private const string CANNOT_LOAD_ASSETS_BY_LABEL = "Cannot load any assets of type={0} by label={1}.";
    
    private const string ASSET_KEY_EXIST = "An asset of type={0} has been already registered with key={1}.";
    private const string ASSET_REFERENCE_EXIST = "An asset of type={0} has been already registered with reference={1}.";
    
    private const string LOCATION_NOT_FOUND = "No resource location found for key={0}.";
    private const string LOCATION_FOR_LABEL_NOT_FOUND = "No locations found for label={0}.";

    public static readonly InvalidKeyException InvalidReference = new InvalidKeyException("Reference is invalid.");

    #endregion



    #region Get String

    public static string CannotFindAssetByKey(string key)
        => string.Format(CANNOT_FIND_ASSET_BY_KEY, key);

    public static string CannotLoadAssetKey<T>(string key)
        => string.Format(CANNOT_LOAD_ASSET_BY_KEY, typeof(T), key);

    public static string CannotLoadAssetReference<T>(string key)
        => string.Format(CANNOT_LOAD_ASSET_BY_REFERENCE, typeof(T), key);

    public static string CannotLoadAssetsLabel<T>(string label)
        => string.Format(CANNOT_LOAD_ASSETS_BY_LABEL, typeof(T), label);

    public static string AssetKeyExist(Type type, string key)
        => string.Format(ASSET_KEY_EXIST, type, key);

    public static string AssetReferenceExist(Type type, string key)
        => string.Format(ASSET_REFERENCE_EXIST, type, key);

    public static string LocationNotFound(string key)
        => string.Format(LOCATION_NOT_FOUND, key);

    public static string LocationForLabelNotFound(string label)
        => string.Format(LOCATION_FOR_LABEL_NOT_FOUND, label);

    #endregion
}
