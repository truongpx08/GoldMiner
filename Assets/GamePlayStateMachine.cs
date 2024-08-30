using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGamePlayState
{
    Playing,
    ShowReward,
    TransferRewardToPoint,
}

public class GamePlayStateMachine : TruongSingleton<GamePlayStateMachine>
{
    private TruongStateMachine stateMachine;
    [SerializeField] private EGamePlayState currentState;
    public EGamePlayState CurrentState => this.currentState;
    private GamePlayPlayingState playingState;
    private ShowRewardState showRewardState;
    private TransferRewardToPointState transferRewardToPointState;

    protected override void Start()
    {
        ChangeState(EGamePlayState.Playing);
    }

    public void ChangeState(EGamePlayState state)
    {
        this.currentState = state;
        if (stateMachine == null) this.stateMachine = new TruongStateMachine();
        switch (state)
        {
            case EGamePlayState.Playing:
                this.playingState ??= gameObject.AddComponent<GamePlayPlayingState>();
                this.stateMachine.ChangeState(this.playingState);
                break;
            case EGamePlayState.ShowReward:
                this.showRewardState ??= gameObject.AddComponent<ShowRewardState>();
                this.stateMachine.ChangeState(this.showRewardState);
                break;
            case EGamePlayState.TransferRewardToPoint:
                this.transferRewardToPointState ??= gameObject.AddComponent<TransferRewardToPointState>();
                this.stateMachine.ChangeState(this.transferRewardToPointState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}


public class GamePlayPlayingState : MonoBehaviour, IEnterState
{
    public void Enter()
    {
        GamePlayUI.Instance.GameOverPanel.gameObject.SetActive(false);
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

public class GamePlayBaseState : MonoBehaviour
{
    protected GameOverPanel GameOverPanel => GamePlayUI.Instance.GameOverPanel;
}

public class ShowRewardState : GamePlayBaseState, IEnterState
{
    public void Enter()
    {
        GameOverPanel.gameObject.SetActive(true);
        GameOverPanel.ShowReward();
        this.GameOverPanel.SetRewardText();
    }
}

public class TransferRewardToPointState : GamePlayBaseState, IEnterState
{
    public void Enter()
    {
        GameOverPanel.ShowPoint();
    }
}