using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Manager_WarScene : Singleton<Manager_WarScene>
{
    public Canvas deathPopUp;
    public TextMeshProUGUI lifeTimeText;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerHungryBar;
    [HideInInspector] public bool playerDeathCheck;
    [HideInInspector] public float lifeTimer;
    public Player _player;
    

    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // 변경 or 삭제 예정
        
        lifeTimer = 0.0f;
        playerDeathCheck = true;
        deathPopUp.gameObject.SetActive(false);

        StartCoroutine(HungryTimer());
    }

    public void Update()
    {
        lifeTimer += Time.deltaTime;

        PlayerDeathSetting();
    }

    // 사망시 팝업창
    private void DeathPopUpWindow()
    {
        deathPopUp.gameObject.SetActive(true);
    }

    // 플레이어 사망 셋팅
    private void PlayerDeathSetting()
    {
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

    public void HealthProgressBar(GameObject playerObject)
    {
        Player player = playerObject.GetComponent<Player>();
        
        playerHealthBar.fillAmount = player.Health / player.MaxHealth;
    }

    public void HungryProgressBar()
    {
        _player.TakeHungry(-4f);

        playerHungryBar.fillAmount = _player.Hungry / _player.MaxHungry;
    }

    IEnumerator HungryTimer()
    {
        while (true)
        {
            HungryProgressBar();
            yield return new WaitForSeconds(1f);
        }
    }
}
