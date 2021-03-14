using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected StateMachine stateMachine;
    public float updateInterval { get; protected set; } = 1.0f;

    protected State(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    public virtual void Start()
    {

    }

    public virtual void IntervalUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}

public class BossStates : State
{
    protected BossStates ownerZombie;

    public BossStates(BossStates zombie, StateMachine stateMachine) : base(stateMachine)
    {
        ownerZombie = zombie;
    }
}

public enum BossStateType
{
    Idle,
    Attack,
    Follow,
    Dead
}