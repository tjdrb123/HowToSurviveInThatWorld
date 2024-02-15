using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Routing : MonoBehaviour, IInteractableObject
{
    public Image _chargingImg;
    public float _interactionTime = 10f;
    public void Interact(PlayerController playerController, Animator animator)
    {
        CoroutineManager.Instance.StartCrt(E_CoroutineKey.ChargeFillAmount, ChargeFillAmount(_chargingImg, _interactionTime, playerController));
        animator.SetFloat("InteractingType", 1);
        animator.SetBool("IsInteracting", true);
    }

    public  void StopInteract(PlayerController playerController, Animator animator)
    {
        CoroutineManager.Instance.StopCrt(E_CoroutineKey.ChargeFillAmount);
        _chargingImg.fillAmount = 0;
        animator.SetBool("IsInteracting", false);
    }

    IEnumerator ChargeFillAmount(Image chargingImg, float interactionTime, PlayerController playerController)
    {
        float fillAmount = 0;
        while (fillAmount < 1)
        {
            fillAmount += Time.deltaTime / interactionTime;
            chargingImg.fillAmount = fillAmount;
            yield return null;
        }
        playerController.IsInteracting = false;
    }
}
