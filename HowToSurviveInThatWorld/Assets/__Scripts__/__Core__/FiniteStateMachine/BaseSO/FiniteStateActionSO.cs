
using System.Collections.Generic;
using UnityEngine;

public abstract class FiniteStateActionSO : DescriptionActionBaseSO
{
    #region Getter

    /// <summary>
    /// # 새로운 사용자 정의를 생성 또는 반환 하는 메서드
    ///   - New (FiniteStateAction) 생성
    ///   - Return (createdInstance) 액션 반환
    /// </summary>
    public FiniteStateAction GetAction(FiniteStateMachine finiteStateMachine, Dictionary<ScriptableObject, object> createdInstance)
    {
        // 해당 딕셔너리에 액션이 존재할 경우 액션을 반환
        if(createdInstance.TryGetValue(this, out var obj)) return obj as FiniteStateAction;

        // 딕셔너리에 존재하지 않을 경우
        // 새로운 액션을 만들어 딕셔너리에 추가하고 반환
        var stateAction = CreateAction();
        createdInstance.Add(this, stateAction);
        stateAction.OriginSO = this;
        stateAction.Initialize(finiteStateMachine);

        return stateAction;
    }

    #endregion
    

    #region Abstract

    /// <summary>
    /// # 액션을 만들어 반환하는 추상 메서드
    /// </summary>
    /// <returns>유한 상태에 실질적인 액션을 반환</returns>
    protected abstract FiniteStateAction CreateAction();

    #endregion
    
}

/// <summary>
/// # 제네릭 타입에 Action Class
/// </summary>
public abstract class FiniteStateActionSO<T> : FiniteStateActionSO where T : FiniteStateAction, new()
{
    protected override FiniteStateAction CreateAction() => new T();
}