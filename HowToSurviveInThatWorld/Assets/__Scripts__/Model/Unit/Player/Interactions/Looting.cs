using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Looting : MonoBehaviour, IInteractableObject
{
    public Image ChargingImg { get; set; }
    public float _interactionTime = 10f;
    [SerializeField] private GameObject Prefabs;
    private UI_ChestInventory _chestInventory;
    private ChestInventory _chestData;
    public void Interact(PlayerController playerController, Animator animator)
    {
        CoroutineManager.Instance.StartCrt(E_CoroutineKey.ChargeFillAmount, ChargeFillAmount(ChargingImg, _interactionTime, playerController));
        animator.SetInteger("InteractingType", 1);
        animator.SetBool("IsInteracting", true);
    }
    public  void StopInteract(PlayerController playerController, Animator animator)
    {
        CoroutineManager.Instance.StopCrt(E_CoroutineKey.ChargeFillAmount);
        ChargingImg.fillAmount = 0;
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
        _chestData = GetComponent<ChestInventory>();
        _chestData.ChestData();
        _chestInventory =  GameObject.Find("ChestCanvas(Clone)").GetComponent<UI_ChestInventory>();
        _chestInventory.Object = gameObject;
    }
}
