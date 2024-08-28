using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGamePlayState
{
    Initial,
    Start,
    GameOver,
}

public class GamePlayStateMachine : MonoBehaviour
{
    private TruongStateMachine stateMachine;
    [SerializeField] private EGamePlayState currentState;
    private InitialState initialState;

    private void Start()
    {
        ChangeState(EGamePlayState.Initial);
    }

    public void ChangeState(EGamePlayState state)
    {
        this.currentState = state;
        if (stateMachine == null) this.stateMachine = new TruongStateMachine();
        switch (state)
        {
            case EGamePlayState.Initial:
                this.initialState ??= gameObject.AddComponent<InitialState>();
                this.stateMachine.ChangeState(this.initialState);
                break;
            case EGamePlayState.Start:
                break;
            case EGamePlayState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}

public class InitialState : MonoBehaviour, IEnterState
{
    public void Enter()
    {
        MiningMachine.Instance.StateMachine.ChangeState(EMiningMachineState.RotateHook);

        // SpawnItem
        var itemTypeList = new List<EItemType> { EItemType.Trap };
        if (itemTypeList == null) throw new ArgumentNullException(nameof(itemTypeList));
        for (var i = 0; i < 5; i++)
        {
            itemTypeList.Add(EItemType.Time);
            itemTypeList.Add(EItemType.Reward);
        }

        foreach (var itemType in itemTypeList)
        {
            var item = Instantiate(Items.Instance.GetItemPrefabWithType(itemType),
                Items.Instance.GetRandomLineWithItem(itemType));
            item.GetComponent<Item>().StateMachine.ChangeState(EItemState.Appearing);
        }
    }
}