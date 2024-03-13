
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Properties
    
    // 추가적 프로퍼티 고유 스트링 또는 정수형 값으로 ID가 필요함.
    // 해당 오브젝트를 식별할 수 있는 고유 아이디 키 값이 있으면 좋을 거 같음.
    
    // 모든 엔티티 관리를 위한 고유 인덱스
    public int UniqueIndex { get; private set; }
    
    // 바라보고 있는 방향에 대한 각도를 60분법으로 반환
    public float LookAngle => Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;

    #endregion
    
    
    
    #region Virtual Events
    
    // # 모든 자식들은 명시적으로 구현해야하는 메서드

    /// <summary>
    /// # OnEnable
    ///   - 객체가 활성화 될 때 이벤트를 구독 시키는 메서드
    ///   - OnEnable 같은 추가 이벤트함수 필요 없이 실행
    /// </summary>
    protected virtual void EntitySubscribeEvents() { }

    /// <summary>
    /// # OnDisable
    ///   - 객체가 비활성화 될 때 모든 이벤트를 해제 시키는 메서드
    ///   - 명시적으로 구현을 강요함으로써 이벤트가 존재할 때 구독을 반드시 해제해야한다.
    /// </summary>
    protected virtual void EntityDisposeEvents() { }

    #endregion



    #region Setup

    /// <summary>
    /// # Setup Unique Index (사용자 정의 인덱스)
    ///   - 고유 인덱스를 지정해주는 메서드
    ///   - 위 인덱스는 EntityManager가 관리하게 설계됌.
    /// </summary>
    public bool SetUniqueIndex(int index)
    {
        if (index is < 0 or > Literals.MAXIMUM_ENTITY_INDEX)
        {
            DebugLogger.LogWarning("Index must be between 0 and " + Literals.MAXIMUM_ENTITY_INDEX);
            return false;
        }

        UniqueIndex = index;
        return true;
    }

    #endregion



    #region Unity Behavior

    private void OnEnable()
    {
        EntitySubscribeEvents();
    }

    private void OnDisable()
    {
        EntityDisposeEvents();
    }

    #endregion



    #region Basic Rotate Methods

    /// <summary>
    /// # 타겟 포지션 방향으로 (rotate Speed)만큼 회전하는 메서드
    ///   - 스피드만큼 즉시가아닌 부드러운 회전을 원할 때 사용
    ///   - [필수] => 반드시 FixedUpdate()에서 실행할 것
    /// </summary>
    public void RotateSmoothByPosition(Vector3 targetPosition, float rotateSpeed, bool isYAxis = true)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        
        // 높이 차이를 무시하기 위함, 필요 하다면 조정 필요
        if (isYAxis) directionToTarget.y = Literals.ZERO_F;
        
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
    }

    /// <summary>
    /// # 타겟 포지션 방향으로 즉시 회전하는 메서드
    ///   - 점진적으로 증가하지 않기 때문에 즉각적일 때 사용
    /// </summary>
    public void RotateInstantByPosition(Vector3 targetPosition, bool isYAxis = true)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        
        // 높이 차이를 무시하기 위함, 필요 하다면 조정 필요
        if (isYAxis) directionToTarget.y = Literals.ZERO_F;
        
        transform.rotation = Quaternion.LookRotation(directionToTarget);
    }

    public void RotateSmoothByMovement(Vector3 movementVector, float turnSmoothTime, ref float turnSmoothSpeed, float rotateThreshold, bool isYAxis = true)
    {
        Vector3 horizontalVector = movementVector;
        if(isYAxis) horizontalVector.y = Literals.ZERO_F;

        // 한계치를 정해 놓음, 쓸 데 없는 회전 연산을 하지 않기 위함.
        if (!(horizontalVector.sqrMagnitude >= rotateThreshold)) return;
        
        float targetRotation = Mathf.Atan2(horizontalVector.x, horizontalVector.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y, targetRotation, ref turnSmoothSpeed, turnSmoothTime);
        transform.eulerAngles = Vector3.up * angle;
    }

    public void RotateInstantByMovement(Vector3 movementVector, float rotateThreshold, bool isYAxis = true)
    {
        Vector3 horizontalVector = movementVector;
        if(isYAxis) horizontalVector.y = Literals.ZERO_F;
        
        if (!(horizontalVector.sqrMagnitude >= rotateThreshold)) return;

        float targetRotation = Mathf.Atan2(horizontalVector.x, horizontalVector.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * targetRotation;
    }

    #endregion
}