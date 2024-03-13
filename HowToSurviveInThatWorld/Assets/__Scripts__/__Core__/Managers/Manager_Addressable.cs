
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

public partial class Manager_Addressable
{
    #region Fields

    /* Location */
    // Key : Label , Value : Addressable Names
    private readonly Dictionary<string, List<IResourceLocation>> _locations;
    private readonly List<IResourceLocation> _noLocation = new(); // _location이 없을 때 반환될 기본 값
    
    /* Assets (실제 자산) */
    private readonly Dictionary<string, object> _assets;
    
    /* Addressable Primary Keys */
    private readonly List<object> _keys;
    
    /* Init Addressable Init Flag */
    // Default = false
    private bool _isInitialize;

    #endregion



    #region Properties

    // Addressable Primary Keys Getter
    public IReadOnlyList<object> Keys => _keys;

    public bool IsInitialize => _isInitialize;

    #endregion



    #region Constructor

    public Manager_Addressable()
    {
        _locations = new Dictionary<string, List<IResourceLocation>>();
        _noLocation = new List<IResourceLocation>();

        _assets = new Dictionary<string, object>();

        _keys = new List<object>();
    }

    #endregion
}
