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
    
    //public List<GameObject> enemiesToSpawn = new List<GameObject>();

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


    void Start()
    {
        canSpawn = true;
        SpawnEnemy();
        InitialCurrency = 25f;
        Currency = InitialCurrency;
        currencyMultiplyer = .05f;
    }

    // Update is called once per frame
    void Update()
    {
        //spawnIndex = Random.Range(0, count);
        Currency += currencyMultiplyer * (Time.deltaTime);
        if(canSpawn)
            SpawnEnemy();
        currentRoundText.SetText("Round: " + currentRound);
        enemiesLeftText.SetText("Enemies left: " + enemies.Count);
    
        startMultiplyerTimer();
        
    }

    void SpawnEnemy()
    {
        for(int i = 0; i < enemies.Count; i++) {
            spawnIndex = Random.Range(0, spawnLocation.Length);
         
            if ((Currency >= enemies[i].cost) && (spawnedSaltarin.Count < enemies[0].maxPerRound) )//saltarin
            {
                GameObject enemy = Instantiate<GameObject>(enemies[0].enemyPrefab, spawnLocation[spawnIndex].position, Quaternion.identity);
                spawnedSaltarin.Add(enemy);
                Currency -= enemies[i].cost;
            }
           
        }
    }

    private IEnumerator SpawnerDelay()
    {
        yield return new WaitForSeconds(10);
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
        int saltarinSize = spawnedSaltarin.Count() - 1;
        if(saltarinSize >= 0)
        {
            GameObject lastGameObject = spawnedSaltarin[saltarinSize];
            spawnedSaltarin.Remove(lastGameObject);
            Destroy(lastGameObject);
        }
       
    }
}
