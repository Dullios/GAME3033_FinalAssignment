using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : BossStates
{
    private static readonly int WalkingHash = Animator.StringToHash("Walking");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    public BossDeadState(BossBehaviour boss, StateMachine stateMachine) : base(boss, stateMachine)
    {

    }

    public override void Start()
    {
        owner.navAgent.isStopped = true;
        owner.navAgent.ResetPath();

        owner.anim.SetFloat(WalkingHash, 0.0f);
        owner.anim.SetBool(IsDeadHash, true);
    }

    public override void Exit()
    {
        owner.navAgent.isStopped = false;

        owner.anim.SetBool(IsDeadHash, false);
    }
}
