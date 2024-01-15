using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Util
{
    //제네릭으로 값을 받아서 내가 원하는 컴퍼넌트를 찾은 후 리턴한다.
    public static T FindChild<T>(GameObject go, string name = null) where T : UnityEngine.Object
    {   
        if (go == null)
            return null;
        //오브젝트의 비활성화된 자식들까지 검사하여 T에 해당하는 오브젝트를 가져옴
        T[] components = go.GetComponentsInChildren<T>(true);
        if (string.IsNullOrEmpty(name)) //이름이 null값이면 그냥 첫번째의 값을 가져오고
            return components[0];
        else
            return components.Where(x => x.name == name).FirstOrDefault(); //components에 들어있는 이름과 name 맞으면 값을 가져오거나 없으면 null
    }

    //GameObject는 컴퍼넌트로 찾을 수 없기 때문에 Transform으로 변환 후 검사하여 전달한다.
    public static GameObject FindChild(GameObject go, string name = null) 
    {
        Transform transform = FindChild<Transform>(go, name);
        if (transform == null) 
            return null;
        return transform.gameObject;
    }

    public static T GetOrAddComponent<T>(GameObject obj) where T : Component //이 함수를 통해 오브젝트의 컴퍼넌트를 불러올수 있고 없으면 새로 만들어서 리턴합니다
    { 
        if (!obj.TryGetComponent<T>(out T component))
            component = obj.AddComponent<T>();
        return component;
    }
}
