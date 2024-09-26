using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayUI : TruongSingleton<GamePlayUI>
{
    [SerializeField] private GameOverPanel gameOverPanel;
    public GameOverPanel GameOverPanel => this.gameOverPanel;
    [SerializeField] private GameObject loading;
    public GameObject Loading => this.loading;
}