using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(() => GameStateMachine.Instance.ChangeState(EGameState.SelectLevel));
    }
}