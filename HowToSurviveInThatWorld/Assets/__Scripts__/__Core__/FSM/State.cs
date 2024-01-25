// 모든 상태가 상속받는 기반 클래스
public abstract class State<T> where T : class
{
    /// 해당 상태를 시작할 때 1회 호출 
    public abstract void Enter(T entity);
    
    // 해당 상태를 업데이트할 때, 매 프레임 호출
    public virtual void ExecuteUpdate(T entity)
    {
        // 기본적인 업데이트 로직
    }

    // 해당 상태를 업데이트할 때, 고정된 프레임률로 호출
    public virtual void ExecuteFixedUpdate(T entity)
    {
        // 물리 연산 관련 업데이트 로직
    }

    /// <summary>
    /// 해당 상태를 종료할 때 1회 호출 
    /// </summary>
    public abstract void Exit(T entity);
}
