using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningMachine : TruongSingleton<MiningMachine>
{
    [SerializeField] private Transform pivot;
    public Transform Pivot => this.pivot;
    [SerializeField] private MiningMachineStateMachine stateMachine;
    public MiningMachineStateMachine StateMachine => this.stateMachine;
    [SerializeField] private Transform hookTransform;
    public Transform HookTransform => this.hookTransform;
    [SerializeField] private Rigidbody2D hookRb;
    public Rigidbody2D HookRb => this.hookRb;
    [SerializeField] private Renderer hookRenderer;
    public Renderer HookRenderer => this.hookRenderer;
    [SerializeField] private LineRenderer stringLineRenderer;
    public LineRenderer StringLineRenderer => this.stringLineRenderer;
    [SerializeField] private HookCollider hookCollider;
    public HookCollider HookCollider => this.hookCollider;
}