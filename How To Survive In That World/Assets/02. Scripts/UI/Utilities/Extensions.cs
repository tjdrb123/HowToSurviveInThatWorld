using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
    public static GameObject FindChild(this GameObject go, string name) => Util.FindChild(go, name);
    public static T FindChild<T>(this GameObject go, string name) where T : UnityEngine.Object => Util.FindChild<T>(go, name);
}
