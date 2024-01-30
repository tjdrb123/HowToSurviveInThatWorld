
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TransitionTable", menuName = "State Machine/Transition Table")]
public class TransitionTableSO : ScriptableObject
{
    #region Struct & Enum

    [Serializable]
    public struct TransitionItem
    {
        public FiniteStateSO FromState;
        public FiniteStateSO ToState;
        public ConditionUsage[] Conditions;
    }

    [Serializable]
    public struct ConditionUsage
    {
        public Result ExpectedResult;
        public FiniteStateConditionSO ConditionSO;
        public Operator Operator;
    }

    public enum Result
    {
        True,
        False
    }
    
    public enum Operator
    {
        And,
        Or
    }

    #endregion



    #region Field

    [SerializeField] private TransitionItem[] _transitions;

    #endregion



    #region Getter

    public FiniteState GetInitialState(FiniteStateMachine finiteStateMachine)
    {
        var states = new List<FiniteState>();
        var transitions = new List<FiniteStateTransition>();
        var createdInstance = new Dictionary<ScriptableObject, object>();
        var fromStates = _transitions.GroupBy(transitions => transitions.FromState);

        foreach (var fromState in fromStates)
        {
            if (fromState.Key == null)
            {
                throw new ArgumentNullException(nameof(fromState.Key), $"TransitionTable : {name}");
            }

            var state = fromState.Key.GetState(finiteStateMachine, createdInstance);
            states.Add(state);
            
            transitions.Clear();
            foreach (var transitionItem in fromState)
            {
                if (transitionItem.ToState == null)
                {
                    throw new ArgumentNullException(nameof(transitionItem.ToState),
                        $"TransitionTable: {name}, From State: {fromState.Key.name}");
                }
                
                var toState = transitionItem.ToState.GetState(finiteStateMachine, createdInstance);
                ProcessConditionUsage(finiteStateMachine, transitionItem.Conditions, createdInstance,
                    out var conditions, out var resultGroup);
                transitions.Add(new FiniteStateTransition(toState, conditions, resultGroup));
            }

            state.StateTransitions = transitions.ToArray();
        }

        return (states.Count > 0)
            ? states[0]
            : throw new InvalidOperationException($"TransitionTable {name} is empty.");
    }

    #endregion



    #region Utils

    private static void ProcessConditionUsage(
        FiniteStateMachine finiteStateMachine,
        ConditionUsage[] conditionUsages,
        Dictionary<ScriptableObject, object> createdInstance,
        out FiniteStateConditionST[] conditions,
        out int[] resultGroup)
    {
        int count = conditionUsages.Length;
        conditions = new FiniteStateConditionST[count];
        for (int i = 0; i < count; ++i)
        {
            conditions[i] = conditionUsages[i].ConditionSO.GetCondition(
                finiteStateMachine, conditionUsages[i].ExpectedResult == Result.True, createdInstance);
        }

        List<int> resultGroupsList = new();
        for (int i = 0; i < count; ++i)
        {
            int idx = resultGroupsList.Count;
            resultGroupsList.Add(1);
            while (i < count - 1 && conditionUsages[i].Operator == Operator.And)
            {
                ++i;
                ++resultGroupsList[idx];
            }
        }

        resultGroup = resultGroupsList.ToArray();
    }

    #endregion
}
