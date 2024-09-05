using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EOptionButtonType
{
    Up,
    Down,
}

public class OptionButton : TruongUIButton
{
    [SerializeField] private EOptionButtonType type;
    [SerializeField] private GameObject selectedImage;
    [SerializeField] private Image bg;
    [SerializeField] private TextMeshProUGUI feeText;
    [SerializeField] private Image icon;

    protected override void Start()
    {
        base.Start();
        AddActionToButton(OnClickButton);
    }

    private void OnClickButton()
    {
        Select();
        Option.Instance.Buttons.ForEach(item =>
        {
            if (item != this)
                item.UnSelect();
        });
    }

    public void UnSelect()
    {
        this.selectedImage.SetActive(false);
    }

    public void Select()
    {
        this.selectedImage.SetActive(true);
    }

    public void Init(CrystalData crystalData)
    {
        if (crystalData.AVAILABLE)
        {
            this.bg.sprite =
                type == EOptionButtonType.Up
                    ? Option.Instance.OnUpButtonSprite
                    : Option.Instance.OnDownButtonSprite;
            this.icon.sprite = Option.Instance.OnTamanSprite;
        }
        else
        {
            this.bg.sprite =
                type == EOptionButtonType.Up
                    ? Option.Instance.OffUpButtonSprite
                    : Option.Instance.OffDownButtonSprite;
            this.icon.sprite = Option.Instance.OffTamanSprite;
        }

        this.feeText.text = TruongVirtual.FormatNumber(crystalData.FEE_INPUT);
    }
}