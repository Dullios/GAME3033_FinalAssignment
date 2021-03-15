﻿using System.Collections;
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
    protected BossBehaviour owner;

    public BossStates(BossBehaviour boss, StateMachine stateMachine) : base(stateMachine)
    {
        owner = boss;
    }
}

public enum BossStateType
{
    Idle,
    Attack,
    Follow,
    Dead
}