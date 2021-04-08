using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Statistics
{
    HEALTH,
    DAMAGE,
    BLOCK
}

public class PlayerStats : MonoBehaviour
{
    [Header("Player States")]
    public bool isRunning;
    public bool isJumping;
    public bool isAttacking;
    public bool isBlocking;
    public bool hasBlocked;
    public bool inMenu;

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
        FinishLevelUp();
    }

    public void ModifyHealth(int value)
    {
        currentHealth += value;

        currentHealth = Mathf.Max(currentHealth, 0);
        currentHealth = Mathf.Min(maxHealth, currentHealth);

        healthBar.value = currentHealth;

        if (currentHealth <= 0)
            GameManager.instance.PlayerDefeated();
    }

    public void LevelUp(int health, int damage, int block)
    {
        healthLevel = health;
        damageLevel = damage;
        blockLevel = block;

        FinishLevelUp();
    }

    //public void OnModifyHealthLevel(bool levelUp)
    //{
    //    healthLevel += levelUp ? 1 : -1;
    //}

    //public void OnModifyDamageLevel(bool levelUp)
    //{
    //    damageLevel += levelUp ? 1 : -1;
    //}

    //public void OnModifyBlockLevel(bool levelUp)
    //{
    //    blockLevel += levelUp ? 1 : -1;
    //}

    public void FinishLevelUp()
    {
        maxHealth = 100 + (healthLevel * 10);
        currentHealth = maxHealth;

        healthBar.value = currentHealth;

        damage = 5 + (damageLevel * 2);

        blockPercent = 65 + (blockLevel * 5);
    }
}
