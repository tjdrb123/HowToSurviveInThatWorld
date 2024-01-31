
using System.Collections.Generic;
using UnityEngine;

public abstract class FiniteStateConditionSO : ScriptableObject
{
    #region Getter

    public FiniteStateConditionST GetCondition(
        FiniteStateMachine finiteStateMachine,
        bool expectedResult,
        Dictionary<ScriptableObject, object> createdInstance)
    {
        if (!createdInstance.TryGetValue(this, out var obj))
        {
            var stateCondition = CreateCondition();
            createdInstance.Add(this, stateCondition);
            stateCondition.OriginSO = this;
            stateCondition.Initialize(finiteStateMachine);

            obj = stateCondition;
        }

        return new FiniteStateConditionST(finiteStateMachine, (FiniteStateCondition)obj, expectedResult);
    }

    #endregion



    #region Abstract

    protected abstract FiniteStateCondition CreateCondition();

    #endregion
}

/// <summary>
/// # 제네릭 타입에 Condition Class
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class FiniteStateConditionSO<T> : FiniteStateConditionSO where T : FiniteStateCondition, new()
{
    protected override FiniteStateCondition CreateCondition() => new T();
}