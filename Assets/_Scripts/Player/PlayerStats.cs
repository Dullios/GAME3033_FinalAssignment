using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Player States")]
    public bool isRunning;
    public bool isJumping;
    public bool isAttacking;
    public bool isBlocking;
    public bool hasBlocked;

    [Header("Attribtues")]
    public int currentHealth;
    public int maxHealth;
    public int damage;
    public int blockPercent;

    [Header("Attribute Levels")]
    private int healthLevel;
    private int damageLevel;
    private int blockLevel;

    [Header("UI Elements")]
    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth = 100;
        damage = 5;
    }

    public void ModifyHealth(int value)
    {
        currentHealth += value;

        currentHealth = Mathf.Max(currentHealth, 0);
        currentHealth = Mathf.Min(maxHealth, currentHealth);

        healthBar.value = currentHealth;
    }

    public void OnModifyHealthLevel(bool levelUp)
    {
        healthLevel += levelUp ? 1 : -1;
    }

    public void OnModifyDamageLevel(bool levelUp)
    {
        damageLevel += levelUp ? 1 : -1;
    }

    public void OnModifyBlockLevel(bool levelUp)
    {
        blockLevel += levelUp ? 1 : -1;
    }

    public void FinishLevelUp()
    {
        // TODO: apply level changes to stats
    }
}
