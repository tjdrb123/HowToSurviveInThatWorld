using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager_WarScene : Singleton<Manager_WarScene>
{
    public Canvas deathPopUp;
    public TextMeshProUGUI lifeTimeText;
    [SerializeField] private Image playerHealthBar;
    [HideInInspector] public bool playerDeathCheck;
    [HideInInspector] public float lifeTimer;
    [HideInInspector] public float playerHP;
    

    public void Start()
    {
        lifeTimer = 0.0f;
        playerDeathCheck = true;
        deathPopUp.gameObject.SetActive(false);
    }

    public void Update()
    {
        lifeTimer += Time.deltaTime;
        
        if (!playerDeathCheck)
        {
            int hours = Mathf.FloorToInt(lifeTimer / 3600f);
            int minutes = Mathf.FloorToInt((lifeTimer % 3600) / 60);
            int seconds = Mathf.FloorToInt(lifeTimer % 60);
            
            lifeTimeText.text = $"Life Time : {hours}H {minutes}M {seconds}S";
            
            DeathPopUpWindow();
            playerDeathCheck = true;
        }
        
    }

    private void DeathPopUpWindow()
    {
        deathPopUp.gameObject.SetActive(true);
    }

    public void HealthProgressBar(GameObject playerObject)
    {
        Player player = playerObject.GetComponent<Player>();
        
        playerHealthBar.fillAmount = player.Health / player.MaxHealth;
    }
}
