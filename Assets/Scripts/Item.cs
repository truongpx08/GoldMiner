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
    [SerializeField] private ItemStateMachine stateMachine;
    public ItemStateMachine StateMachine => this.stateMachine;
    [SerializeField] private SpriteRenderer sR;
    public SpriteRenderer SR => this.sR;
    [SerializeField] private Sprite rewardSprite;
    public Sprite RewardSprite => this.rewardSprite;
    [SerializeField] private Sprite characterSprite;
    public Sprite CharacterSprite => this.characterSprite;
}