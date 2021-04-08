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
    private List<GameObject> minions;

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
        if(bossCounter % 3 == 0)
        {
            for(int i = 0; i < minionSpawn.Length; i++)
            {
                GameObject minion = Instantiate(minionPrefab, minionSpawn[i], Quaternion.identity);
                minion.GetComponent<MinionController>().followTarget = player;
                minions.Add(minion);
            }
        }

        bossPrefabIndex = Random.Range(0, 3);
        currentBoss = Instantiate(bossPrefabs[bossPrefabIndex], bossSpawnPosition, Quaternion.identity);
        
        BossBehaviour bb = currentBoss.GetComponent<BossBehaviour>();
        bb.followTarget = player;
        bb.actionDelay = 3 - (bossCounter * 0.25f);

        EnemyStats es = currentBoss.GetComponent<EnemyStats>();
        es.currentHealth = es.maxHealth = 100 + (bossCounter * 10);
        es.damage = 10 + (bossCounter * 3);
    }

    public void BossDefeated()
    {
        foreach (GameObject min in minions)
            Destroy(min);

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

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
