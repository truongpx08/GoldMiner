using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionDescriptionText : TruongSingleton<OptionDescriptionText>
{
    [SerializeField] private TextMeshProUGUI tmp;

    public void SetText(int level)
    {
        var levelData = DataManager.Instance.UserData.data.crystalData
            .Find(item => item.level == level);
        var chanceData = DataManager.Instance.UserData.data.chance;
        this.tmp.text = $"{GetIcon(2)}Receive {levelData.CHEST_VALUE}{GetIcon(7)} for 1 chest when you sell.\n\n" +
                        $"{GetIcon(2)}Receive chance to have more when you open:\n" +
                        $"\t{GetIcon(3)}{chanceData.NORMAL_CHEST}% change to receive {levelData.NORMAL_CHEST}{GetIcon(7)}\n" +
                        $"\t{GetIcon(3)}{chanceData.RARE_CHEST}% change to receive {levelData.RARE_CHEST}{GetIcon(7)}\n" +
                        $"\t{GetIcon(3)}{chanceData.EPIC_CHEST}% change to receive {levelData.EPIC_CHEST}{GetIcon(7)}\n" +
                        $"\t{GetIcon(3)}{chanceData.LEGENDARY_CHEST}% change to receive {levelData.LEGENDARY_CHEST}{GetIcon(7)}\n";
    }

    private string GetIcon(int index)
    {
        return $"<sprite index={index}>";
    }
}