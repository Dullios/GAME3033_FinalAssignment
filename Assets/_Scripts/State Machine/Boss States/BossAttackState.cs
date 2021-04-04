using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossStates
{
    private GameObject followTarget;
    private float attackRange = 3.0f;

    public readonly int WalkingHash = Animator.StringToHash("Walking");
    public readonly int IsAttackingHash = Animator.StringToHash("isAttacking");

    public BossAttackState(GameObject target, BossBehaviour boss, StateMachine stateMachine, float delay) : base(boss, stateMachine, delay)
    {
        followTarget = target;
        updateInterval = 2.0f;
    }

    public override void Start()
    {
        delayTimer = 0.0f;

        owner.navAgent.isStopped = true;
        owner.navAgent.ResetPath();

        owner.anim.SetFloat(WalkingHash, 0.0f);
        owner.anim.SetTrigger(IsAttackingHash);
        owner.rightHand.enabled = true;
    }

    public override void IntervalUpdate()
    {
        // take damage
    }

    public override void Update()
    {
        owner.transform.LookAt(followTarget.transform.position, Vector3.up);

        float distanceBetween = Vector3.Distance(owner.transform.position, followTarget.transform.position);
        if (distanceBetween > attackRange)
            stateMachine.ChangeState(BossStateType.Follow);

        delayTimer += Time.deltaTime;

        if (delayTimer >= actionDelay)
        {
            owner.anim.SetTrigger(IsAttackingHash);
            owner.rightHand.enabled = true;
            delayTimer = 0.0f;
        }
    }

    public override void Exit()
    {
        owner.anim.SetBool(IsAttackingHash, false);
    }
}
