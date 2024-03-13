using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapChanger : MonoBehaviour
{
    public GameObject enterCheckPanel;
    public Image panelImage;
    public TextMeshProUGUI panelText;
    public FadeInOut fadeInOut;
    
    public Sprite mapImage;
    public string mapNameTxt;

    private Image _image;
    public void OnButtonClick()
    {
        enterCheckPanel.SetActive(true);
        fadeInOut.enabled = true;

        panelImage.sprite = mapImage;
        panelText.text = mapNameTxt;
    }

    private void Start()
    {
        _image = gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (!enterCheckPanel.activeSelf)
        {
            _image.color = new Color(255, 255, 255, 255);
            fadeInOut.enabled = false;
        }
    }
}
