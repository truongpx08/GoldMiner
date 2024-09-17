using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MTamanBalance : TruongSingleton<MTamanBalance>
{
    [SerializeField] private TextMeshProUGUI tmp;

    public void SetText(float value)
    {
        this.tmp.text = value.ToString(CultureInfo.InvariantCulture);
    }
}