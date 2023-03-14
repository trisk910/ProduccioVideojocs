using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class IASpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyL
    {
        public GameObject enemyPrefab;
        public int cost;
        public int maxPerRound;
    }
    [Header("List of Enemies")]
    public List<EnemyL> enemies = new List<EnemyL>();
    

    public Transform[] spawnLocation;
    private int spawnIndex;


    private bool canSpawn;

    private int spawnTimerBetweenWaves;
    [Header("Spawner Currency")]
    public float Currency;
    public float InitialCurrency;
    public float currencyMultiplyer;  

    private float multiplyerTimer;

    [Header("Rounds")]
    public int currentRound = 1;
    public TextMeshProUGUI currentRoundText;
    public TextMeshProUGUI enemiesLeftText;
    public List<GameObject> spawnedSaltarin = new List<GameObject>();
    public List<GameObject> spawnedDemonio = new List<GameObject>();
    public int maxPerWave;
    public int enemiesAlive;
    public int totalSpwanedEnemies;

    private GameObject Player;
    public GameObject UpGradeMenu;

    private GameObject Radar;

    void Start()
    {
        canSpawn = true;
        SpawnEnemy();
        InitialCurrency = 25f;
        Currency = InitialCurrency;
        currencyMultiplyer = .05f;
        maxPerWave = 18;
        UpGradeMenu.SetActive(false);
        Player = GameObject.FindGameObjectWithTag("Player");
        Radar = GameObject.FindGameObjectWithTag("Radar");
    }

    // Update is called once per frame
    void Update()
    {
        //spawnIndex = Random.Range(0, count);

        if (canSpawn){
            Currency += currencyMultiplyer * (Time.deltaTime);
            SpawnEnemy();            
        }


        currentRoundText.SetText("Round: " + currentRound);
        enemiesLeftText.SetText("Enemies alive: " + enemiesAlive);
    
        startMultiplyerTimer();
        roundStatus();
        XrayEnabler();
    }

    private void SpawnEnemy()
    {
        for(int i = 0; i < enemies.Count; i++) {
            spawnIndex = Random.Range(0, spawnLocation.Length);
         
            if ((Currency >= enemies[i].cost) && (spawnedSaltarin.Count < enemies[0].maxPerRound) )//saltarin
            {
                GameObject enemy = Instantiate<GameObject>(enemies[0].enemyPrefab, spawnLocation[spawnIndex].position, Quaternion.identity);
                spawnedSaltarin.Add(enemy);
                enemiesAlive++;
                totalSpwanedEnemies++;
                Currency -= enemies[i].cost;
                Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
            }
            if ((Currency >= enemies[i].cost) && (spawnedDemonio.Count < enemies[1].maxPerRound))//Demonio
            {
                GameObject enemy = Instantiate<GameObject>(enemies[1].enemyPrefab, spawnLocation[spawnIndex].position, Quaternion.identity);
                spawnedDemonio.Add(enemy);
                enemiesAlive++;
                totalSpwanedEnemies++;
                Currency -= enemies[i].cost;
                Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
            }
        }
    }
    private void roundStatus()
    {
        if(totalSpwanedEnemies>= maxPerWave)
        {
            canSpawn= false;
            increaseMaxPerRound();
        }
    }
    private void increaseMaxPerRound()
    {
        maxPerWave += 10;
        totalSpwanedEnemies = 0;
        currentRound++;
        for(int x = 0; x < enemies.Count; x++)
        {
            enemies[x].maxPerRound += 5;
        }
        StartCoroutine(roundFinish());
        ShowUpgradeMenu();
    }
    public void ShowUpgradeMenu()
    {
        Time.timeScale = 0f;
        UpGradeMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeGame()
    {
        UpGradeMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private IEnumerator roundFinish()
    {
        yield return new WaitForSeconds(5);
        canSpawn = true;
    }
    private void startMultiplyerTimer()
    {
        multiplyerTimer += Time.deltaTime;
        
        if (multiplyerTimer > 30f)
        {
            float addCurrencyMultiplyer = currencyMultiplyer;
            currencyMultiplyer = addCurrencyMultiplyer + 0.5f;
            resetMultiplyerTimer();
        }
    }
    private void resetMultiplyerTimer()
    {
        multiplyerTimer = 0;
    }

    public void substractEnemySaltarin()
    {
        if (spawnedSaltarin.Count >= 0)
        {
            spawnedSaltarin.RemoveAt(spawnedSaltarin.Count - 1);
            enemiesAlive--;
        }       
    }
    public void substractEnemyDemonio()
    {
        if (spawnedDemonio.Count >= 0)
        {
            spawnedDemonio.RemoveAt(spawnedDemonio.Count - 1);
            enemiesAlive--;
        }
    }

    private void XrayEnabler()
    {
        if(enemiesAlive <= 4)
        {
            //hacer bucle por cada tipo de enemigo
            for (int x = 0; x < spawnedSaltarin.Count; x++)
            {
                spawnedSaltarin[x].GetComponent<Enemy>().EnableLastAlive(true);
            }
            for (int z = 0; z < spawnedDemonio.Count; z++)
            {
                spawnedDemonio[z].GetComponent<Enemy>().EnableLastAlive(true);
            }
        }
        else
        {
            for (int x = 0; x < spawnedSaltarin.Count; x++)
            {
                spawnedSaltarin[x].GetComponent<Enemy>().EnableLastAlive(false);
            }
            for (int z = 0; z < spawnedDemonio.Count; z++)
            {
                spawnedDemonio[z].GetComponent<Enemy>().EnableLastAlive(false);
            }
        }
    }
}
