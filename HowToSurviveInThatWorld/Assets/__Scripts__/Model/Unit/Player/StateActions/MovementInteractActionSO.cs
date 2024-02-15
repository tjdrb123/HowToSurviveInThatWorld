
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MovementInteractAction", menuName = "State Machine/Actions/Movement Interact Action")]
public class MovementInteractActionSO : FiniteStateActionSO
{
    #region Field

    public float _InteractionTime = 50f;

    #endregion


    #region Property (Override)

    [SerializeField] private FiniteStateAction.SpecificMoment _moment;
    
    // Properties
    
    public FiniteStateAction.SpecificMoment Moment => _moment;
    protected override FiniteStateAction CreateAction() => new MovementInteractAction();
    
    #endregion
}

public class MovementInteractAction : FiniteStateAction
{
    #region Fields
    private PlayerController _playerController;
    private Animator _animator;

    // Property (Origin SO)
    private new MovementInteractActionSO OriginSO => base.OriginSO as MovementInteractActionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
        _animator = finiteStateMachine.GetComponent<Animator>();
    }
    
    public override void FiniteStateEnter()
    {
        if (OriginSO.Moment == SpecificMoment.OnEnter)
        {
            if(_playerController._hitColliders[0].tag == "SimplePickUp")
            {
                _playerController._hitColliders[0].gameObject.SetActive(false);
                _animator.SetFloat("InteractingType", 0);
                _animator.SetBool("IsInteracting", true);
            }

            if (_playerController._hitColliders[0].tag == "Routing")
            {
                CoroutineManager.Instance.StartCrt(E_CoroutineKey.ChargeFillAmount, ChargeFillAmount(_playerController._chargingImg, 10f));
                _animator.SetFloat("InteractingType", 1);
                _animator.SetBool("IsInteracting", true);
            }
        }
    }

    IEnumerator ChargeFillAmount(Image chargingImg, float interactionTime)
    {
        float fillAmount = 0;
        while(fillAmount < 1)
        {
            fillAmount += Time.deltaTime / interactionTime;
            chargingImg.fillAmount = fillAmount;
            yield return null;
        }
        _playerController.IsInteracting = false;
    }


    public override void FiniteStateExit()
    {
        if (_playerController._hitColliders[0].tag == "SimplePickUp")
        {
            _playerController._hitColliders[0].gameObject.SetActive(true);
            _animator.SetBool("IsInteracting", false);
        }

        if (_playerController._hitColliders[0].tag == "Routing")
        {
            CoroutineManager.Instance.StopCrt(E_CoroutineKey.ChargeFillAmount);
            _playerController._chargingImg.fillAmount = 0;
            _animator.SetBool("IsInteracting", false);
        }
    }
    
    public override void FiniteStateUpdate() 
    { 
        // None
    }
    
    public override void FiniteStateFixedUpdate()
    {
        
    }

    #endregion
}