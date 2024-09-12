using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public enum EFinishType
{
    None,
    buyAll,
    gacha,
}

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private OpenButton openButton;
    [SerializeField] private SellButton sellButton;
    [SerializeField] private Transform reward;
    [SerializeField] private Transform point;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private EFinishType finishType;
    [SerializeField] private TextMeshProUGUI pointText;
    public EFinishType FinishType => this.finishType;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI tamanXBalance;

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

    public void SetFinishType(EFinishType buyAll)
    {
        this.finishType = buyAll;
    }

    public void SetPoint(float dataTamanXReward)
    {
        this.pointText.text = dataTamanXReward.ToString(CultureInfo.InvariantCulture);
    }

    public void SetDescriptionText()
    {
        var level = DataManager.Instance.Level;
        var levelData = DataManager.Instance.UserData.data.crystalData
            .Find(item => item.level == level);
        var chanceData = DataManager.Instance.UserData.data.chance;
        this.descriptionText.text = $"{GetIcon(0)}Receive {levelData.CHEST_VALUE}{GetIcon(7)} for 1 chest when you sell.\n\n" +
                                    $"{GetIcon(0)}Receive chance to have more when you open:\n" +
                                    $"\t{GetIcon(1)}{chanceData.NORMAL_CHEST}% change to receive {levelData.NORMAL_CHEST}{GetIcon(7)}\n" +
                                    $"\t{GetIcon(1)}{chanceData.RARE_CHEST}% change to receive {levelData.RARE_CHEST}{GetIcon(7)}\n" +
                                    $"\t{GetIcon(1)}{chanceData.EPIC_CHEST}% change to receive {levelData.EPIC_CHEST}{GetIcon(7)}\n" +
                                    $"\t{GetIcon(1)}{chanceData.LEGENDARY_CHEST}% change to receive {levelData.LEGENDARY_CHEST}{GetIcon(7)}\n";
    }

    private string GetIcon(int index)
    {
        return $"<sprite index={index}>";
    }

    public void SetTamanXBalance(float userTamanX)
    {
        this.tamanXBalance.text = userTamanX.ToString(CultureInfo.InvariantCulture);
    }
}