
public interface IFiniteStateComponent
{
    /// <summary>
    /// # 스테이트로 들어갈 때 실행 될 메서드
    /// </summary>
    void FiniteStateEnter();
    
    /// <summary>
    /// # 스테이트가 종료(빠져나올 때)될 때 실행 될 메서드
    /// </summary>
    void FiniteStateExit();
}