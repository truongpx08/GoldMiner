using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : TruongSingleton<Option>
{
    [SerializeField] private List<OptionButton> buttons;
    public List<OptionButton> Buttons => this.buttons;

    // Up
    [SerializeField] private Sprite onUpButtonSprite;
    public Sprite OnUpButtonSprite => this.onUpButtonSprite;
    [SerializeField] private Sprite offUpButtonSprite;
    public Sprite OffUpButtonSprite => this.offUpButtonSprite;
    // Down
    [SerializeField] private Sprite onDownButtonSprite;
    public Sprite OnDownButtonSprite => this.onDownButtonSprite;
    [SerializeField] private Sprite offDownButtonSprite;
    public Sprite OffDownButtonSprite => this.offDownButtonSprite;
    [SerializeField] private Sprite onTamanSprite;
    public Sprite OnTamanSprite => this.onTamanSprite;
    [SerializeField] private Sprite offTamanSprite;
    [SerializeField] private OptionButton currentOption;
    public OptionButton CurrentOption => this.currentOption;
    public Sprite OffTamanSprite => this.offTamanSprite;


    public void InitAllButton(List<CrystalData> data)
    {
        int index = 0;
        foreach (var button in buttons)
        {
            button.Init(data[index]);
            index++;
        }

        SelectFirstButton();
    }

    public void SetCurrentOption(OptionButton optionButton)
    {
        this.currentOption = optionButton;
    }

    private void SelectFirstButton()
    {
        buttons.ForEach(item =>
        {
            if (item == buttons[0])
                item.Select();
            else
                item.UnSelect();
        });
    }
}