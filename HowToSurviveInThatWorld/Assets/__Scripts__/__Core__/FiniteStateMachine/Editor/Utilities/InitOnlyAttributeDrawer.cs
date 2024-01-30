
using UnityEditor;
using UnityEngine;

public class InitOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(InitOnlyAttribute))]
public class InitOnlyAttributeDrawer : PropertyDrawer
{
    private static readonly string _Text = "Changes to this parameter during Play mode won't be reflected on existing StateMachines";
    private static readonly GUIStyle _Style = new GUIStyle(GUI.skin.GetStyle("helpbox")) { padding = new RectOffset(5, 5, 5, 5) };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorApplication.isPlaying)
        {
            position.height = _Style.CalcHeight(new GUIContent(_Text), EditorGUIUtility.currentViewWidth);
            EditorGUI.HelpBox(position, _Text, MessageType.Info);
            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUI.GetPropertyHeight(property, label);
        }

        EditorGUI.PropertyField(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property, label);

        if (EditorApplication.isPlaying)
        {
            height += _Style.CalcHeight(new GUIContent(_Text), EditorGUIUtility.currentViewWidth) + EditorGUIUtility.standardVerticalSpacing * 4;
        }

        return height;
    }
}
