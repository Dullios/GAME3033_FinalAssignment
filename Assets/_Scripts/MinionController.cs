using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MinionState
{
    IDLE,
    WALKING,
    ATTACKING,
    DEAD
}

public class MinionController : MonoBehaviour
{
    [Header("Minion States")]
    public MinionState state;
    public CapsuleCollider horn;
    public bool isDead;
    
    [Header("Navigation")]
    public GameObject followTarget;
    public float currentDistance;
    public float stopDistance;
    public float delayAction;
    public float actionTimer;

    // Components
    private EnemyStats stats;
    private Animator anim;
    private NavMeshAgent navAgent;

    // Animator Hashes
    public readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    public readonly int IsAttackingHash = Animator.StringToHash("isAttacking");
    public readonly int IsDeadHash = Animator.StringToHash("isDead");

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        state = MinionState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MinionState.IDLE:
                navAgent.isStopped = true;
                navAgent.ResetPath();
                
                anim.SetBool(IsWalkingHash, false);

                currentDistance = Vector3.Distance(transform.position, followTarget.transform.position);
                if (currentDistance > stopDistance)
                    state = MinionState.WALKING;
                else
                {
                    actionTimer += Time.deltaTime;

                    if(actionTimer >= delayAction)
                    {
                        state = MinionState.ATTACKING;
                        actionTimer = 0;
                    }
                }
                break;
            
            case MinionState.WALKING:
                navAgent.isStopped = true;
                navAgent.ResetPath();

                navAgent.SetDestination(followTarget.transform.position);

                anim.SetBool(IsWalkingHash, true);

                currentDistance = Vector3.Distance(transform.position, followTarget.transform.position);
                if (currentDistance <= stopDistance)
                    state = MinionState.IDLE;
                break;
            
            case MinionState.ATTACKING:
                anim.SetTrigger(IsAttackingHash);
                state = MinionState.IDLE;
                break;
            
            case MinionState.DEAD:
                if (isDead)
                    return;

                anim.SetBool(IsDeadHash, true);
                isDead = true;
                break;
        }
    }

    IEnumerator OnDead()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
