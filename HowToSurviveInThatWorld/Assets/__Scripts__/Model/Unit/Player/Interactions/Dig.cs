using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig : MonoBehaviour, IInteractableObject
{
    private int _DigCount = 0;
    [SerializeField]private EtcItem _etcItem;
    
    
    public void Interact(PlayerController playerController, Animator animator)
    {
        _DigCount++;
        animator.SetInteger("InteractingType", 2);
        animator.SetBool("IsInteracting", true);
    }

    public void StopInteract(PlayerController playerController, Animator animator)
    {
        if (_DigCount == _etcItem.PlusValue)
        {
            Manager_Inventory.Instance.Additem(_etcItem, 20); //랜덤 값을 넣어줘도 상관없음
            Destroy(gameObject);
            _DigCount = 0;
        }
        animator.SetBool("IsInteracting", false);
    }
}
