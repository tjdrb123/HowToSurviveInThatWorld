using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Manager_WarScene : Singleton<Manager_WarScene>
{
    public Canvas deathPopUp;
    public TextMeshProUGUI lifeTimeText;
    public TextMeshProUGUI killCountText;
    
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerHungryBar;
    [SerializeField] private GameObject playerHitPanel;
    
    [HideInInspector] public bool playerDeathCheck;
    [HideInInspector] public float lifeTimer;
    [HideInInspector] public int killCount = 0;
    [HideInInspector] public Player _player;
    

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
            killCountText.text = $"Kill Count : {killCount}";
            
            DeathPopUpWindow();
            playerDeathCheck = true;
        }
    }

    public void HealthProgressBar(GameObject playerObject)
    {
        Player player = playerObject.GetComponent<Player>();
        playerHealthBar.fillAmount = player.Health / player.MaxHealth;
        StopCoroutine(PlayerHitPanel());
        StartCoroutine(PlayerHitPanel());
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

    IEnumerator PlayerHitPanel()
    {
        playerHitPanel.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        playerHitPanel.SetActive(false);
    }
}
