using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Bio
{
    [SerializeField] private InputReader _input;
    public bool IsAttack;
    public Vector2 Direction;

    protected override void EntitySubscribeEvents()
    {
        _input.OnMoveEvent += Movement;
        _input.OnAttackEvent += Attack;
        _input.OnAttackCanceledEvent += CanceledAttack;
    }

    private void Movement(Vector2 moveDirection)
    {
        Direction = moveDirection;
    }

    private void Attack()
    {
        IsAttack = true;
    }

    private void CanceledAttack()
    {
        IsAttack = false;
    }
}
