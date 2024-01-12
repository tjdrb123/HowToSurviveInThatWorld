
/// <summary>
/// # DontBeDestroyed
///   - 씬 전환이 일어나더라도 절대 삭제 되어선 안될 매니저들 모음
///   - Game, Resource 등 매니저들의 통합 클래스
///
/// # 사용 방법
///   - Managers Field : 실제 인스턴스들을 생성 및 보관
///   - Getter Properties : 인스턴스 호출
/// </summary>
public class Managers : SingletonBehavior<Managers>
{
    #region Managers Field

    private readonly Manager_Game _GameManager = new();

    #endregion



    #region Getter Properties

    public static Manager_Game Game => Instance._GameManager;

    #endregion
}
