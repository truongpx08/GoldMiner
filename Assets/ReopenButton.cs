using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReopenButton : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(() =>
        {
            GamePlayStateMachine.Instance.ChangeState(EGamePlayState.Reopen);
            
            
            Debug.Log("On click reopen button");
        });
    }
}