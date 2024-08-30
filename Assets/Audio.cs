using System;
using UnityEngine;
using UnityEngine.UI;

public class Audio : TruongUIButton
{
    [SerializeField] private bool isOn;
    [SerializeField] private Image image;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    private const string AudioKey = "Audio";

    protected override void Start()
    {
        this.isOn = PlayerPrefs.GetInt(AudioKey, 1) == 1;
        UpdateSpite();
        AddActionToButton(OnClickButton);
    }

    private void UpdateSpite()
    {
        this.image.sprite = isOn ? this.onSprite : this.offSprite;
    }

    private void OnClickButton()
    {
        this.isOn = !isOn;
        UpdateSpite();
        // save
        PlayerPrefs.SetInt(AudioKey, isOn ? 1 : 0);
        PlayerPrefs.Save(); // Lưu thay đổi  
    }
}