
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PresenterConfigSO))]
public class PresenterConfigSOEditor : Editor
{
    #region Fields

    private List<Type> _presenterTypes;
    private int _selectedIndex;

    #endregion



    #region Unity Behavior

    private void OnEnable()
    {
        // Presenter를 상속받는 모든 타입을 찾습니다.
        _presenterTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => 
                IsSubclassOfRawGeneric(typeof(Presenter<,>), type) 
                && !type.IsAbstract)
            .ToList();

        // 현재 설정된 타입을 드롭다운에서 선택된 인덱스로 설정합니다.
        var configSO = (PresenterConfigSO)target;
        if (configSO.Type != null)
        {
            _selectedIndex = _presenterTypes.IndexOf(configSO.Type);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 기본 인스펙터 필드 그리기

        var configSO = (PresenterConfigSO)target;

        // 드롭다운 메뉴를 사용하여 Presenter 타입을 선택할 수 있게 합니다.
        _selectedIndex = EditorGUILayout.Popup("Presenter Type", _selectedIndex, _presenterTypes.Select(type => type.Name).ToArray());

        // 선택된 타입을 PresenterConfigSO의 Type 필드에 저장합니다.
        if (_selectedIndex >= 0 && _selectedIndex < _presenterTypes.Count)
        {
            configSO.Type = _presenterTypes[_selectedIndex];
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(configSO);
        }
    }

    #endregion



    #region Util
    
    // Presenter<,>를 상속받는 모든 타입을 찾는 메서드
    private bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                // 제네릭 타입 정의 자체가 아닌 경우에만 true를 반환
                return toCheck != generic;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    #endregion
}