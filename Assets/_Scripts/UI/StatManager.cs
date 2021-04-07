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
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        playerStats.LevelUp(healthController.minPoint, damageController.minPoint, blockController.minPoint);
    }

    private void UpdateDisplay()
    {
        statDisplay.text = $"Points: {statPoints}";
    }
}
