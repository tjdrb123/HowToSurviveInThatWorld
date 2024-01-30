
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(FiniteStateSO))]
public class StateEditor : Editor
{
    #region Fields
    
    private ReorderableList _list;
    private SerializedProperty _actions;

    #endregion



    #region Unity Behavior

    private void OnEnable()
    {
        Undo.undoRedoPerformed += DoUndo;
        _actions = serializedObject.FindProperty("_StateActions");
        _list = new ReorderableList(serializedObject, _actions, true, true, true, true);
        SetupActionsList(_list);
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= DoUndo;
    }
    
    public override void OnInspectorGUI()
    {
        _list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void DoUndo()
    {
        serializedObject.UpdateIfRequiredOrScript();
    }

    #endregion



    #region Setup

    private static void SetupActionsList(ReorderableList reorderList)
    {
        reorderList.elementHeight *= 1.5f;
        reorderList.drawHeaderCallback += rect => GUI.Label(rect, "Actions");
        reorderList.onAddCallback += list =>
        {
            int count = list.count;
            list.serializedProperty.InsertArrayElementAtIndex(count);
            var prop = list.serializedProperty.GetArrayElementAtIndex(count);
            prop.objectReferenceValue = null;
        };

        reorderList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var r = rect;
            r.height = EditorGUIUtility.singleLineHeight;
            r.y += 5;
            r.x += 5;

            var prop = reorderList.serializedProperty.GetArrayElementAtIndex(index);
            if (prop.objectReferenceValue != null)
            {
                //The icon of the asset SO (basically an object field, cut to show just the icon)
                r.width = 35;
                EditorGUI.PropertyField(r, prop, GUIContent.none);
                r.width = rect.width - 50;
                r.x += 42;

                //The name of the StateAction
                string label = prop.objectReferenceValue.name;
                GUI.Label(r, label, EditorStyles.boldLabel);

                //The description
                r.x += 180;
                r.width = rect.width - 50 - 180;
                string description = (prop.objectReferenceValue as DescriptionActionBaseSO)?.Description;
                GUI.Label(r, description);
            }
            else
                EditorGUI.PropertyField(r, prop, GUIContent.none);
        };

        reorderList.onChangedCallback += list => list.serializedProperty.serializedObject.ApplyModifiedProperties();
        reorderList.drawElementBackgroundCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (isFocused)
                EditorGUI.DrawRect(rect, ContentStyle.Focused);

            EditorGUI.DrawRect(rect, index % 2 != 0 ? ContentStyle.ZebraDark : ContentStyle.ZebraLight);
        };
    }

    #endregion
}
