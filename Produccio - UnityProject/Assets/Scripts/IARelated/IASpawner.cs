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
    public BoxCollider spawnLocationS;


    private bool canSpawn;

    private int spawnTimerBetweenWaves;
    [Header("Spawner Currency")]
    public float Currency;
    public float InitialCurrency;
    public float currencyMultiplyer;  

    //private float multiplyerTimer;

    [Header("Rounds")]
    public int currentRound = 1;
    public TextMeshProUGUI currentRoundText;
    public TextMeshProUGUI enemiesLeftText;
    public List<GameObject> spawnedSaltarin = new List<GameObject>();
    public List<GameObject> spawnedDemonio = new List<GameObject>();
    public List<GameObject> spawnedTank = new List<GameObject>();
    public int maxPerWave;
    public int enemiesAlive = 0;
    public int totalSpwanedEnemies;

    private GameObject Player;

    [Header("UI")]
    public GameObject UpGradeMenu;

    private GameObject Radar;


    [Header("ParticleEffect")]
    public GameObject SpawnEffect;

    void Start()
    {
        canSpawn = true;
        SpawnEnemy();
        InitialCurrency = 25f;
        Currency = InitialCurrency;
        currencyMultiplyer = 1.5f;
        maxPerWave = 10;
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
        countEnemiesAlive();
        enemiesLeftText.SetText("Enemies alive: " + enemiesAlive);
    
        //startMultiplyerTimer();
        roundStatus();
        //XrayEnabler();
        addCurrencyShortCut();
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Currency >= enemies[i].cost)
            {
                switch (enemies[i].enemyPrefab.name)
                {
                    case "SaltarinV1":
                        if (spawnedSaltarin.Count < enemies[i].maxPerRound)
                        {
                            BoxCollider spawnArea = spawnLocationS.GetComponent<BoxCollider>();
                            Vector3 spawnPosition = new Vector3(
                                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                                transform.position.y,
                                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
                            );
                            GameObject enemy = Instantiate<GameObject>(enemies[i].enemyPrefab, spawnPosition, Quaternion.identity);
                            spawnedSaltarin.Add(enemy);
                            Currency -= enemies[i].cost;
                            totalSpwanedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    case "DemonioV1":
                        if (spawnedDemonio.Count < enemies[i].maxPerRound)
                        {
                            BoxCollider spawnArea = spawnLocationS.GetComponent<BoxCollider>();
                            Vector3 spawnPosition = new Vector3(
                                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                                transform.position.y,
                                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
                            );
                            GameObject enemy = Instantiate<GameObject>(enemies[i].enemyPrefab, spawnPosition, Quaternion.identity);
                            spawnedDemonio.Add(enemy);
                            Currency -= enemies[i].cost;
                            totalSpwanedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    case "TankV1":
                        if (spawnedTank.Count < enemies[i].maxPerRound)
                        {
                            BoxCollider spawnArea = spawnLocationS.GetComponent<BoxCollider>();
                            Vector3 spawnPosition = new Vector3(
                                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                                transform.position.y,
                                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
                            );
                            GameObject enemy = Instantiate<GameObject>(enemies[i].enemyPrefab, spawnPosition, Quaternion.identity);
                            spawnedTank.Add(enemy);
                            Currency -= enemies[i].cost;
                            totalSpwanedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    default:
                        Debug.LogWarning("Unknown enemy type: " + enemies[i].enemyPrefab.name);
                        break;
                }
            }
        }
    }
    private Vector3 GetRandomSpawnPoint(Vector3 center, float radius, float yLevel, float objectScale)
    {
        Vector3 spawnPoint = center + Random.insideUnitSphere * radius;
        spawnPoint.y = yLevel;

        Collider[] hitColliders = Physics.OverlapSphere(spawnPoint, objectScale);
        int attempts = 0;
        while (hitColliders.Length > 0 && attempts < 20)
        {
            spawnPoint = center + Random.insideUnitSphere * radius;
            spawnPoint.y = yLevel;
            hitColliders = Physics.OverlapSphere(spawnPoint, objectScale);
           // attempts++;
        }

        if (attempts == 10)
        {
            Debug.LogWarning("Could not find a suitable spawn point after 10 attempts.");
        }

        return spawnPoint;
    }
    private void countEnemiesAlive()
    {
        enemiesAlive = spawnedDemonio.Count + spawnedSaltarin.Count + spawnedTank.Count;
    }
    private void roundStatus()
    {
        if(totalSpwanedEnemies>= maxPerWave)
        {
            canSpawn= false;
            if (enemiesAlive < 1)
            {
                increaseMaxPerRound();
                enemiesAlive = 0;
            }
        }
    }
    private void increaseMaxPerRound()
    {
        if (currentRound <= 20)
        {
            maxPerWave += 6;
            //maxPerWave += 12;
            totalSpwanedEnemies = 0;
            currentRound++;
            Currency = 0;
            for (int x = 0; x < enemies.Count; x++)
            {
                enemies[x].maxPerRound += 2;
            }
            /*enemies[0].maxPerRound =+ 3; //saltarin
            enemies[1].maxPerRound =+ 5; //demonio
            enemies[2].maxPerRound =+ 4; //tank*/

            float addCurrencyMultiplyer = currencyMultiplyer;
            currencyMultiplyer = addCurrencyMultiplyer + 0.5f;
            StartCoroutine(roundFinish());
            ShowUpgradeMenu();
        }
        else
        {
            EndGame();
        }
       
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
    /*private void startMultiplyerTimer()
    {
        if (canSpawn)
        {
            multiplyerTimer += Time.deltaTime;

            if (multiplyerTimer > 30f)
            {
                float addCurrencyMultiplyer = currencyMultiplyer;
                currencyMultiplyer = addCurrencyMultiplyer + 0.5f;
                resetMultiplyerTimer();
            }
        }
    }*/
    /*private void resetMultiplyerTimer()
    {
        multiplyerTimer = 0;
    }*/

    public void substractEnemySaltarin()
    {
        if (spawnedSaltarin.Count > 0)
        {
            spawnedSaltarin.RemoveAt(spawnedSaltarin.Count - 1);
            enemiesAlive--;
        }       
    }
    public void substractEnemyDemonio()
    {
        if (spawnedDemonio.Count > 0)
        {
            spawnedDemonio.RemoveAt(spawnedDemonio.Count - 1);
            enemiesAlive--;
        }
    }
    public void substractEnemyTank()
    {
        if (spawnedTank.Count > 0)
        {
            spawnedTank.RemoveAt(spawnedTank.Count - 1);
            enemiesAlive--;
        }
    }

    


    private void EndGame()
    {

    }

    //cheats
    private void addCurrencyShortCut()
    {
        if (Input.GetKeyUp(KeyCode.P))
            Currency += 10f;
    }
}
