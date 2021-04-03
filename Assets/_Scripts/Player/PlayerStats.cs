using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
