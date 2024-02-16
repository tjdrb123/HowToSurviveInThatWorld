
using UnityEngine.AddressableAssets;

public class GameSceneSO : DescriptionBaseSO
{
    #region Fields
    
    /* Enum */
    public enum GameSceneType
    {
        // Playable Scene
        Location,
        Title,
        
        // Special Scene
        Initialize,
        PersistentManagers,
        GamePlay
    }

    /* Member */
    public GameSceneType SceneType;
    public AssetReference SceneReference;

    #endregion
}