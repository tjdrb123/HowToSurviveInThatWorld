
using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Fields

    // Identifier
    protected string _name;
    protected string _tag;

    // Base Transform Info
    protected Transform _transform;
    protected GameObject _gameObject;
    private Vector3 _lookDirection;
    
    // Active Flag
    private bool _isActive;
    
    #endregion



    #region Properties

    /* Getter */
    // 현재 활성화 상태인지 비활성화 상태인지
    public bool IsActive => _isActive;
    // 바라보고 있는 방향에 대한 각도를 60분법으로 반환
    public float LookAngle => Mathf.Atan2(_lookDirection.z, _lookDirection.x) * Mathf.Rad2Deg;

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



    #region Unity Behavior

    private void Awake()
    {
        InitializeAwake();
    }

    private void Start()
    {
        InitializeStart();
    }

    protected void OnEnable()
    {
        EntitySubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        EntityDisposeEvents();
    }

    #endregion



    #region Initializer

    /// <summary>
    /// # Awake에서 실행 될 초기화 내용
    ///   - AddComponent와 같은 메서드로 추가 되어 바로 실행될 부분을 작성
    ///   - 해당 게임 오브젝트가 활성화 되자 마자 필요한 내용들이 초기화 되어야함
    ///   - ex) 주로 GetComponent, Caching
    /// </summary>
    /// <returns>초기화 성공 시 : True / 실패 시 : False</returns>
    protected virtual bool InitializeAwake()
    {
        try
        {
            _gameObject = gameObject;
        
            /* Setup */
            // Identifier
            _name = _gameObject.name;
            _tag = _gameObject.tag;
        
            // Transform
            _transform = _gameObject.transform;

            if (_transform == null)
            {
                DebugLogger.LogError($"Transform component missing {_name}");
                return false;
            }

            _lookDirection = _transform.forward;
        
            // Active State (Self)
            _isActive = _gameObject.activeSelf;

            return true;
        }
        catch (Exception exception)
        {
            DebugLogger.LogError("initializeAwake Failed : " + exception.Message);
            return false;
        }
    }

    /// <summary>
    /// # Start에서 실행 될 초기화 내용
    ///   - AddComponent 이 후 한 프레임 이후에 실행 될 초기화 내용
    ///   - ex) 주로 초기 이동(포지션, 회전) 셋팅, 스케일 등등
    /// </summary>
    /// <returns>초기화 성공 시 : True / 실패 시 : False</returns>
    protected virtual bool InitializeStart()
    {
        return true;
    }

    #endregion



    #region Active

    protected void SetActive(bool isActive)
    {
        _isActive = isActive;

        gameObject.SetActive(_isActive);
    }

    #endregion



    #region Rotate Methods

    /// <summary>
    /// # 타겟 포지션 방향으로 (rotate Speed)만큼 회전하는 메서드
    ///   - 스피드만큼 즉시가아닌 부드러운 회전을 원할 때 사용
    ///   - [필수] => 반드시 FixedUpdate()에서 실행할 것
    /// </summary>
    protected void RotateSmoothByPosition(Vector3 targetPosition, float rotateSpeed, bool isYAxis = true)
    {
        Vector3 directionToTarget = (targetPosition - _transform.position).normalized;
        
        // 높이 차이를 무시하기 위함, 필요 하다면 조정 필요
        if (isYAxis)
        {
            directionToTarget.y = Literals.ZERO_F;
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
    }

    /// <summary>
    /// # 타겟 포지션 방향으로 즉시 회전하는 메서드
    ///   - 점진적으로 증가하지 않기 때문에 즉각적일 때 사용
    /// </summary>
    protected void RotateInstantByPosition(Vector3 targetPosition, bool isYAxis = true)
    {
        Vector3 directionToTarget = (targetPosition - _transform.position).normalized;
        
        // 높이 차이를 무시하기 위함, 필요 하다면 조정 필요
        if (isYAxis)
        {
            directionToTarget.y = Literals.ZERO_F;
        }
        
        _transform.rotation = Quaternion.LookRotation(directionToTarget);
    }

    #endregion
}