
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.EditorGUI;

public class AddTransitionHelper : IDisposable
{
    #region Fields
    
    // Class
    public class TransitionItemSO : ScriptableObject
    {
        public TransitionTableSO.TransitionItem Item;
    }

    private SerializedTransition SerializedTransition { get; }
    private readonly SerializedObject _transition;
    private readonly ReorderableList _list;
    private readonly TransitionTableEditor _transitionEditor;
    private bool _toggle;

    #endregion



    #region Constructor

    public AddTransitionHelper(TransitionTableEditor editor)
    {
        _transitionEditor = editor;
        _transition = new SerializedObject(ScriptableObject.CreateInstance<TransitionItemSO>());
        SerializedTransition = new SerializedTransition(_transition.FindProperty("Item"));
        _list = new ReorderableList(_transition, SerializedTransition.Conditions);
        SetupConditionsList(_list);
    }

    #endregion



    #region Display

    public void Display(Rect position)
	{
		position.x += 8;
		position.width -= 16;
		var rect = position;
		float listHeight = _list.GetHeight();
		float singleLineHeight = EditorGUIUtility.singleLineHeight;

		// Display add button only if not already adding a transition
		if (!_toggle)
		{
			position.height = singleLineHeight;

			// Reserve space
			GUILayoutUtility.GetRect(position.width, position.height);

			if (GUI.Button(position, "Add Transition"))
			{
				_toggle = true;
				SerializedTransition.ClearProperties();
			}

			return;
		}

		// Background
		{
			position.height = listHeight + singleLineHeight * 4;
			DrawRect(position, ContentStyle.LightGray);
		}

		// Reserve space
		GUILayoutUtility.GetRect(position.width, position.height);

		// State Fields
		{
			position.y += 10;
			position.x += 20;
			StatePropField(position, "From", SerializedTransition.FromState);
			position.x = rect.width / 2 + 20;
			StatePropField(position, "To", SerializedTransition.ToState);
		}

		// Conditions List
		{
			position.y += 30;
			position.x = rect.x + 5;
			position.height = listHeight;
			position.width -= 10;
			_list.DoList(position);
		}

		// Add and cancel buttons
		{
			position.y += position.height + 5;
			position.height = singleLineHeight;
			position.width = rect.width / 2 - 20;
			if (GUI.Button(position, "Add Transition"))
			{
				if (SerializedTransition.FromState.objectReferenceValue == null)
					Debug.LogException(new ArgumentNullException("FromState"));
				else if (SerializedTransition.ToState.objectReferenceValue == null)
					Debug.LogException(new ArgumentNullException("ToState"));
				else if (SerializedTransition.FromState.objectReferenceValue == SerializedTransition.ToState.objectReferenceValue)
					Debug.LogException(new InvalidOperationException("FromState and ToState are the same."));
				else
				{
					_transitionEditor.AddTransition(SerializedTransition);
					_toggle = false;
				}
			}
			position.x += rect.width / 2;
			if (GUI.Button(position, "Cancel"))
			{
				_toggle = false;
			}
		}

		void StatePropField(Rect pos, string label, SerializedProperty prop)
		{
			pos.height = singleLineHeight;
			LabelField(pos, label);
			pos.x += 40;
			pos.width /= 4;
			PropertyField(pos, prop, GUIContent.none);
		}
	}

    #endregion



    #region Override

    public void Dispose()
    {
	    UnityEngine.Object.DestroyImmediate(_transition.targetObject);
	    _transition.Dispose();
	    GC.SuppressFinalize(this);
    }

    #endregion



    #region Setup

    private static void SetupConditionsList(ReorderableList reorderList)
	{
		reorderList.elementHeight *= 2.3f;
		reorderList.drawHeaderCallback += rect => GUI.Label(rect, "Conditions");
		reorderList.onAddCallback += list =>
		{
			int count = list.count;
			list.serializedProperty.InsertArrayElementAtIndex(count);
			var prop = list.serializedProperty.GetArrayElementAtIndex(count);
			prop.FindPropertyRelative("ConditionSO").objectReferenceValue = null;
			prop.FindPropertyRelative("ExpectedResult").enumValueIndex = 0;
			prop.FindPropertyRelative("Operator").enumValueIndex = 0;
		};

		reorderList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			var prop = reorderList.serializedProperty.GetArrayElementAtIndex(index);
			rect = new Rect(rect.x, rect.y + 2.5f, rect.width, EditorGUIUtility.singleLineHeight);
			var condition = prop.FindPropertyRelative("ConditionSO");
			if (condition.objectReferenceValue != null)
			{
				string label = condition.objectReferenceValue.name;
				GUI.Label(rect, "If");
				GUI.Label(new Rect(rect.x + 20, rect.y, rect.width, rect.height), label, EditorStyles.boldLabel);
				EditorGUI.PropertyField(new Rect(rect.x + rect.width - 180, rect.y, 20, rect.height), condition, GUIContent.none);
			}
			else
			{
				EditorGUI.PropertyField(new Rect(rect.x, rect.y, 150, rect.height), condition, GUIContent.none);
			}
			EditorGUI.LabelField(new Rect(rect.x + rect.width - 120, rect.y, 20, rect.height), "Is");
			EditorGUI.PropertyField(new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height), prop.FindPropertyRelative("ExpectedResult"), GUIContent.none);
			EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y + EditorGUIUtility.singleLineHeight + 5, 60, rect.height), prop.FindPropertyRelative("Operator"), GUIContent.none);
		};

		reorderList.onChangedCallback += list => reorderList.serializedProperty.serializedObject.ApplyModifiedProperties();
		reorderList.drawElementBackgroundCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			if (isFocused)
				EditorGUI.DrawRect(rect, ContentStyle.Focused);

			if (index % 2 != 0)
				EditorGUI.DrawRect(rect, ContentStyle.ZebraDark);
			else
				EditorGUI.DrawRect(rect, ContentStyle.ZebraLight);
		};
	}

    #endregion
}
