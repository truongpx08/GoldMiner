using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETagType
{
    Diamond
}

public class HookCollider : MonoBehaviour
{
    public GameObject HookedItem { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Xuyên qua đối tượng: " + other.tag);
        var stateMachine = MiningMachine.Instance.StateMachine;
        if (stateMachine.CurrentState == EMiningMachineState.DropLine)
        {
            this.HookedItem = other.gameObject;
            switch (Enum.Parse<ETagType>(this.HookedItem.tag))
            {
                case ETagType.Diamond:
                    stateMachine.ChangeState(EMiningMachineState.HookedItem);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void ClearHookedItem()
    {
        this.HookedItem = null;
    }
}