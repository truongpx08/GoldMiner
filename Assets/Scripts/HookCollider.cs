using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollider : MonoBehaviour
{
    public Item HookedItem { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Xuyên qua đối tượng: " + other.tag);
        var currentItem = other.gameObject.GetComponent<Item>();
        if (currentItem == null) return; // Ensure currentItem is not null  

        string itemTag = currentItem.tag;

        var miningMachineStateMachine = MiningMachine.Instance.StateMachine;
        switch (miningMachineStateMachine.CurrentState)
        {
            case EMiningMachineState.DropLine:

                switch (itemTag)
                {
                    case nameof(EItemType.Time):
                    case nameof(EItemType.Chest):
                        HookItem(currentItem, miningMachineStateMachine);
                        break;

                    case nameof(EItemType.Trap):
                        ClearHookedItem();
                        miningMachineStateMachine.ChangeState(EMiningMachineState.PullLine);
                        miningMachineStateMachine.PullLineState.SetSpeedSameDropLine();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case EMiningMachineState.PullLine:
                if (itemTag == nameof(EItemType.Trap) && this.HookedItem != null)
                {
                    miningMachineStateMachine.PullLineState.SetSpeedSameDropLine();
                    this.HookedItem.StateMachine.ChangeState(EItemState.Disappearing);
                    ClearHookedItem();
                }

                break;
        }
    }

    private void HookItem(Item currentItem, MiningMachineStateMachine miningMachineStateMachine)
    {
        this.HookedItem = currentItem;
        miningMachineStateMachine.ChangeState(EMiningMachineState.HookedItem);
        this.HookedItem.StateMachine.ChangeState(EItemState.CaughtByMachine);
    }

    public void ClearHookedItem()
    {
        this.HookedItem = null;
    }
}