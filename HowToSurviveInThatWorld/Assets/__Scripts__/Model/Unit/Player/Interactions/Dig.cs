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
        playerController._hitColliders[0].gameObject.SetActive(false);
        animator.SetInteger("InteractingType", 2);
        animator.SetBool("IsInteracting", true);
    }

    public void StopInteract(PlayerController playerController, Animator animator)
    {
        if (_DigCount == _etcItem.PlusValue)
        {
            Manager_Inventory.Instance.Additem(_etcItem, 4);
            Destroy(gameObject);
            _DigCount = 0;
        }
        playerController._hitColliders[0].gameObject.SetActive(true);
        animator.SetBool("IsInteracting", false);
    }
}
