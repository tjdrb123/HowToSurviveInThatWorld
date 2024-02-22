using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig : MonoBehaviour, IInteractableObject
{
    public void Interact(PlayerController playerController, Animator animator)
    {
        playerController._hitColliders[0].gameObject.SetActive(false);
        animator.SetInteger("InteractingType", 2);
        animator.SetBool("IsInteracting", true);
    }

    public void StopInteract(PlayerController playerController, Animator animator)
    {
        playerController._hitColliders[0].gameObject.SetActive(true);
        animator.SetBool("IsInteracting", false);
    }
}
