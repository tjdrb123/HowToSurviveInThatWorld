
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.EditorGUI;

public class TransitionDisplayHelper
{
    #region Fields

    public SerializedTransition SerializedTransition { get; }
    private readonly ReorderableList _reorderList;
    private readonly TransitionTableEditor _transitionEditor;

    #endregion



    #region Constructor

    public TransitionDisplayHelper(SerializedTransition serializedTransition, TransitionTableEditor editor)
    {
        SerializedTransition = serializedTransition;
        _reorderList = new ReorderableList(
            SerializedTransition.Transition.serializedObject,
            SerializedTransition.Conditions,
            true, false, true, true);
        SetupConditionsList(_reorderList);
        _transitionEditor = editor;
    }

    #endregion



    #region Edit

    public bool Display(ref Rect position)
    {
        var rect = position;
        float listHeight = _reorderList.GetHeight();
        float singleLineHeight = EditorGUIUtility.singleLineHeight;

        // Reserve Space
        {
            rect.height = singleLineHeight + 10 + listHeight;
            GUILayoutUtility.GetRect(rect.width, rect.height);
            position.y += rect.height + 5;
        }
        
        // Background
        {
            rect.x += 5;
            rect.width -= 10;
            rect.height -= listHeight;
            DrawRect(rect, ContentStyle.DarkGray);
        }

        // Transition Header
        {
            rect.x += 3;
            LabelField(rect, "To");

            rect.x += 20;
            LabelField(rect, SerializedTransition.ToState.objectReferenceValue.name, EditorStyles.boldLabel);
        }
        
        // Buttons
        {
            bool Button(Rect pos, string icon) => GUI.Button(pos, EditorGUIUtility.IconContent(icon));

            var buttonRect = new Rect(x: rect.width - 25, y: rect.y + 5, width: 30, height: 18);

            int i, l;
            {
                var transitions = _transitionEditor.GetStateTransitions(SerializedTransition.FromState.objectReferenceValue);
                l = transitions.Count - 1;
                i = transitions.FindIndex(t => t.Index == SerializedTransition.Index);
            }

            // Remove transition
            if (Button(buttonRect, "Toolbar Minus"))
            {
                _transitionEditor.RemoveTransition(SerializedTransition);
                return true;
            }
            buttonRect.x -= 35;

            // Move transition down
            if (i < l)
            {
                if (Button(buttonRect, "scrolldown"))
                {
                    _transitionEditor.ReorderTransition(SerializedTransition, false);
                    return true;
                }
                buttonRect.x -= 35;
            }

            // Move transition up
            if (i > 0)
            {
                if (Button(buttonRect, "scrollup"))
                {
                    _transitionEditor.ReorderTransition(SerializedTransition, true);
                    return true;
                }
                buttonRect.x -= 35;
            }

            // State editor
            if (Button(buttonRect, "SceneViewTools"))
            {
                _transitionEditor.DisplayStateEditor(SerializedTransition.ToState.objectReferenceValue);
                return true;
            }
        }
        
        rect.x = position.x + 5;
        rect.y += rect.height;
        rect.width = position.width - 10;
        rect.height = listHeight;

        // Display conditions
        _reorderList.DoList(rect);

        return false;
    }
    
    private static void SetupConditionsList(ReorderableList reorderList)
		{
			reorderList.elementHeight *= 2.3f;
			reorderList.headerHeight = 1f;
			reorderList.onAddCallback += list =>
			{
				int count = list.count;
				list.serializedProperty.InsertArrayElementAtIndex(count);
				var prop = list.serializedProperty.GetArrayElementAtIndex(count);
				prop.FindPropertyRelative("ConditionSO").objectReferenceValue = null;
				prop.FindPropertyRelative("ExpectedResult").enumValueIndex = 0;
				prop.FindPropertyRelative("Operator").enumValueIndex = 0;
			};

			reorderList.drawElementCallback += (rect, index, isActive, isFocused) =>
			{
				var prop = reorderList.serializedProperty.GetArrayElementAtIndex(index);
				rect = new Rect(rect.x, rect.y + 2.5f, rect.width, EditorGUIUtility.singleLineHeight);
				var condition = prop.FindPropertyRelative("ConditionSO");

				// Draw the picker for the Condition SO
				if (condition.objectReferenceValue != null)
				{
					string label = condition.objectReferenceValue.name;
					GUI.Label(rect, "If");
					var r = rect;
					r.x += 20;
					r.width = 35;
					EditorGUI.PropertyField(r, condition, GUIContent.none);
					r.x += 40;
					r.width = rect.width - 120;
					GUI.Label(r, label, EditorStyles.boldLabel);
				}
				else
				{
					PropertyField(new Rect(rect.x, rect.y, 150, rect.height), condition, GUIContent.none);
				}

				// Draw the boolean value expected by the condition (i.e. "Is True", "Is False")
				LabelField(new Rect(rect.x + rect.width - 80, rect.y, 20, rect.height), "Is");
				PropertyField(new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height), prop.FindPropertyRelative("ExpectedResult"), GUIContent.none);

				// Only display the logic condition if there's another one after this
				if (index < reorderList.count - 1)
					PropertyField(new Rect(rect.x + 20, rect.y + EditorGUIUtility.singleLineHeight + 5, 60, rect.height), prop.FindPropertyRelative("Operator"), GUIContent.none);
			};

			reorderList.onChangedCallback += list => list.serializedProperty.serializedObject.ApplyModifiedProperties();
			reorderList.drawElementBackgroundCallback += (rect, index, isActive, isFocused) =>
			{
				if (isFocused)
					DrawRect(rect, ContentStyle.Focused);

				if (index % 2 != 0)
					DrawRect(rect, ContentStyle.ZebraDark);
				else
					DrawRect(rect, ContentStyle.ZebraLight);
			};
		}

    #endregion
}
