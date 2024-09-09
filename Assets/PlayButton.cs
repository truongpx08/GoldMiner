using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(() =>
        {
            ApiService.Instance.Request(EApiType.PostStart,
                json =>
                {
                    var jsonObject = JsonUtility.FromJson<StartData>(json);
                    GameStateMachine.Instance.ChangeState(EGameState.Playing);
                    GameStateMachine.Instance.PlayingState.SetData(jsonObject.data);
                });
        });
    }
}