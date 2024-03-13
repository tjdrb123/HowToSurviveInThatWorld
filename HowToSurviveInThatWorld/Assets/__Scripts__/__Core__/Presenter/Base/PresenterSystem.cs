
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PresenterSystem : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<PresenterConfigSO> _ConfigTableSO;
    
    private List<Presenter<Component, Component>> _presenters;

    #endregion



    #region Unity Behavior

    private void Awake()
    {
        InitializeInternal();
        InitializePresenters();
    }

    private void OnEnable()
    {
        RegisterEventFromPresenters();
    }

    private void OnDisable()
    {
        UnregisterEventFromPresenters();
    }

    #endregion



    #region Management Presenter

    /// <summary>
    /// # Presenter Initialize
    ///   - 프레젠터를 생성하는 메서드
    /// </summary>
    private void InitializeInternal()
    {
        _presenters ??= new List<Presenter<Component, Component>>();

        foreach (var presenter in _ConfigTableSO.
                     Select(PresenterFactory.CreatePresenter).
                     Where(presenter => presenter != null))
        {
            _presenters.Add(presenter);
        }
    }

    /// <summary>
    /// # 프레젠터들을 초기화 하는 메서드
    /// </summary>
    private void InitializePresenters()
    {
        foreach (var presenter in _presenters)
        {
            presenter.Initialize();
        }
    }

    private void RegisterEventFromPresenters()
    {
        foreach (var presenter in _presenters)
        {
            presenter.RegisterEvents();
        }
    }
    
    private void UnregisterEventFromPresenters()
    {
        foreach (var presenter in _presenters)
        {
            presenter.UnregisterEvents();
        }
    }

    #endregion
}
