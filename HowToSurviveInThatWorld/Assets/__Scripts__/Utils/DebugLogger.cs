
using UnityEngine;

public static class DebugLogger
{
    public static void Log(string msg)
    {
        #if DEBUG_MODE
        Debug.Log(msg);
        #endif
    }

    public static void LogWarning(string msg)
    {
        #if DEBUG_MODE
        Debug.LogWarning(msg);
        #endif
    }

    public static void LogError(string msg)
    {
        #if DEBUG_MODE
        Debug.LogError(msg);
        #endif
    }
}
