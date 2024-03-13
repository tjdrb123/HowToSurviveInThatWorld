
public readonly struct FiniteStateConditionST
{
    #region Fields

    public readonly FiniteStateMachine FiniteStateMachine;
    public readonly FiniteStateCondition FiniteStateCondition;
    public readonly bool ExpectedResult;

    #endregion



    #region Constructor & Utils

    public FiniteStateConditionST(
        FiniteStateMachine finiteStateMachine,
        FiniteStateCondition finiteStateCondition,
        bool expectedResult)
    {
        FiniteStateMachine = finiteStateMachine;
        FiniteStateCondition = finiteStateCondition;
        ExpectedResult = expectedResult;
    }

    public bool IsMet()
    {
        bool statement = FiniteStateCondition.GetStatement();
        bool isMet = statement == ExpectedResult;

#if UNITY_EDITOR
        FiniteStateMachine.Debugger.TransitionConditionResult(FiniteStateCondition.OriginSO.name, statement, isMet);
#endif
        
        return isMet;
    }

    #endregion
}
