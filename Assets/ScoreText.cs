using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : TruongSingleton<ScoreText>
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private int score;

    protected override void Start()
    {
        base.Start();
        this.score = 0;
        UpdateTextMesh();
    }

    private void UpdateTextMesh()
    {
        this.tmp.text = $"Score\n{score}";
    }

    public void IncreaseScore()
    {
        score++;
        UpdateTextMesh();
    }
}