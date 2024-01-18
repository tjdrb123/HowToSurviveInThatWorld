
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
    
    // 추 후 필요로 하면 작성

    #endregion



    #region Failed Callback

    // 추 후 필요로 하면 작성

    #endregion
}
