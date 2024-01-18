
using System;
using UnityEngine.AddressableAssets;

public static class AddressableException
{
    #region Fields

    private const string CANNOT_FIND_ASSET_BY_KEY = "Cannot find any asset by key={0}.";
    // private const string NoInstanceKeyInitialized = "No instance with key={0} has been instantiated through AddressablesManager.";
    // private const string NoInstanceReferenceInitialized = "No instance with key={0} has been instantiated through AddressablesManager.";
    // private const string AssetKeyNotInstanceOf = "The asset with key={0} is not an instance of {1}.";
    // private const string assetReferenceNotInstanceOf = "The asset with reference={0} is not an instance of {1}.";
    // private const string noSceneKeyLoaded = "No scene with key={0} has been loaded through AddressablesManager.";
    // private const string noSceneReferenceLoaded = "No scene with reference={0} has been loaded through AddressablesManager.";
    private const string CANNOT_LOAD_ASSET_BY_KEY = "Cannot load any asset of type={0} by key={1}.";
    private const string CANNOT_LOAD_ASSET_BY_REFERENCE = "Cannot load any asset of type={0} by reference={1}.";
    private const string ASSET_KEY_EXIST = "An asset of type={0} has been already registered with key={1}.";
    private const string ASSET_REFERENCE_EXIST = "An asset of type={0} has been already registered with reference={1}.";
    // private const string cannotInstantiateKey = "Cannot instantiate key={0}.";
    // private const string cannotInstantiateReference = "Cannot instantiate reference={0}.";

    public static readonly InvalidKeyException InvalidReference = new InvalidKeyException("Reference is invalid.");

    #endregion



    #region Get String

    public static string CannotFindAssetByKey(string key)
        => string.Format(CANNOT_FIND_ASSET_BY_KEY, key);

    public static string CannotLoadAssetKey<T>(string key)
        => string.Format(CANNOT_LOAD_ASSET_BY_KEY, typeof(T), key);

    public static string CannotLoadAssetReference<T>(string key)
        => string.Format(CANNOT_LOAD_ASSET_BY_REFERENCE, typeof(T), key);

    public static string AssetKeyExist(Type type, string key)
        => string.Format(ASSET_KEY_EXIST, type, key);

    public static string AssetReferenceExist(Type type, string key)
        => string.Format(ASSET_REFERENCE_EXIST, type, key);

    #endregion
}
