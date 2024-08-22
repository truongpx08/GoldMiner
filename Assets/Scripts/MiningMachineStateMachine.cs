using System;
using UnityEngine;

public enum EMiningMachineState
{
    Rotating,
    Drop,
    Drag,
}

public class MiningMachineStateMachine : MonoBehaviour
{
    [SerializeField] private EMiningMachineState currentState;
    private TruongStateMachine stateMachine;
    private MiningMachineRotatingState rotatingState;
    public MiningMachineRotatingState RotatingState => this.rotatingState;
    private MiningMachineDropState dropState;
    private MiningMachineDragState dragState;
    public EMiningMachineState CurrentState => this.currentState;

    private void Start()
    {
        ChangeState(EMiningMachineState.Rotating);
    }

    public void ChangeState(EMiningMachineState state)
    {
        this.currentState = state;
        if (stateMachine == null) this.stateMachine = new TruongStateMachine();
        switch (state)
        {
            case EMiningMachineState.Rotating:
                if (this.rotatingState == null)
                    this.rotatingState = gameObject.AddComponent<MiningMachineRotatingState>();
                this.stateMachine.ChangeState(this.rotatingState);
                break;
            case EMiningMachineState.Drop:
                if (this.dropState == null)
                    this.dropState = gameObject.AddComponent<MiningMachineDropState>();
                this.stateMachine.ChangeState(this.dropState);
                break;
            case EMiningMachineState.Drag:
                if (this.dragState == null)
                    this.dragState = gameObject.AddComponent<MiningMachineDragState>();
                this.stateMachine.ChangeState(this.dragState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}

public class MiningMachineBaseState : MonoBehaviour
{
    [SerializeField] protected bool isEntering;
    [SerializeField] protected bool hasInit;
    [SerializeField] protected float speed;
    protected MiningMachine MiningMachine => MiningMachine.Instance;

    protected void UpdateString()
    {
        MiningMachine.StringLineRenderer.SetPosition(0, MiningMachine.Pivot.position);
        MiningMachine.StringLineRenderer.SetPosition(1, MiningMachine.HookTransform.transform.position);
    }
}

[Serializable]
public class MiningMachineRotatingState : MiningMachineBaseState, IEnterState, IExitState
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
        MiningMachine.StateMachine.ChangeState(EMiningMachineState.Drop);
    }
}

[Serializable]
public class MiningMachineDropState : MiningMachineBaseState, IEnterState, IExitState
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
        MiningMachine.StateMachine.ChangeState(EMiningMachineState.Drag);
    }
}

[Serializable]
public class MiningMachineDragState : MiningMachineBaseState, IEnterState, IExitState
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
            MiningMachine.StateMachine.RotatingState.InitialPosition.y)
            MiningMachine.StateMachine.ChangeState(EMiningMachineState.Rotating);
    }
}