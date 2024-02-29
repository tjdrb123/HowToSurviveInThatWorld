using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateInfo : MonoBehaviour
{
    private Animator _animator;
    private Player _player;
    private bool _deadCheck = true;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }
    
    void Update()
    {
        if (_player.Health <= 0 && _deadCheck)
        {
            _deadCheck = false;
            _animator.SetTrigger("IsDead");
            gameObject.layer = 0;
            DeathSetting();
        }
    }

    private void DeathSetting()
    {
        Component[] components = gameObject.GetComponents<Component>();

        foreach (var component in (IEnumerable)components)
        {
            if (component is FiniteStateMachine fsm)
            {
                fsm.enabled = false;
            }
            else if (component is PlayerController controller)
            {
                controller.enabled = false;
            }
            else if (component is Player player)
            {
                player.enabled = false;
            }
            else if (component is CharacterController characterController)
            {
                characterController.enabled = false;
            }
            else if (component is Collider boxCollider)
            {
                boxCollider.enabled = false;
            }
        }
    }
}
