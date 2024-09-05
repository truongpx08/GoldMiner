using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootstrapManager : MonoBehaviour
{
    private void Start()
    {
        GameStateMachine.Instance.ChangeState(EGameState.SelectLevel);
    }
}