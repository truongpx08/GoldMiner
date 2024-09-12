using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class BootstrapManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GameStart();

    private void Start()
    {
        if (Application.isEditor)
            GameStateMachine.Instance.ChangeState(EGameState.SelectLevel);
        else
        {
            CallReact();
        }
    }

    private void CallReact()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    GameStart();
#endif
    }
}