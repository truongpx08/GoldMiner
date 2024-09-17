using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EItemState
{
    Appearing, // Hiện ra  
    Moving, // Di chuyển  
    CaughtByMachine, // Bị máy đào gắp trúng  
    Disappearing, // Ẩn đi  
    Stop // Dung lai
}

public class ItemStateMachine : MonoBehaviour
{
    private TruongStateMachine stateMachine;
    [SerializeField] private EItemState currentState;
    private AppearingState appearingState;
    private ItemMovingState movingState;
    private CaughtByMachineState caughtByMachineState;
    private ItemDisappearingState disappearingState;
    private ItemStopState stop;


    public void ChangeState(EItemState state)
    {
        this.currentState = state;
        if (stateMachine == null) this.stateMachine = new TruongStateMachine();
        switch (state)
        {
            case EItemState.Appearing:
                this.appearingState ??= gameObject.AddComponent<AppearingState>();
                this.stateMachine.ChangeState(this.appearingState);
                break;
            case EItemState.Moving:
                this.movingState ??= gameObject.AddComponent<ItemMovingState>();
                this.stateMachine.ChangeState(this.movingState);
                break;
            case EItemState.CaughtByMachine:
                this.caughtByMachineState ??= gameObject.AddComponent<CaughtByMachineState>();
                this.stateMachine.ChangeState(this.caughtByMachineState);
                break;
            case EItemState.Disappearing:
                this.disappearingState ??= gameObject.AddComponent<ItemDisappearingState>();
                this.stateMachine.ChangeState(this.disappearingState);
                break;
            case EItemState.Stop:
                this.stop ??= gameObject.AddComponent<ItemStopState>();
                this.stateMachine.ChangeState(this.stop);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}


public class ItemBaseState : MonoBehaviour
{
    [SerializeField] protected Item itemReference;

    protected void LoadItemReference()
    {
        this.itemReference = GetComponentInParent<Item>();
    }
}


public class AppearingState : ItemBaseState, IEnterState
{
    private const float SpacingX = 4;
    public const float LeftLimit = -6f; // Giới hạn bên trái  
    public const float RightLimit = 6f; // Giới hạn bên phải  

    public void Enter()
    {
        LoadItemReference();
        this.itemReference.CharacterSpine.gameObject.SetActive(true);
        this.itemReference.Reward.gameObject.SetActive(false);

        //Set Position
        var line = this.itemReference.transform.parent;
        if (line.childCount == 2)
        {
            var other = line.GetChild(0);
            var otherX = other.transform.localPosition.x;
            itemReference.transform.localPosition = otherX > 0
                ? new Vector3(Random.Range(LeftLimit, Mathf.Min(0f, otherX - SpacingX)), 0, 0)
                : new Vector3(Random.Range(Mathf.Max(0f, otherX + SpacingX), RightLimit), 0, 0);
        }
        else
            itemReference.transform.localPosition = new Vector3(Random.Range(LeftLimit, RightLimit), 0, 0);

        this.itemReference.StateMachine.ChangeState(EItemState.Moving);
    }
}

public class ItemMovingState : ItemBaseState, IEnterState, IExitState
{
    [SerializeField] private bool entering;
    [SerializeField] private float speed = 4f; // Tốc độ di chuyển  

    [SerializeField] private bool movingToRight;
    private Vector3 targetPosition; // Vị trí đích  
    private Vector3 ItemPosition => this.itemReference.transform.localPosition;

    public void Enter()
    {
        LoadItemReference();
        this.entering = true;
    }

    public void Exit()
    {
        this.entering = false;
    }


    private void Start()
    {
        // Đặt vị trí bắt đầu  
        List<bool> boolList = new List<bool> { true, false };
        this.movingToRight = boolList[Random.Range(0, boolList.Count)];

        SetTargetPosition();
        SetDirection();
    }

    private void Update()
    {
        if (this.entering)
            Move();
    }

    private void Move()
    {
        // Di chuyển vật phẩm tới vị trí mục tiêu  
        this.itemReference.transform.localPosition =
            Vector3.MoveTowards(ItemPosition, targetPosition, speed * UnityEngine.Time.deltaTime);

        // Kiểm tra nếu vật phẩm đã đến vị trí mục tiêu  
        if (Vector3.Distance(ItemPosition, targetPosition) < 0.1f)
        {
            // Đảo ngược hướng di chuyển  
            movingToRight = !movingToRight;

            SetTargetPosition();
            SetDirection();
        }
    }

    private void SetDirection()
    {
        var itemTransform = this.itemReference.transform;
        Vector3 lP = itemTransform.localScale;
        float x;
        if (this.movingToRight)
            x = -1f;
        else
            x = 1f;
        itemTransform.localScale = new Vector3(x, lP.y, lP.z);
    }

    private void SetTargetPosition()
    {
        targetPosition = movingToRight
            ? new Vector3(AppearingState.RightLimit, ItemPosition.y, ItemPosition.z)
            : new Vector3(AppearingState.LeftLimit, ItemPosition.y, ItemPosition.z);
    }
}

public class CaughtByMachineState : ItemBaseState, IEnterState
{
    public void Enter()
    {
        LoadItemReference();
        this.itemReference.CharacterSpine.gameObject.SetActive(false);
        this.itemReference.Reward.gameObject.SetActive(true);
    }
}

public class ItemDisappearingState : ItemBaseState, IEnterState
{
    public void Enter()
    {
        LoadItemReference();
        this.itemReference.gameObject.SetActive(false);
    }
}

public class ItemStopState : ItemBaseState, IEnterState
{
    public void Enter()
    {
    }
}