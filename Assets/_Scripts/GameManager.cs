using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, ISaveable
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
    private List<GameObject> minions = new List<GameObject>();

    [Header("Game Stats")]
    public int bossCounter = -1;
    public int bossPrefabIndex;

    public UnityEvent OnSaveEvent;
    public UnityEvent OnLoadEvent;

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
        if(PlayerPrefs.HasKey("BossCounter"))
        {
            OnLoad();
        }
        else
        {
            bossCounter = -1;
        }

        StartNewRound();
    }

    public void StartNewRound()
    {
        statPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        
        player.GetComponent<PlayerStats>().inMenu = false;
        player.transform.position = startPosition;

        bossCounter++;
        if(bossCounter != 00 && bossCounter % 3 == 0)
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
        player.GetComponent<PlayerStats>().inMenu = true;

        yield return new WaitForSeconds(3);
        Destroy(currentBoss);
        currentBoss = null;

        Cursor.lockState = CursorLockMode.None;
        
        statPanel.SetActive(true);
        StatManager.instance.AddPoints(2);
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

        foreach (GameObject minion in minions)
            Destroy(minion);

        player.GetComponent<PlayerStats>().FinishLevelUp();

        StartNewRound();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnSave()
    {
        OnSaveEvent.Invoke();

        PlayerPrefs.SetInt("BossCounter", bossCounter);
        PlayerPrefs.SetInt("BossIndex", bossPrefabIndex);
    }

    public void OnLoad()
    {
        OnLoadEvent.Invoke();

        bossCounter = PlayerPrefs.GetInt("BossCounter") - 1;
        bossPrefabIndex = PlayerPrefs.GetInt("BossIndex");
    }
}
