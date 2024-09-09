using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private OpenButton openButton;
    [SerializeField] private SellButton sellButton;
    [SerializeField] private Transform reward;
    [SerializeField] private Transform point;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject homeButton;

    public void ShowReward()
    {
        this.openButton.gameObject.SetActive(true);
        this.sellButton.gameObject.SetActive(true);
        this.reward.gameObject.SetActive(true);
        this.point.gameObject.SetActive(false);
        this.homeButton.SetActive(false);
    }

    public void ShowPoint()
    {
        this.openButton.gameObject.SetActive(false);
        this.sellButton.gameObject.SetActive(false);
        this.reward.gameObject.SetActive(false);
        this.point.gameObject.SetActive(true);
        this.homeButton.SetActive(true);
    }

    public void SetRewardText()
    {
        this.rewardText.text = ScoreText.Instance.Score.ToString();
    }
}