
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "State Machine/State")]
public class FiniteStateSO : ScriptableObject
{
    #region Field

    [SerializeField] private FiniteStateActionSO[] _StateActions;

    #endregion



    #region Getter

    public FiniteState GetState(FiniteStateMachine finiteStateMachine, Dictionary<ScriptableObject, object> createdInstance)
    {
        if (createdInstance.TryGetValue(this, out var obj)) return obj as FiniteState;

        var finiteState = new FiniteState();
        createdInstance.Add(this, finiteState);

        finiteState.OriginSO = this;
        finiteState.StateMachine = finiteStateMachine;
        finiteState.StateTransitions = Array.Empty<FiniteStateTransition>();
        finiteState.StateActions = GetActions(_StateActions, finiteStateMachine, createdInstance);

        return finiteState;
    }

    private static FiniteStateAction[] GetActions(
        FiniteStateActionSO[] scriptableActions,
        FiniteStateMachine finiteStateMachine,
        Dictionary<ScriptableObject, object> createdInstance)
    {
        int count = scriptableActions.Length;
        var actions = new FiniteStateAction[count];
        for (int i = 0; i < count; ++i)
        {
            actions[i] = scriptableActions[i].GetAction(finiteStateMachine, createdInstance);
        }

        return actions;
    }

    #endregion
}
