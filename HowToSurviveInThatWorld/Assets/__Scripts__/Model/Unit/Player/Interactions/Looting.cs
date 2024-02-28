using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Looting : MonoBehaviour, IInteractableObject
{
    public Image _chargingImg;
    public float _interactionTime = 10f;
    [SerializeField] private GameObject Prefabs;
    private UI_ChestInventory _chestInventory;
    public void Interact(PlayerController playerController, Animator animator)
    {
        CoroutineManager.Instance.StartCrt(E_CoroutineKey.ChargeFillAmount, ChargeFillAmount(_chargingImg, _interactionTime, playerController));
        animator.SetInteger("InteractingType", 1);
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
        Instantiate(Prefabs);
        _chestInventory =  GameObject.Find("ChestCanvas(Clone)").GetComponent<UI_ChestInventory>();
        _chestInventory.Object = gameObject;
    }
}
