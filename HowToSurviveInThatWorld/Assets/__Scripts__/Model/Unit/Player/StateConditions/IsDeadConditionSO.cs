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
        //return Managers.Data._playerData.Hp.CurValue < 0;
        return false;
    }

    #endregion
}