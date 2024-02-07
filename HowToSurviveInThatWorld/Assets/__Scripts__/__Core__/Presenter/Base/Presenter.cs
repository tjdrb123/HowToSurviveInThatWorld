
using System;
using UnityEngine;

public class Presenter<TModel, TView> 
    where TModel : Component 
    where TView : Component
{
    #region Fields

    protected PresenterConfigSO _configSO;

    protected TModel _modelComponent;
    protected TView _viewComponent;

    #endregion



    #region Properties

    public TModel Model => _modelComponent;
    public TView View => _viewComponent;

    #endregion



    #region Constructor

    public Presenter(PresenterConfigSO configSO)
    {
        _configSO = configSO 
            ? configSO 
            : throw new NullReferenceException("Presenter Config SO is null");
    }

    #endregion



    #region Abstract

    public virtual void Initialize() { }
    public virtual void RegisterEvents() { }
    public virtual void UnregisterEvents() { }

    #endregion
}
