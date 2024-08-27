using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Trap, // Bẫy  
    Time, // Thời gian  
    Reward // Phần thưởng  
}

public class Item : MonoBehaviour
{
    [SerializeField] private ItemStateMachine stateMachine;
    public ItemStateMachine StateMachine => this.stateMachine;
}