using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(() =>
        {
            GamePlayUI.Instance.GameOverPanel.SetFinishType(EFinishType.buyAll);
            ApiService.Instance.Request(EApiType.PostFinish, json =>
            {
                var jsonObject = JsonUtility.FromJson<FinishData>(json);
                GamePlayUI.Instance.GameOverPanel.SetPoint(jsonObject.data.tamanXReward);
                GamePlayStateMachine.Instance.ChangeState(EGamePlayState.TransferRewardToPoint);
            });
        });
    }
}