
using UnityEngine;

public class SingletonPersist<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Initialize()
    {
        // this == _instance
        DontDestroyOnLoad(this);
    }
}
