
/// <summary>
/// # DontBeDestroyed
///   - 씬 전환이 일어나더라도 절대 삭제 되어선 안될 매니저들 모음
///   - Game, Resource 등 매니저들의 통합 클래스
///
/// # 사용 방법
///   - Managers Field : 실제 인스턴스들을 생성 및 보관
///   - Getter Properties : 인스턴스 호출
/// 
/// # No Standalone -> SingletonBehavior를 상속 받지 않는 기존 매니저들
/// # Standalone -> SingletonBehavior를 상속 받는 혼자서도 사용 가능한 싱글톤 매니저들
/// </summary>
public class Managers : SingletonBehavior<Managers>
{
    #region Managers Field
    
    private readonly Manager_Game _GameManager = new();
    private readonly Manager_Data _DataManager = new();
    //private readonly Manager_Resource _ResourceManager = new();
    
    #endregion



    #region Getter Properties

    /* No Standalone */
    public static Manager_Game Game => Instance._GameManager;
    public static Manager_Data Data => Instance._DataManager;
    //public static Manager_Resource Resource => Instance._ResourceManager;
    
    
    /* Standalone */
    public static CoroutineManager Coroutine => CoroutineManager.Instance;

    #endregion
}
