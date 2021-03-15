using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehaviour : MonoBehaviour
{
    // Components
    public Animator anim { get; private set; }
    public NavMeshAgent navAgent { get; private set; }
    public StateMachine stateMachine { get; private set; }

    public GameObject followTarget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();

        Initialize(followTarget);
    }

    public void Initialize(GameObject target)
    {
        followTarget = target;

        BossIdleState idleState = new BossIdleState(this, stateMachine);
        stateMachine.AddState(BossStateType.Idle, idleState);

        BossFollowState followState = new BossFollowState(followTarget, this, stateMachine);
        stateMachine.AddState(BossStateType.Follow, followState);

        BossAttackState attackState = new BossAttackState(followTarget, this, stateMachine);
        stateMachine.AddState(BossStateType.Attack, attackState);

        BossDeadState deadState = new BossDeadState(this, stateMachine);
        stateMachine.AddState(BossStateType.Dead, deadState);

        stateMachine.Initialize(BossStateType.Follow);
    }
}
