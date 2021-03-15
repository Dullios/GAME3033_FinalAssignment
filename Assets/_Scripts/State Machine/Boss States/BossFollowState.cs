using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowState : BossStates
{
    private GameObject followTarget;

    private const float stopDistance = 3.0f;

    public readonly int WalkingHash = Animator.StringToHash("Walking");

    public BossFollowState(GameObject target, BossBehaviour boss, StateMachine stateMachine) : base(boss, stateMachine)
    {
        followTarget = target;
        updateInterval = 2.0f;
    }

    public override void Start()
    {
        owner.navAgent.SetDestination(followTarget.transform.position);
    }

    public override void IntervalUpdate()
    {
        owner.navAgent.SetDestination(followTarget.transform.position);
    }

    public override void Update()
    {
        owner.anim.SetFloat(WalkingHash, owner.navAgent.velocity.normalized.z);

        if(Vector3.Distance(owner.transform.position, followTarget.transform.position) <= stopDistance)
        {
            stateMachine.ChangeState(BossStateType.Attack);
        }
    }
}
