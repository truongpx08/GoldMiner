using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESelectLevelState
{
    GetData,
    InitializeUI,
    Play,
}

public class SelectLevelStateMachine : TruongSingleton<SelectLevelStateMachine>
{
    private TruongStateMachine stateMachine;
    [SerializeField] private ESelectLevelState currentState;
    public ESelectLevelState CurrentState => this.currentState;

    public GetDataState GetDataState { get; private set; }
    private InitializeUIState initializeUIState;

    public void ChangeState(ESelectLevelState state)
    {
        this.currentState = state;
        if (stateMachine == null) this.stateMachine = new TruongStateMachine();
        switch (state)
        {
            case ESelectLevelState.GetData:
                this.GetDataState ??= gameObject.AddComponent<GetDataState>();
                this.stateMachine.ChangeState(this.GetDataState);
                break;
            case ESelectLevelState.InitializeUI:
                this.initializeUIState ??= gameObject.AddComponent<InitializeUIState>();
                this.stateMachine.ChangeState(this.initializeUIState);
                break;
            case ESelectLevelState.Play:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}


public class GetDataState : MonoBehaviour, IEnterState
{
    [SerializeField] private UserData newData;
    public UserData NewData => this.newData;

    public void Enter()
    {
        ApiRequest.Instance.GetUserData(json =>
        {
            this.newData = JsonUtility.FromJson<UserData>(json);

            SelectLevelStateMachine.Instance.ChangeState(ESelectLevelState.InitializeUI);
        });
    }
}

public class InitializeUIState : MonoBehaviour, IEnterState
{
    public void Enter()
    {
        var data = SelectLevelStateMachine.Instance.GetDataState.NewData.data;
        HighScore.Instance.SetHighScore(data.highScore);
        Balance.Instance.SetBalanceText((int)data.user.taman);
    }
}