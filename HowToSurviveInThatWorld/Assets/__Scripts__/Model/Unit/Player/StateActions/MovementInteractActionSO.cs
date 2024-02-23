
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
            var interactableObject = _playerController._hitColliders[0].GetComponent<IInteractableObject>();
            if (interactableObject != null)
                interactableObject.Interact(_playerController, _animator);
            else
                _playerController.IsInteracting = false;
        }
    }

    public override void FiniteStateExit()
    {
        var interactableObject = _playerController._hitColliders[0].GetComponent<IInteractableObject>();
        if (interactableObject != null)
            interactableObject.StopInteract(_playerController, _animator);
    }

    public override void FiniteStateUpdate()
    {
        
    }

    public override void FiniteStateFixedUpdate()
    {

    }

    #endregion
}