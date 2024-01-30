
public abstract class FiniteStateCondition : IFiniteStateComponent, IFiniteStateInitializer
{
    #region Fields

    private bool _isCached;
    private bool _isCachedStatement;
    
    public FiniteStateConditionSO OriginSO { get; set; }

    #endregion



    #region Process Cache

    /// <summary>
    /// # Abstract -> 상태 조건을 반환하는 메서드
    /// </summary>
    protected abstract bool Statement();

    public bool GetStatement()
    {
        if (!_isCached)
        {
            _isCached = true;
            _isCachedStatement = Statement();
        }

        return _isCachedStatement;
    }

    public void ClearStatementCache()
    {
        _isCached = false;
    }

    #endregion


    
    #region Only Use Override

    public virtual void FiniteStateEnter() { }
    public virtual void FiniteStateExit() { }
    public virtual void Initialize(FiniteStateMachine finiteStateMachine) { }

    #endregion
}
