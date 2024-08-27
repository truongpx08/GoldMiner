using System;
using UnityEngine;

public enum EMiningMachineState
{
    RotateHook, // Xoay qua lại móc câu để chọn hướng thả dây câu  
    DropLine, // Thả dây câu  
    PullLine, // Kéo dây câu  
    HookedItem, // Móc trúng vật phẩm  
    ReceiveItem // Nhận vật phẩm  
}

public class MiningMachineStateMachine : MonoBehaviour
{
    [SerializeField] private EMiningMachineState currentState;
    public EMiningMachineState CurrentState => this.currentState;
    private TruongStateMachine stateMachine;
    public RotateHookState RotateHookState { get; private set; }
    private DropLineState dropLineState;
    private PullLineState pullLineState;
    private HookedItemState hookedItemState;
    private ReceiveItemState receiveItemState;

    private void Start()
    {
        ChangeState(EMiningMachineState.RotateHook);
    }

    public void ChangeState(EMiningMachineState state)
    {
        this.currentState = state;
        if (stateMachine == null) this.stateMachine = new TruongStateMachine();
        switch (state)
        {
            case EMiningMachineState.RotateHook:
                if (this.RotateHookState == null)
                    this.RotateHookState = gameObject.AddComponent<RotateHookState>();
                this.stateMachine.ChangeState(this.RotateHookState);
                break;
            case EMiningMachineState.DropLine:
                if (this.dropLineState == null)
                    this.dropLineState = gameObject.AddComponent<DropLineState>();
                this.stateMachine.ChangeState(this.dropLineState);
                break;
            case EMiningMachineState.PullLine:
                if (this.pullLineState == null)
                    this.pullLineState = gameObject.AddComponent<PullLineState>();
                this.stateMachine.ChangeState(this.pullLineState);
                break;
            case EMiningMachineState.HookedItem:
                if (this.hookedItemState == null)
                    this.hookedItemState = gameObject.AddComponent<HookedItemState>();
                this.stateMachine.ChangeState(this.hookedItemState);
                break;
            case EMiningMachineState.ReceiveItem:
                if (this.receiveItemState == null)
                    this.receiveItemState = gameObject.AddComponent<ReceiveItemState>();
                this.stateMachine.ChangeState(this.receiveItemState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}


public class MiningMachineBase1State : MonoBehaviour
{
    protected MiningMachine MiningMachine => MiningMachine.Instance;
}

public class MiningMachineBase2State : MiningMachineBase1State
{
    [SerializeField] protected bool isEntering;
    [SerializeField] protected bool hasInit;
    [SerializeField] protected float speed;

    protected void UpdateString()
    {
        MiningMachine.StringLineRenderer.SetPosition(0, MiningMachine.Pivot.position);
        MiningMachine.StringLineRenderer.SetPosition(1, MiningMachine.HookTransform.transform.position);
    }
}

[Serializable]
public class RotateHookState : MiningMachineBase2State, IEnterState, IExitState
{
    [SerializeField] private float angleMax = 70;
    [SerializeField] private float time;
    [SerializeField] private Vector3 initialPosition;
    public Vector3 InitialPosition => this.initialPosition;

    public void Enter()
    {
        if (!this.hasInit)
        {
            hasInit = true;
            this.speed = 5f;
            this.initialPosition = this.MiningMachine.HookTransform.localPosition;
        }

        this.MiningMachine.HookRb.velocity = Vector2.zero;
        this.MiningMachine.HookTransform.localPosition = initialPosition; // Khôi phục vị trí ban đầu  

        this.isEntering = true;
    }

    public void Exit()
    {
        this.isEntering = false;
    }

    private void FixedUpdate()
    {
        //RotatePivot();
        if (!isEntering) return;
        this.time += Time.fixedDeltaTime;
        float rotationZ = Mathf.Sin(this.time * speed) * angleMax;
        MiningMachine.Pivot.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    private void Update()
    {
        if (!isEntering) return;
        UpdateString();
        //HandleUserInput();
        if (!Input.GetMouseButtonDown(0)) return;
        MiningMachine.StateMachine.ChangeState(EMiningMachineState.DropLine);
    }
}

[Serializable]
public class DropLineState : MiningMachineBase2State, IEnterState, IExitState
{
    [SerializeField] private Vector2 velocity;

    public void Enter()
    {
        if (!this.hasInit)
        {
            hasInit = true;
            this.speed = 6f;
        }

        this.isEntering = true;
    }

    public void Exit()
    {
        this.isEntering = false;
    }

    private void FixedUpdate()
    {
        //HandleDrag
        if (!isEntering) return;
        velocity = (MiningMachine.HookTransform.position - MiningMachine.Pivot.position)
            .normalized;
        this.MiningMachine.HookRb.velocity = velocity * speed;
    }

    private void Update()
    {
        if (!isEntering) return;
        UpdateString();
        HandleOutOfCameraView();
    }

    private void HandleOutOfCameraView()
    {
        if (this.MiningMachine.HookRenderer.isVisible) return;
        MiningMachine.StateMachine.ChangeState(EMiningMachineState.PullLine);
    }
}

[Serializable]
public class PullLineState : MiningMachineBase2State, IEnterState, IExitState
{
    [SerializeField] private Vector2 velocity;

    public void Enter()
    {
        if (!this.hasInit)
        {
            hasInit = true;
            this.speed = 6f;
        }

        this.isEntering = true;
    }

    public void Exit()
    {
        this.isEntering = false;
    }

    private void FixedUpdate()
    {
        //HandleDrag
        if (!isEntering) return;
        velocity = (this.MiningMachine.HookTransform.transform.position - this.MiningMachine.Pivot.position)
            .normalized;
        this.MiningMachine.HookRb.velocity = -velocity * speed;
    }

    private void Update()
    {
        if (!isEntering) return;
        UpdateString();
        HandleDragComplete();
    }

    private void HandleDragComplete()
    {
        if (this.MiningMachine.HookTransform.localPosition.y >=
            MiningMachine.StateMachine.RotateHookState.InitialPosition.y)
            MiningMachine.StateMachine.ChangeState(MiningMachine.HookCollider.HookedItem != null
                ? EMiningMachineState.ReceiveItem
                : EMiningMachineState.RotateHook);
    }
}

public class HookedItemState : MiningMachineBase1State, IEnterState
{
    public void Enter()
    {
        var hookedItem = MiningMachine.HookCollider.HookedItem;
        hookedItem.transform.parent = MiningMachine.HookTransform;
        hookedItem.transform.localPosition = Vector3.zero;

        this.MiningMachine.StateMachine.ChangeState(EMiningMachineState.PullLine);
    }
}

public class ReceiveItemState : MiningMachineBase1State, IEnterState
{
    public void Enter()
    {
        MiningMachine.HookCollider.HookedItem.SetActive(false);
        MiningMachine.HookCollider.ClearHookedItem();

        this.MiningMachine.StateMachine.ChangeState(EMiningMachineState.RotateHook);
    }
}