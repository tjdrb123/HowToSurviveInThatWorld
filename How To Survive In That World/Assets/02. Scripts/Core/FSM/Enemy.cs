using UnityEngine;

#region EnemyStates Enum

public enum E_EnemyStates
{
    // Enemy가 가지는 상태
}

#endregion

public class Enemy : BaseGameEntity
{
    # region Field
    
    private int       _health;
    private Locations _currentLocation;
    
    private State<Enemy>[]      _states;
    private StateMachine<Enemy> _stateMachine;
    
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

    #endregion
    
    public override void Setup(string name)
    {
        base.Setup(name);
        
        gameObject.name = $"{ID:D2}_Player_{name}";
        
        //_states                                   = new State<Enemy>[];
        //_states[(int)E_EnemyStates.] = new EnemyOwnedStates.();
        
        _stateMachine = new StateMachine<Enemy>();
        //_stateMachine.Setup();

        _health = 0;
        _currentLocation = Locations.Dungeon;
    }

    public override void Updated()
    {
        _stateMachine.Execute();
    }

    public void ChangeState(E_EnemyStates newState)
    {
        _stateMachine.ChangeState(_states[(int)newState]);
    }
}
