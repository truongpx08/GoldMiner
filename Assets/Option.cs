using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : TruongSingleton<Option>
{
    [SerializeField] private List<OptionButton> buttons;
    public List<OptionButton> Buttons => this.buttons;

    protected override void Start()
    {
        base.Start();
        buttons.ForEach(item =>
        {
            if (item == buttons[0])
                item.Select();
            else
                item.UnSelect();
        });
    }
}