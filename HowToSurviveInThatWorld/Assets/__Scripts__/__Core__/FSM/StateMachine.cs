/// <summary>
/// 현재 상태에 대한 관리, 상태 변경 등에 대한 처리입니다.
/// </summary>
public class StateMachine<T> where T : class
{
    # region Field
    
    private T        _ownerEntity;   // StateMachine의 소유주
    private State<T> _currentState;  // 현재 상태
    private State<T> _previousState; // 이전 상태
    private State<T> _globalState;   // 전역 상태
    
    #endregion

    public void Setup(T owner, State<T> entryState)
    {
        _ownerEntity  = owner;
        _currentState = null;
        _previousState = null;
        _globalState  = null;
        
        // entryState 상태로 상태 변경
        ChangeState(entryState);
    }
    
    public void ExecuteUpdate()
    {
        if (_globalState != null)
            _globalState.ExecuteUpdate(_ownerEntity);

        if (_currentState != null)
            _currentState.ExecuteUpdate(_ownerEntity);
    }
    
    public void ExecuteFixedUpdate()
    {
        if (_globalState != null)
            _globalState.ExecuteFixedUpdate(_ownerEntity);

        if (_currentState != null)
            _currentState.ExecuteFixedUpdate(_ownerEntity);
    }

    public void ChangeState(State<T> newState)
    {
        // 새로 바꾸려는 상태가 비어있으면 상태를 바꾸지 않는다
        if (newState == null)
            return;
        
        // 현재 재생중인 상태가 있으면 Exit() 메서드 호출
        if (_currentState != null)
        {
            // 상태가 변경되면 현재 상태는 이전 상태가 되기 때문에 previousState에 저장
            _previousState = _currentState;
            
            _currentState.Exit(_ownerEntity);
        }
        
        // 새로운 상태로 변경하고, 새로 바뀐 상태의 Enter() 메서드 호출
        _currentState = newState;
        _currentState.Enter(_ownerEntity);
    }

    public void SetGlobalState(State<T> newState)
    {
        _globalState = newState;
    }

    public void RevertToPreviousState()
    {
        ChangeState(_previousState);
    }
}
