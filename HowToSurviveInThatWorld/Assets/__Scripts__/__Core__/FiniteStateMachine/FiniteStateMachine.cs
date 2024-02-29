
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
    private TransitionTableSO _transitionTableSO;
    
    private readonly Dictionary<Type, Component> _cachedComponents = new();
    [NonSerialized] public FiniteState _currentState;
    
#if UNITY_EDITOR
    [Space]
    [SerializeField]
    public FiniteStateMachineDebugger Debugger;
#endif

    #endregion



    #region Unity Behavior

    private void Awake()
    {
        _currentState = _transitionTableSO.GetInitialState(this);
#if UNITY_EDITOR
        Debugger.Awake(this);
#endif
    }

    private void Start()
    {
        _currentState.FiniteStateEnter();
    }
    
#if UNITY_EDITOR
    private void OnEnable()
    {
        UnityEditor.AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    private void OnAfterAssemblyReload()
    {
        _currentState = _transitionTableSO.GetInitialState(this);
        Debugger.Awake(this);
    }

    private void OnDisable()
    {
        UnityEditor.AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
    }
#endif

    private void Update()
    {
        if (_currentState.TryGetTransition(out var transitionState))
            Transition(transitionState);
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
