using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Trap, // Bẫy  
    Time, // Thời gian  
    Chest // Rương
}

public class Item : MonoBehaviour
{
    [SerializeField] private EItemType type;
    public EItemType Type => this.type;
    [SerializeField] private ItemStateMachine stateMachine;
    public ItemStateMachine StateMachine => this.stateMachine;
    [SerializeField] private GameObject characterSpine;
    public GameObject CharacterSpine => this.characterSpine;
    [SerializeField] private GameObject reward;
    public GameObject Reward => this.reward;
}