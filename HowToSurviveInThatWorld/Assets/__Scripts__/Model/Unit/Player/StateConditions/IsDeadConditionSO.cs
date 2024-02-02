
using UnityEngine;

[CreateAssetMenu(fileName = "IsDeadCondition", menuName = "State Machine/Conditions/Is Dead Condition")]
public class IsDeadConditionSO : FiniteStateConditionSO<IsDeadCondition>
{
    #region Property (Override)
    
    #endregion
}

public class IsDeadCondition : FiniteStateCondition
{
    #region Fields

    // Property (Origin SO)
    private new IsDeadConditionSO OriginSO => base.OriginSO as IsDeadConditionSO;

    #endregion
    

    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        
    }
    
    protected override bool Statement()
    {
        // 일단 플레이어 체력 데이터를 이렇게 받아왔습니다.
        Debug.Log(Managers.Data._playerData.Hp.CurValue < 0);
        return Managers.Data._playerData.Hp.CurValue < 0;
    }

    #endregion
}