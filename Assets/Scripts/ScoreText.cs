using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : TruongSingleton<ScoreText>
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private int score;
    public int Score => this.score;


    protected override void Start()
    {
        base.Start();
        SetScore(0);
    }

    public void SetScore(int value)
    {
        this.score = value;
        UpdateTextMesh();
    }

    private void UpdateTextMesh()
    {
        this.tmp.text = $"Score\n{score}";
    }
}