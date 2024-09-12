using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionDescriptionText : TruongSingleton<OptionDescriptionText>
{
    [SerializeField] private TextMeshProUGUI tmp;

    public void SetText(int level)
    {
        var levelData = SelectLevelStateMachine.Instance.GetDataState.Data.data.crystalData
            .Find(item => item.level == level);
        var chanceData = SelectLevelStateMachine.Instance.GetDataState.Data.data.chance;
        this.tmp.text = $"Receive {levelData.CHEST_VALUE} for 1 chest when you sell.\n\n" +
                        "Receive chance to have more when you open:\n" +
                        $"\t{chanceData.NORMAL_CHEST}% change to receive {levelData.NORMAL_CHEST}\n" +
                        $"\t{chanceData.RARE_CHEST}% change to receive {levelData.RARE_CHEST}\n" +
                        $"\t{chanceData.EPIC_CHEST}% change to receive {levelData.EPIC_CHEST}\n" +
                        $"\t{chanceData.LEGENDARY_CHEST}% change to receive {levelData.LEGENDARY_CHEST}\n";
    }
}