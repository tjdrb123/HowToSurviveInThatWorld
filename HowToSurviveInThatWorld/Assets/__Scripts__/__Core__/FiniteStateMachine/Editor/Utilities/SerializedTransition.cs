
using UnityEditor;

public readonly struct SerializedTransition
{
    #region Fields

    public readonly SerializedProperty Transition;
    public readonly SerializedProperty FromState;
    public readonly SerializedProperty ToState;
    public readonly SerializedProperty Conditions;
    public readonly int Index;

    #endregion



    #region Constructor & Utils

    public SerializedTransition(SerializedProperty transition)
    {
        Transition = transition;
        FromState = Transition.FindPropertyRelative("FromState");
        ToState = Transition.FindPropertyRelative("ToState");
        Conditions = Transition.FindPropertyRelative("Conditions");
        Index = -1;
    }

    public SerializedTransition(SerializedProperty transition, int index)
    {
        Transition = transition.GetArrayElementAtIndex(index);
        FromState = Transition.FindPropertyRelative("FromState");
        ToState = Transition.FindPropertyRelative("ToState");
        Conditions = Transition.FindPropertyRelative("Conditions");
        Index = index;
    }
    
    /// <summary>
    /// # Utility
    /// </summary>
    internal void ClearProperties()
    {
        FromState.objectReferenceValue = null;
        ToState.objectReferenceValue = null;
        Conditions.ClearArray();
    }

    #endregion
}