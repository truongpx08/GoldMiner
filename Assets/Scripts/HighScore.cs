using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class HighScore : TruongSingleton<HighScore>
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    public void SetHighScore(float value)
    {
        this.highScoreText.text = value.ToString(CultureInfo.InvariantCulture);
    }
}