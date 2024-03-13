using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickUp : MonoBehaviour, IInteractableObject
{
    private item _item;
    private void Awake()
    {
        _item = GetComponent<item>();
    }
    public void Interact(PlayerController playerController, Animator animator)
    {
        _item.ItemPlus();
        animator.SetInteger("InteractingType", 0);
        animator.SetBool("IsInteracting", true);
    }

    public void StopInteract(PlayerController playerController, Animator animator)
    {
        animator.SetBool("IsInteracting", false);
        Destroy(gameObject);
    }
}
