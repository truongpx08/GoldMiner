using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Balance : TruongSingleton<Balance>
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    public void SetBalanceText(int value)
    {
        this.textMeshPro.text = TruongVirtual.FormatNumber(value);
    }
}