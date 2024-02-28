
using UnityEngine;

public interface IInteractableObject
{
    void Interact(PlayerController playerController, Animator animator);
    void StopInteract(PlayerController playerController, Animator animator);
}
