using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    //Dictionary자식에 있는 Type과 타입에 해당하는 오브젝트를 관리합니다.
    private Dictionary<Type, UnityEngine.Object[]> _objects = new();

    private bool _initialized = false;

    protected virtual void OnEnable()
    {
        Initialize();
    }

    //초기화를 했는지 안했는지 체크하는 함수입니다.
    //여기서 Protected가 아닌 public으로 하는 이유는 초기화를 다른 클래스에서도 하기 위함입니다.
    public virtual bool Initialize()
    {
        if (_initialized == true)
            return false;

        _initialized = true;
        return true;
    }

    protected virtual void SetOrder() //여기에서 sortingOrder의 값을 초기화 해줍니다
    {

    }

    private void Bind<T>(Type type) where T : UnityEngine.Object  
    {
        string[] names = Enum.GetNames(type); //type의 값들을 다 가져와서 사용함
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; //type의 값들의 크기만큼 배열크기 지정

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = typeof(T) == typeof(GameObject) ? this.gameObject.FindChild(names[i]) : gameObject.FindChild<T>(names[i]);
        }
        _objects.Add(typeof(T), objects);
    }

    //Bind를 이용해서 dictionary에 값들을 묶어서 저장합니다.
    protected void BindObject(Type type) => Bind<GameObject>(type);
    protected void BindText(Type type) => Bind<TextMeshProUGUI>(type);
    protected void BindButton(Type type) => Bind<Button>(type);
    protected void BindImage(Type type) => Bind<Image>(type);


    private T Get<T>(int index) where T : UnityEngine.Object
    {
        if (!_objects.TryGetValue(typeof(T), out UnityEngine.Object[] objs))
            return null;
        if (index < 0 && index >= _objects.Count)
            return null;
        return objs[index] as T;
    }

    protected GameObject GetObject(int index) => Get<GameObject>(index);
    protected TextMeshProUGUI GetText(int index) => Get<TextMeshProUGUI>(index);
    protected Button GetButton(int index) => Get<Button>(index);
    protected Image GetImage(int index) => Get<Image>(index);

}
