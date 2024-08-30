using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(() => { GamePlayStateMachine.Instance.ChangeState(EGamePlayState.TransferRewardToPoint); });
    }
}