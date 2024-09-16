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
    [SerializeField] private CrystalData data;
    public CrystalData Data => this.data;

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
        Option.Instance.SetCurrentOption(this);
        DataManager.Instance.SetLevel(this.data.level);
        OptionDescriptionText.Instance.SetText(this.data.level);
    }

    public void Init(CrystalData crystalData)
    {
        this.data = crystalData;
        if (this.data.AVAILABLE)
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

        this.feeText.text = TruongVirtual.FormatNumber(this.data.FEE_INPUT);
    }
}