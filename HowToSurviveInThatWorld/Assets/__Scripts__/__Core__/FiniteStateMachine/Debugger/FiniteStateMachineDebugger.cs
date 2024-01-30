
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class FiniteStateMachineDebugger
{
    #region Fields

    [SerializeField] [Tooltip("디버그 로그 토글, 상태 전환 트리거")]
    private bool _DebugTransition;

    [SerializeField] [Tooltip("전체 리스트 (컨디션), ConditionName == BooleanResult [PassedTest]")]
    private bool _AppendConditionsInfo = true;

    [SerializeField] [Tooltip("전체 리스트 (액션), 활성화된 액션 by 새로운 상태")]
    private bool _AppendActionsInfo = true;

    [SerializeField] [Tooltip("현재 스테이트 이름 [Readonly]")]
    private string _CurrentStateName;

    private FiniteStateMachine _finiteStateMachine;
    private StringBuilder _logBuilder;
    private string _targetState = string.Empty;
    
    private const string CHECK_MARK = "\u2714";
    private const string UNCHECK_MARK = "\u2718";
    private const string THICK_ARROW = "\u279C";
    private const string SHARP_ARROW = "\u27A4";

    #endregion



    #region Process (Transition & Conditions)

    public void Awake(FiniteStateMachine finiteStateMachine)
    {
        _finiteStateMachine = finiteStateMachine;
        _logBuilder = new StringBuilder();

        _CurrentStateName = _finiteStateMachine._currentState.OriginSO.name;
    }

    public void TransitionEvaluationBegin(string targetState)
    {
        _targetState = targetState;

        if (!_DebugTransition) return;

        _logBuilder.Clear();
        _logBuilder.AppendLine($"{_finiteStateMachine.name} state changed");
        _logBuilder.AppendLine($"{_CurrentStateName}   {SHARP_ARROW}    {_targetState}");

        if (_AppendConditionsInfo)
        {
            _logBuilder.AppendLine();
            _logBuilder.AppendLine("Transition Conditions");
        }
    }

    public void TransitionConditionResult(string conditionName, bool result, bool isMet)
    {
        if (!_DebugTransition || _logBuilder.Length == (int)Literals.ZERO_F || !_AppendConditionsInfo) return;

        _logBuilder.Append($"     {THICK_ARROW} {conditionName} == {result}");

        _logBuilder.AppendLine(isMet ? $"  [{CHECK_MARK}]" : $"  [{UNCHECK_MARK}]");
    }

    public void TransitionEvaluationEnd(bool passed, FiniteStateAction[] actions)
    {
        if (passed)
        {
            _CurrentStateName = _targetState;
        }

        if (!_DebugTransition || _logBuilder.Length == (int)Literals.ZERO_F) return;

        if (passed)
        {
            LogActions(actions);
            PrintDebugLog();
        }

        _logBuilder.Clear();
    }

    #endregion



    #region Process (Actions)

    private void LogActions(IEnumerable<FiniteStateAction> actions)
    {
        if (!_AppendActionsInfo)
            return;

        _logBuilder.AppendLine();
        _logBuilder.AppendLine("State Actions:");

        foreach (FiniteStateAction action in actions)
        {
            _logBuilder.AppendLine($"    {THICK_ARROW} {action.OriginSO.name}");
        }
    }
    
    private void PrintDebugLog()
    {
        _logBuilder.AppendLine();
        _logBuilder.Append("--------------------------------");

        DebugLogger.Log(_logBuilder.ToString());
    }

    #endregion
}
