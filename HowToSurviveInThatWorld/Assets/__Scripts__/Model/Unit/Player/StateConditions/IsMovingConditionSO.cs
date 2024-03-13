﻿
using UnityEngine;

[CreateAssetMenu(fileName = "IsMovingCondition", menuName = "State Machine/Conditions/Is Moving Condition")]
public class IsMovingConditionSO : FiniteStateConditionSO<IsMovingCondition>
{
    #region Field

    public float MovementInputThreshold = 0.02f;

    #endregion
}

public class IsMovingCondition : FiniteStateCondition
{
    #region Fields

    private PlayerController _playerController;
    
    // Property (Origin SO)
    private new IsMovingConditionSO OriginSO => base.OriginSO as IsMovingConditionSO;

    #endregion



    #region Override

    public override void Initialize(FiniteStateMachine finiteStateMachine)
    {
        _playerController = finiteStateMachine.GetComponent<PlayerController>();
    }
    
    protected override bool Statement()
    {
        Vector3 movementVector = _playerController.MovementInput;
        movementVector.y = Literals.ZERO_F;
        return movementVector.sqrMagnitude > OriginSO.MovementInputThreshold;
    }

    #endregion
}