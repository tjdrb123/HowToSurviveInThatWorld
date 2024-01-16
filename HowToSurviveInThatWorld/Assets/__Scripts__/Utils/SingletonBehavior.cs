
using UnityEngine;

public class SingletonBehavior<T> : SingletonBase where T : MonoBehaviour
{
    // Field Members
    private static T _instance;



    #region Properties

    public static T Instance
    {
        get
        {
            if (_isDisabled)
            {
                DebugLogger.LogError("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning Null Instance");
                return null;
            }

            lock (_Locked)
            {
                return GetInstance();
            }
        }
    }

    #endregion
    
    
    
    #region Properties Util Methods

    private static T GetInstance()
    {
        if (_instance != null) return _instance;
        
        _instance = (T)FindFirstObjectByType(typeof(T));

        if (_instance == null)
        {
            var singletonObject = new GameObject { name = "[Singleton] " + typeof(T) };
                
            _instance = singletonObject.AddComponent<T>();

            DontDestroyOnLoad(singletonObject);

            DebugLogger.Log("[Singleton] An instance of '" + typeof(T) + "' created with DontDestroyOnLoad");
        }
        else
        {
            DontDestroyOnLoad(_instance);
                
            DebugLogger.Log("[Singleton] Using instance already created. '" + typeof(T) + "' so Update DontDestroyOnLoad");
        }

        return _instance;
    }

    #endregion
    
    
    
    #region Disabled Singleton

    protected virtual void OnDisable()
    {
        if (_instance == this)
        {
            _isDisabled = true;
        }
    }

    #endregion
}