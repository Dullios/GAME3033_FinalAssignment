using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Attribtues")]
    public int currentHealth;
    public int maxHealth;
    public int damage;

    // Animator Hashes
    public readonly int IsDeadHash = Animator.StringToHash("isDead");

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth = 100;
        damage = 10;
    }

    public void SetStats(int health, int dmg)
    {
        currentHealth = maxHealth = health;
        damage = dmg;
    }

    public void DealDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            GetComponent<BossBehaviour>().stateMachine.ChangeState(BossStateType.Dead);
    }
}
