using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;

public enum EGamePlayState
{
    Playing,
    ShowReward,
    TransferRewardToPoint,
    Reopen,
}

public class GamePlayStateMachine : TruongSingleton<GamePlayStateMachine>
{
    private TruongStateMachine stateMachine;
    [SerializeField] private EGamePlayState currentState;
    public EGamePlayState CurrentState => this.currentState;
    private GamePlayPlayingState playingState;
    private ShowRewardState showRewardState;
    private TransferRewardToPointState transferRewardToPointState;
    private ReopenState reopenState;

    protected override void Start()
    {
        ChangeState(EGamePlayState.Playing);
    }

    [Button]
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
            case EGamePlayState.Reopen:
                this.reopenState ??= gameObject.AddComponent<ReopenState>();
                this.stateMachine.ChangeState(this.reopenState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}


public class GamePlayBaseState : MonoBehaviour
{
    protected GameOverPanel GameOverPanel => GamePlayUI.Instance.GameOverPanel;
}

public class GamePlayPlayingState : GamePlayBaseState, IEnterState
{
    public void Enter()
    {
        GamePlayUI.Instance.GameOverPanel.gameObject.SetActive(false);
        MiningMachine.Instance.StateMachine.ChangeState(EMiningMachineState.RotateHook);
        Items.Instance.SpawnAllItem();
        GamePlayUI.Instance.Loading.SetActive(false);
    }
}

public class ShowRewardState : GamePlayBaseState, IEnterState
{
    public void Enter()
    {
        MiningMachine.Instance.StateMachine.ChangeState(EMiningMachineState.Stop);
        GameOverPanel.gameObject.SetActive(true);
        GameOverPanel.ShowReward();
        GameOverPanel.SetTamanXBalance(DataManager.Instance.UserData.data.user.tamanX);
        GameOverPanel.SetRewardText();
        GameOverPanel.SetDescriptionText();

        AudioManager.Instance.StopPullLine();
    }
}

public class TransferRewardToPointState : GamePlayBaseState, IEnterState
{
    public void Enter()
    {
        GameOverPanel.ShowPoint();
    }
}

public class ReopenState : GamePlayBaseState, IEnterState
{
    public void Enter()
    {
        if (!Application.isEditor)
        {
            GamePlayUI.Instance.Loading.SetActive(true);
            ReopenUtils.Instance.CallReact();
        }
    }
}