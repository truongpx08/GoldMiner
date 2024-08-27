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
    Disappearing // Ẩn đi  
}

public class ItemStateMachine : MonoBehaviour
{
    private TruongStateMachine stateMachine;
    [SerializeField] private EItemState currentState;
    private AppearingState appearingState;
    private ItemMovingState movingState;

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
                break;
            case EItemState.Disappearing:
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
    public void Enter()
    {
        LoadItemReference();
        itemReference.transform.localPosition = new Vector3(Random.Range(-3, 3), 0, 0);
        this.itemReference.StateMachine.ChangeState(EItemState.Moving);
    }
}

public class ItemMovingState : ItemBaseState, IEnterState, IExitState
{
    [SerializeField] private bool entering;
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển  
    [SerializeField] private float leftLimit = -3f; // Giới hạn bên trái  
    [SerializeField] private float rightLimit = 3f; // Giới hạn bên phải  
    [SerializeField] private bool movingToRight;
    private Vector3 targetPosition; // Vị trí đích  
    private Vector3 ItemPosition => this.itemReference.transform.position;

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
        this.itemReference.transform.position =
            Vector3.MoveTowards(ItemPosition, targetPosition, speed * Time.deltaTime);

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
            x = -0.5f;
        else
            x = 0.5f;
        itemTransform.localScale = new Vector3(x, lP.y, lP.z);
    }

    private void SetTargetPosition()
    {
        targetPosition = movingToRight
            ? new Vector3(rightLimit, ItemPosition.y, ItemPosition.z)
            : new Vector3(leftLimit, ItemPosition.y, ItemPosition.z);
    }
}