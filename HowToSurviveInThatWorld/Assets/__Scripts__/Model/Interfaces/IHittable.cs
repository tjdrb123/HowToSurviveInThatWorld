
/// <summary>
/// # 맞을 수 있는 모든 게임 오브젝트가 상속받을 인터페이스
///   - 현재는 비어 있지만 AttackerInfo같은 공통 구조체 하나가 필요해보임
/// </summary>
public interface IHittable
{
    void HitToUnit();
}