
using System.Collections.Generic;

public class FiniteState : IFiniteStateComponent, IFiniteStateUpdater
{
    #region Fields

    public FiniteStateSO OriginSO;
    public FiniteStateMachine StateMachine;
    public FiniteStateTransition[] StateTransitions;
    public FiniteStateAction[] StateActions;

    #endregion



    #region Implements

    public void FiniteStateEnter()
    {
        ProcessStateEnter(StateTransitions);
        ProcessStateEnter(StateActions);
    }

    public void FiniteStateExit()
    {
        ProcessStateExit(StateTransitions);
        ProcessStateExit(StateActions);
    }

    public void FiniteStateUpdate()
    {
        ProcessStateUpdate();
    }

    public void FiniteStateFixedUpdate()
    {
        ProcessStateFixedUpdate();
    }

    #endregion



    #region Process

    private void ProcessStateEnter(IEnumerable<IFiniteStateComponent> stateComponents)
    {
        foreach (var stateComponent in stateComponents)
        {
            stateComponent.FiniteStateEnter();
        }
    }
    
    private void ProcessStateExit(IEnumerable<IFiniteStateComponent> stateComponents)
    {
        foreach (var stateComponent in stateComponents)
        {
            stateComponent.FiniteStateExit();
        }
    }

    private void ProcessStateUpdate()
    {
        foreach (var stateAction in StateActions)
        {
            stateAction.FiniteStateUpdate();
        }
    }

    private void ProcessStateFixedUpdate()
    {
        foreach (var stateAction in StateActions)
        {
            stateAction.FiniteStateFixedUpdate();
        }
    }

    #endregion



    #region Utils

    public bool TryGetTransition(out FiniteState finiteState)
    {
        finiteState = null;

        foreach (var transition in StateTransitions)
        {
            if (transition.TryGetTransition(out finiteState))
                break;
        }

        foreach (var transition in StateTransitions)
        {
            transition.ClearConditionsCache();
        }

        return finiteState != null;
    }

    #endregion
}
