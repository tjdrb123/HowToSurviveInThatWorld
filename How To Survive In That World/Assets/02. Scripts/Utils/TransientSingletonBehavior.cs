
using UnityEngine;

public class TransientSingletonBehavior<T> : SingletonBase where T : MonoBehaviour
{
    // Field Member
    private static T _instance;



    #region Properties

    public static T Instance
    {
        get
        {
            if (_isDisabled)
            {
                DebugLogger.LogError("[TransientSingleton] Instance '" + typeof(T) + "' already destroyed. Returning Null Instance");
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
            
            DebugLogger.Log("[TransientSingleton] An instance of '" + typeof(T) + "' created");
        }
        else
        {
            DebugLogger.Log("[TransientSingleton] Using instance already created: '" + typeof(T) + "'");
        }

        return _instance;
    }

    #endregion



    #region Disabled Singleton

    protected void OnDisable()
    {
        if (_instance == this)
        {
            _isDisabled = true;
        }
    }

    #endregion
}
