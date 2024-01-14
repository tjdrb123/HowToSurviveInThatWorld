using UnityEngine;

#region PlayerStates Enum

public enum E_PlayerStates
{
    RestAndSleep = 0,
    FightHard,
    //Global ( 이미지 참고 )
}

#endregion

public class Player : BaseGameEntity
{
    #region Field
    
    private int _health;
    private Locations _currentLocation;

    // Player가 가지고 있는 모든 상태, 현재 상태
    private State<Player>[] _states;
    private StateMachine<Player> _stateMachine;
    
    #endregion

    #region Properties

    public int Health
    {
        get => _health;
        set => _health = Mathf.Max(0, value);
    }

    public Locations CurrentLocation
    {
        get => _currentLocation;
        set => _currentLocation = value;
    }
    
    public E_PlayerStates CurrentState // 현재 상태
    {
        get;
        private set;
    }

    #endregion
    

    public override void Setup(string name)
    {
        base.Setup(name);
        
        // 생성되는 오브젝트 이름 설정
        gameObject.name = $"{ID:D2}_Player_{name}";
        
        // Player가 가질 수 없는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
        _states                                   = new State<Player>[5];
        _states[(int)E_PlayerStates.RestAndSleep] = new PlayerOwnedStates.RestAndSleep();
        _states[(int)E_PlayerStates.FightHard]    = new PlayerOwnedStates.FightHard();
        //_states[(int)E_PlayerStates.Global]       = new PlayerOwnedStates.StateGlobal ( 이미지 참고 )
        
        // 상태를 관리하는 StateMachine에 메모리를 할당하고, 첫 상태를 설정
        _stateMachine = new StateMachine<Player>();
        _stateMachine.Setup(this, _states[(int)E_PlayerStates.RestAndSleep]);
        
        // 전역 상태 설정 ( 이미지 참고 )
        //_stateMachine.SetGlobalState(_states[(int)E_PlayerStates.Global]);
        
        
        _health = 0;
        _currentLocation = Locations.Viliage;
        // 위에처럼 나머지 필드 데이터 셋업
        /*

        */
    }

    public override void Updated()
    {
        _stateMachine.Execute();
    }

    public void ChangeState(E_PlayerStates newState)
    {
        CurrentState = newState; // 상태가 바뀌면 새로운 상태가 현재상태로
        
        _stateMachine.ChangeState(_states[(int)newState]);
    }
    
    public void RevertToPreviousState()
    {
        _stateMachine.RevertToPreviousState();
    }
}
