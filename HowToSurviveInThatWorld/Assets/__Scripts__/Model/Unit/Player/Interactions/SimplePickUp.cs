using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickUp : MonoBehaviour, IInteractableObject
{
    public void Interact(PlayerController playerController, Animator animator)
    {
        playerController._hitColliders[0].gameObject.SetActive(false);
    }

    public void StopInteract(PlayerController playerController, Animator animator)
    {
        playerController._hitColliders[0].gameObject.SetActive(true);
    }
}
