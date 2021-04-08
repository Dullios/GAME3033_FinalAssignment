using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatManager : MonoBehaviour, ISaveable
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

        GameManager.instance.OnSaveEvent.AddListener(OnSave);
        GameManager.instance.OnLoadEvent.AddListener(OnLoad);
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

        GameManager.instance.OnSave();

        Cursor.lockState = CursorLockMode.Locked;
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

    public void OnSave()
    {
        PlayerPrefs.SetInt("StatPoints", statPoints);
        PlayerPrefs.SetInt("HealthMinPoint", healthController.minPoint);
        PlayerPrefs.SetInt("DamageMinPoint", damageController.minPoint);
        PlayerPrefs.SetInt("BlockMinPoint", blockController.minPoint);
    }

    public void OnLoad()
    {
        statPoints = PlayerPrefs.GetInt("StatPoints");
        healthController.minPoint = PlayerPrefs.GetInt("HealthMinPoint");
        damageController.minPoint = PlayerPrefs.GetInt("DamageMinPoint");
        blockController.minPoint = PlayerPrefs.GetInt("BlockMinPoint");

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.LevelUp(healthController.minPoint, damageController.minPoint, blockController.minPoint);
    }
}
