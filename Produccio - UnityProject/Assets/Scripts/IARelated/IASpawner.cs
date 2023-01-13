using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IASpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyL
    {
        public GameObject enemyPrefab;
        public int cost;
    }

    public List<EnemyL> enemies = new List<EnemyL>();
    public int currWave;
    private int waveValue;
    //public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    private int spawnIndex;

    /*public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;*/

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private bool canSpawn;

    private int spawnTimerBetweenWaves;

    public float Currency;
    public float InitialCurrency;
    public float currencyMultiplyer;
    private float initialCurrencyMultiplier;

    private float multiplyerTimer;

    void Start()
    {
        canSpawn = true;
        SpawnEnemy();
        InitialCurrency = 25f;
        Currency = InitialCurrency;
        currencyMultiplyer = .05f;
        initialCurrencyMultiplier = currencyMultiplyer;
        //StartCoroutine(currencyMultiplyerTimer());
        
    }

    // Update is called once per frame
    void Update()
    {
        //spawnIndex = Random.Range(0, count);
        Currency += currencyMultiplyer * (Time.deltaTime);
        if(canSpawn)
            SpawnEnemy();


        startMultiplyerTimer();
        //Debug.Log("Timer: " + multiplyerTimer);
    }

    void SpawnEnemy()
    {
        for(int i = 0; i < enemies.Count; i++) {
            spawnIndex = Random.Range(0, spawnLocation.Length);
            if (Currency >= enemies[i].cost) {
                GameObject enemy = Instantiate<GameObject>(enemies[0].enemyPrefab, spawnLocation[spawnIndex].position, Quaternion.identity);
                //enemiesToSpawn.RemoveAt(0);
                spawnedEnemies.Add(enemy); 
                Currency -= enemies[i].cost;
            }
        }
    }

    private IEnumerator SpawnerDelay()
    {
        yield return new WaitForSeconds(3);
    }
    /*private IEnumerator currencyMultiplyerTimer()
    {
        yield return new WaitForSeconds(30);
        float addCurrencyMultiplyer = currencyMultiplyer;
        currencyMultiplyer = addCurrencyMultiplyer +  0.5f;
        //Debug.Log("CurrencyMultiplyer: " + currencyMultiplyer);
    }*/

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
}
