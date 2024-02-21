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
        playerController._hitColliders[0].gameObject.SetActive(false);
        _item.ItemPlus();
        Destroy(this);
    }

    public void StopInteract(PlayerController playerController, Animator animator)
    {
        playerController._hitColliders[0].gameObject.SetActive(true);
    }
}
