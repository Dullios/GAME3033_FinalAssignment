using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public int statPoints;
    [SerializeField]
    private TMP_Text statDisplay;

    [Header("Stat Panels")]
    public StatController healthController;
    public StatController damageController;
    public StatController blockController;

    public static StatManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    public void AddPoints(int value)
    {
        statPoints += value;
        UpdateDisplay();
    }

    public bool UsePoint()
    {
        if (statPoints == 0)
            return false;
        else
        {
            statPoints--;
            UpdateDisplay();
            return true;
        }
    }

    public void ReturnPoint()
    {
        statPoints++;
        UpdateDisplay();
    }

    public void LevelUpPlayer()
    {
        healthController.minPoint = healthController.currentPoint;
        damageController.minPoint = damageController.currentPoint;
        blockController.minPoint = blockController.currentPoint;

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        playerStats.LevelUp(healthController.minPoint, damageController.minPoint, blockController.minPoint);
    }

    public void ResetStats()
    {
        statPoints = 0;

        healthController.minPoint = 0;
        healthController.currentPoint = 0;

        damageController.minPoint = 0;
        damageController.currentPoint = 0;

        blockController.minPoint = 0;
        blockController.currentPoint = 0;
    }

    private void UpdateDisplay()
    {
        statDisplay.text = $"Points: {statPoints}";
    }
}
