using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(() => GameStateMachine.Instance.ChangeState(EGameState.Playing));
    }
}