using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossStates
{
    public readonly int WalkingHash = Animator.StringToHash("Walking");

    public BossIdleState(BossBehaviour boss, StateMachine stateMachine, float delay) : base(boss, stateMachine, delay)
    {

    }

    public override void Start()
    {
        owner.navAgent.isStopped = true;
        owner.navAgent.ResetPath();
        owner.anim.SetFloat(WalkingHash, 0.0f);
    }
}
