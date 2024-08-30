using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : TruongUIButton
{
    [SerializeField] private GameObject selectedImage;

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
}