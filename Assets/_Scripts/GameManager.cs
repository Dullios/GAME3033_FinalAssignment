using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject statPanel;
    public GameObject gameOverPanel;

    [Header("Player Details")]
    public GameObject player;
    public Vector3 startPosition;

    [Header("Boss Details")]
    public GameObject[] bossPrefabs;
    public GameObject currentBoss;
    public Vector3 bossSpawnPosition;

    [Header("Minion Spawn")]
    public GameObject minionPrefab;
    public Vector3[] minionSpawn;

    [Header("Game Stats")]
    public int bossCounter = -1;
    public int bossPrefabIndex;

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
        StartNewRound();

        /* 
         * TODO: Check for save
         * 
         * New game:
         *  set bossCounter to 0
         * 
         * */
    }

    public void StartNewRound()
    {
        statPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        
        player.GetComponent<PlayerStats>().inMenu = false;
        player.transform.position = startPosition;

        bossCounter++;
        // TODO: check mod 3 to spawn minions

        bossPrefabIndex = Random.Range(0, 3);
        currentBoss = Instantiate(bossPrefabs[bossPrefabIndex], bossSpawnPosition, Quaternion.identity);
        
        BossBehaviour bb = currentBoss.GetComponent<BossBehaviour>();
        bb.followTarget = player;
        //bb.actionDelay = 
        // TODO: set enemy stats
    }

    public void BossDefeated()
    {
        StartCoroutine(BossDefeatedRoutine());
    }

    IEnumerator BossDefeatedRoutine()
    {
        yield return new WaitForSeconds(3);

        Cursor.lockState = CursorLockMode.None;
        
        statPanel.SetActive(true);
        StatManager.instance.AddPoints(2);

        player.GetComponent<PlayerStats>().inMenu = true;
    }

    public void PlayerDefeated()
    {
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        player.GetComponent<PlayerStats>().inMenu = true;
    }

    public void ResetGame()
    {
        bossCounter = -1;
        Destroy(currentBoss);
        currentBoss = null;

        player.GetComponent<PlayerStats>().FinishLevelUp();

        StartNewRound();
    }
}
