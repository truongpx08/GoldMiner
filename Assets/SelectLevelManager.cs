using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelManager : MonoBehaviour
{
    private void Start()
    {
        SelectLevelStateMachine.Instance.ChangeState(ESelectLevelState.GetData);
    }
}
