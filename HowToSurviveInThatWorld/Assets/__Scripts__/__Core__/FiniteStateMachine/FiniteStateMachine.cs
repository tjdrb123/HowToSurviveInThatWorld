
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class FiniteStateMachine : MonoBehaviour
{
    #region Fields

    [Tooltip("초기 상태를 '유한상태머신'에 셋팅")] [SerializeField]
    private TransitionTableSO _transitionTable;
    
    private readonly Dictionary<Type, Component> _cachedComponents = new();
    private FiniteState _currentState;

    #endregion



    #region Unity Behavior

    private void Awake()
    {
        
    }

    private void Start()
    {
        _currentState.FiniteStateEnter();
    }

    private void Update()
    {
        if (_currentState.TryGetTransition(out var transitionState))
        {
            Transition(transitionState);
        }
        
        _currentState.FiniteStateUpdate();
    }

    private void FixedUpdate()
    {
        _currentState.FiniteStateFixedUpdate();
    }

    #endregion



    #region Transition

    private void Transition(FiniteState transitionState)
    {
        _currentState.FiniteStateExit();
        _currentState = transitionState;
        _currentState.FiniteStateEnter();
    }

    #endregion



    #region Utils

    public new bool TryGetComponent<T>(out T component) where T : Component
    {
        var type = typeof(T);
        if (!_cachedComponents.TryGetValue(type, out var value))
        {
            if (base.TryGetComponent(out component))
            {
                _cachedComponents.Add(type, component);
            }

            return component != null;
        }

        component = value as T;
        return true;
    }

    public new T GetComponent<T>() where T : Component
    {
        return TryGetComponent(out T component)
            ? component
            : throw new InvalidOperationException($"{typeof(T).Name} not found in {name}");
    }

    #endregion
}
