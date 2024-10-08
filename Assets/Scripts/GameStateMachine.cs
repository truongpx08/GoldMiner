using System;
using UnityEngine;

public enum EGameState
{
    Bootstrap,
    SelectLevel, // Trạng thái chọn màn chơi  
    Playing, // Trạng thái đang chơi  
}

public class GameStateMachine : TruongSingleton<GameStateMachine>
{
    private TruongStateMachine stateMachine;
    [SerializeField] private EGameState currentState;
    private SelectLevelState selectLevelState;
    public PlayingState PlayingState { get; private set; }
    private BootstrapState bootstrapState;

    protected override void Start()
    {
        ChangeState(EGameState.Bootstrap);
    }

    public void ChangeState(EGameState state)
    {
        Debug.Log("Changing state" + state);
        this.currentState = state;
        if (stateMachine == null) this.stateMachine = new TruongStateMachine();
        switch (state)
        {
            case EGameState.Bootstrap:
                this.bootstrapState ??= new BootstrapState();
                this.stateMachine.ChangeState(this.bootstrapState);
                break;
            case EGameState.SelectLevel:
                this.selectLevelState ??= new SelectLevelState();
                this.stateMachine.ChangeState(this.selectLevelState);
                break;
            case EGameState.Playing:
                this.PlayingState ??= gameObject.AddComponent<PlayingState>();
                this.stateMachine.ChangeState(this.PlayingState);
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

public class PlayingState : MonoBehaviour, IEnterState
{
    [SerializeField] private StartDataDetails data;
    public StartDataDetails Data => this.data;

    public void Enter()
    {
        SceneLoader.Instance.LoadScene("Assets/Scenes/GamePlay.unity");
    }


    public void SetData(StartDataDetails jsonObjectData)
    {
        this.data = jsonObjectData;
    }
}

public class BootstrapState : IEnterState
{
    public void Enter()
    {
        SceneLoader.Instance.LoadScene("Assets/Scenes/Bootstrap.unity");
    }
}