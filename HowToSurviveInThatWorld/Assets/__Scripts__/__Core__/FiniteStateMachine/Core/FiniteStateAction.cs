
/// <summary>
/// # 실질적인 오브젝트에 행동(액션)을 나타는 클래스
/// </summary>
public abstract class FiniteStateAction : IFiniteStateComponent, IFiniteStateUpdater, IFiniteStateInitializer
{
    #region Fields

    public FiniteStateActionSO OriginSO { get; set; }

    public enum SpecificMoment
    {
        OnEnter,
        OnExit,
        OnUpdate,
        OnFixedUpdate
    }

    #endregion



    #region Only Use Override

    public virtual void FiniteStateEnter() { }
    public virtual void FiniteStateExit() { }
    public virtual void FiniteStateFixedUpdate() { }
    public abstract void FiniteStateUpdate();

    #endregion



    #region Initialize (Interface)

    /// <summary>
    /// # Initialize
    ///   - 새 인스턴스를 생성할 때 호출.
    ///   - 생성자와 같은 역할을 함. 필수 구성 요소를 캐싱 하는 역할
    /// </summary>
    /// <param name="finiteStateMachine"></param>
    public virtual void Initialize(FiniteStateMachine finiteStateMachine) { }

    #endregion
}
