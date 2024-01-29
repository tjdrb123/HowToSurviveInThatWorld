
public class FiniteStateTransition : IFiniteStateComponent
{
    #region Fields

    private FiniteState _targetState;
    private FiniteStateConditionST[] _stateConditions;
    private int[] _resultGroups;
    private bool[] _results;

    #endregion



    #region Constructor

    public FiniteStateTransition(FiniteState targetState, FiniteStateConditionST[] stateConditions, int[] resultGroups = null)
    {
        _targetState = targetState;
        _stateConditions = stateConditions;
        _resultGroups = resultGroups is { Length: > 0 } ? resultGroups : new int[1];
        _results = new bool[_resultGroups.Length];
    }

    #endregion
    


    #region Transition

    public bool TryGetTransition(out FiniteState state)
    {
        state = ShouldTransition() ? _targetState : null;
        
        return state != null;
    }

    private bool ShouldTransition()
    {
        int count = _resultGroups.Length;
        for (int i = 0, idx = 0; i < count && idx < _stateConditions.Length; ++i)
        {
            for (int j = 0; j < _resultGroups[i]; ++j, ++idx)
            {
                _results[i] = (j == 0)
                    ? _stateConditions[idx].IsMet()
                    : _results[i] && _stateConditions[idx].IsMet();
            }
        }

        bool result = false;
        for (int i = 0; i < count && !result; ++i)
        {
            result = result || _results[i];
        }

        return result;
    }

    public void ClearConditionsCache()
    {
        for (int i = 0; i < _stateConditions.Length; ++i)
        {
            _stateConditions[i].FiniteStateCondition.ClearStatementCache();
        }
    }

    #endregion



    #region Implement Interface

    public void FiniteStateEnter()
    {
        for (int i = 0; i < _stateConditions.Length; ++i)
        {
            _stateConditions[i].FiniteStateCondition.FiniteStateEnter();
        }
    }

    public void FiniteStateExit()
    {
        for (int i = 0; i < _stateConditions.Length; ++i)
        {
            _stateConditions[i].FiniteStateCondition.FiniteStateExit();
        }
    }

    #endregion
}
