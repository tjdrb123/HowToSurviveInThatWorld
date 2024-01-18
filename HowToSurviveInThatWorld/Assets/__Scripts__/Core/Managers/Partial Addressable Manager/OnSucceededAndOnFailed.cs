
using System.Collections.Generic;

public partial class Manager_Addressable
{
    #region Succeeded Callback

    /// <summary>
    /// # 어드레서블 매니저 초기화 성공, 콜백 메서드
    /// </summary>
    private void OnSucceededByInit()
    {
        _isInitialize = true;
        DebugLogger.Log("Addressable Manager Initialize complete.");
    }
    
    /// <summary>
    /// # IResourceLocation
    ///   - 리소스 로케이션 로드 성공, 콜백 메서드
    /// </summary>
    /// <param name="key">Addressable Label</param>
    private void OnSucceededByLocation(object key)
    {
        _isLoadLocation = true;
        DebugLogger.Log($"Addressable : Location loaded complete. {key}");
    }

    private void OnSucceededByAssets<T>(string label, IList<T> list)
    {
        
    }

    #endregion



    #region Failed Callback

    

    #endregion
}
