using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReopenDesButton : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(() =>
        {
            const string message = "You have:\n" +
                                   "10% chance to increase 1 rarity level\n" +
                                   "70% chance to maintain the same rarity\n" +
                                   "20% chance to decrease 1 rarity level";
            ErrorPopup.Instance.ShowError(message);
        });
    }
}