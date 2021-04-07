using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject statPanel;

    [Header("Player Details")]
    public Vector3 startPosition;

    [Header("Boss Details")]
    public GameObject[] bossPrefabs;
    public GameObject currentBoss;
    public Vector3 bossSpawnPosition;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        statPanel.SetActive(false);
    }

    public void StartNewRound()
    {
        statPanel.SetActive(false);

        //TODO: 
    }
}
