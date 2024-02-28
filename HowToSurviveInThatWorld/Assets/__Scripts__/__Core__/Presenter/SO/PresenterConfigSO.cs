
using UnityEngine;

[CreateAssetMenu(fileName = "PresenterConfig", menuName = "Presenter/Config")]
public class PresenterConfigSO : ScriptableObject
{
    #region Fields
    
    public GameObject Model;
    public GameObject View;
    public System.Type Type;

    #endregion
}