using System;
using UnityEngine;

public enum EGameState
{
    SelectLevel, // Trạng thái chọn màn chơi  
    Playing, // Trạng thái đang chơi  
}

public class GameStateMachine : TruongSingleton<GameStateMachine>
{
    private TruongStateMachine stateMachine;
    [SerializeField] private EGameState currentState;
    private SelectLevelState selectLevelState;
    private PlayingState playingState;

    protected override void Start()
    {
        ChangeState(EGameState.SelectLevel);
    }

    public void ChangeState(EGameState state)
    {
        Debug.Log("Changing state" + state);
        this.currentState = state;
        if (stateMachine == null) this.stateMachine = new TruongStateMachine();
        switch (state)
        {
            case EGameState.SelectLevel:
                this.selectLevelState ??= new SelectLevelState();
                this.stateMachine.ChangeState(this.selectLevelState);
                break;
            case EGameState.Playing:
                this.playingState ??= new PlayingState();
                this.stateMachine.ChangeState(this.playingState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}


public class SelectLevelState : IEnterState
{
    public void Enter()
    {
        SceneLoader.Instance.LoadScene("Assets/Scenes/SelectLevel.unity");
    }
}

public class PlayingState : IEnterState
{
    public void Enter()
    {
        SceneLoader.Instance.LoadScene("Assets/Scenes/GamePlay.unity");
    }
}