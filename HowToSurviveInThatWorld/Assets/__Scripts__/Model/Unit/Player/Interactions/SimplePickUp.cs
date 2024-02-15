using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickUp : MonoBehaviour, IInteractableObject
{
    public void Interact(PlayerController playerController, Animator animator)
    {
        playerController._hitColliders[0].gameObject.SetActive(false);
        animator.SetFloat("InteractingType", 0);
        animator.SetBool("IsInteracting", true);
    }

    public void StopInteract(PlayerController playerController, Animator animator)
    {
        playerController._hitColliders[0].gameObject.SetActive(true);
        animator.SetBool("IsInteracting", false);
    }
}
