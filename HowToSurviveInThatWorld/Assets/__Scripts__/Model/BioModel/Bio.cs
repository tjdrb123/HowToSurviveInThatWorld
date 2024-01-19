using UnityEngine;

// Hit 메서드 강제 (인터페이스)
public interface IHit
{
    public void Hit(float myCurrentHealth, float opponentAttackDamage);
}
public class Bio : Entity
{
    #region Field Variables
    
    // (필수사항) 체력, 이동속도,
    // (고려사항) 공격력, EXP, Vector3, ##상호작용##
    protected float _health;
    protected float _moveSpeed;
    
    // (필수사항) 애니메이터, 콜라이더, 리지드바디
    protected Animator _animator;
    protected Collider _collider;
    protected Rigidbody _rigidbody;
    
    #endregion

    
    
    #region Base Initialize
    
    protected override bool InitializeAwake()
    {
        base.InitializeAwake();
        return CreatSetupComponent(_gameObject);
    }
    
    #endregion

    
    
    #region Bio Component Setup
    
    /// <summary>
    /// CreatSetupComponent 메소드에 자신의 gameObject를 넣어서 Component들 할당.
    /// override하여 더 필요한 Component들을 추가가능.
    /// </summary>
    protected virtual bool CreatSetupComponent(GameObject gameObject)
    {
        if (gameObject == null)
        {
            DebugLogger.LogError($"GameObject missing {_name}");
            return false;
        }
        
        gameObject = this.gameObject;
        _animator = gameObject.GetComponent<Animator>();
        _collider = gameObject.GetComponent<Collider>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        
        return true;
    }
    
    #endregion
    
}
