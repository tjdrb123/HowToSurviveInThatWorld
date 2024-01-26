
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Fields

    private static T _instance;
    private static bool _isApplicationQuit;
    private static readonly object _Lock = new();

    #endregion



    #region Property

    public static T Instance
    {
        get
        {
            if (_isApplicationQuit)
            {
                DebugLogger.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed.");
                return null;
            }

            lock (_Lock)
            {
                return _instance != null ? _instance : GetInstance();
            }
        }
    }

    private static T GetInstance()
    {
        _instance = FindFirstObjectByType<T>();

        if (_instance == null)
        {
            var singleton = new GameObject { name = "[Singleton] " + typeof(T) };
            _instance = singleton.AddComponent<T>();

            DebugLogger.Log("[Singleton] An instance of '" + typeof(T) + "' created.");
        }

        return _instance;
    }

    #endregion



    #region Unity Behavior

    private void Awake()
    {
        Initialize();
    }

    private void OnApplicationQuit()
    {
        _isApplicationQuit = true;
    }

    private void OnDestroy()
    {
        lock (_Lock)
        {
            Destroyed();
        }
    }

    #endregion



    #region Virtual

    protected virtual void Initialize()
    {
        
    }

    protected virtual void Destroyed()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    #endregion
}
