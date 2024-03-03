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
        Manager_Sound.instance.AudioPlay(gameObject, "Sounds/SFX/Player/IsInteracting", false, false);
        animator.SetInteger("InteractingType", 1);
        animator.SetBool("IsInteracting", true);
    }
    public  void StopInteract(PlayerController playerController, Animator animator)
    {
        CoroutineManager.Instance.StopCrt(E_CoroutineKey.ChargeFillAmount);
        Manager_Sound.instance.AudioStop(gameObject);
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
        if (!gameObject.CompareTag("Produce"))
        {
            _chestData = GetComponent<ChestInventory>();
            _chestData.ChestData();
            _chestInventory = GameObject.Find("ChestCanvas(Clone)").GetComponent<UI_ChestInventory>();
            _chestInventory.Object = gameObject;
        }
    }
}
