using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IASpawnerV2 : MonoBehaviour
{
    [System.Serializable]
    public class EnemyL
    {
        public GameObject enemyPrefab;
        public int maxPerRound;
    }
    [Header("List of Enemies")]
    public List<EnemyL> enemies = new List<EnemyL>();


   
    private int spawnTimerBetweenWaves;
   

    [Header("Rounds")]
    public int currentRound = 1;
    public TextMeshProUGUI currentRoundText;
    public TextMeshProUGUI enemiesLeftText;
    public List<GameObject> spawnedSaltarin = new List<GameObject>();
    public List<GameObject> spawnedDemonio = new List<GameObject>();
    public List<GameObject> spawnedTank = new List<GameObject>();
    public int maxPerWave;
    public int enemiesAlive = 0;
    public int totalSpawnedEnemies;

    private GameObject Player;

    [Header("UI")]
    public GameObject UpGradeMenu;

    private GameObject Radar;


    [Header("SpawnRelated")]
    public GameObject SpawnEffect;
    public BoxCollider spawnLocationS;

    private GameObject pm;

    private GameManager gameManager;

    void Start()
    {
        EnableWaveSpawner();
        //SpawnEnemy();       
        maxPerWave = 10;
        UpGradeMenu.SetActive(false);
        Player = GameObject.FindGameObjectWithTag("Player");
        Radar = GameObject.FindGameObjectWithTag("Radar");
        pm = GameObject.FindGameObjectWithTag("MenuManager");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void EnableWaveSpawner()
    {
        InvokeRepeating("SpawnEnemy", 5f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {       

        //if (canSpawn)
        //{
            //SpawnEnemy();
        //}
                
        currentRoundText.SetText("Round: " + currentRound);
        countEnemiesAlive();
        enemiesLeftText.SetText("Enemies alive: " + enemiesAlive);

        //startMultiplyerTimer();
        roundStatus();
        //XrayEnabler();
       
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {          
                switch (enemies[i].enemyPrefab.name)
                {//Demonio
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
                           
                            totalSpawnedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    case "DemonioV2":
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

                            totalSpawnedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    case "DemonioV3":
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

                            totalSpawnedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    //Saltarin
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

                                totalSpawnedEnemies++;
                                //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                                Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                                GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                                spawnParticle.GetComponent<ParticleSystem>().Play();
                                Destroy(spawnParticle, 2f);
                            }
                            break;
                    case "SaltarinV2":
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

                            totalSpawnedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    case "SaltarinV3":
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

                            totalSpawnedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    //Tank
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
                            
                                    totalSpawnedEnemies++;
                                    //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                                    Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                                    GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                                    spawnParticle.GetComponent<ParticleSystem>().Play();
                                    Destroy(spawnParticle, 2f);
                                }
                                break;
                    case "TankV2":
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

                            totalSpawnedEnemies++;
                            //Radar.GetComponent<RadarController>().AddEnemy(enemy.gameObject.transform);
                            Radar.GetComponent<NewRadar>().AddEnemy(enemy.gameObject.transform);

                            GameObject spawnParticle = Instantiate(SpawnEffect, spawnPosition, Quaternion.identity);
                            spawnParticle.GetComponent<ParticleSystem>().Play();
                            Destroy(spawnParticle, 2f);
                        }
                        break;
                    case "TankV3":
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

                            totalSpawnedEnemies++;
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
   /* private Vector3 GetRandomSpawnPoint(Vector3 center, float radius, float yLevel, float objectScale)
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
    }*/
    private void countEnemiesAlive()
    {
        enemiesAlive = spawnedDemonio.Count + spawnedSaltarin.Count + spawnedTank.Count;
    }
    private void roundStatus()
    {
        if (totalSpawnedEnemies >= maxPerWave)
        {
            CancelInvoke();
            
            if (enemiesAlive < 1)
            {
                if(currentRound<21)
                    increaseMaxPerRound();
                else
                    EndGame();
                enemiesAlive = 0;
            }
        }
    }
   
    /*public void ShowUpgradeMenu()
    {
        Time.timeScale = 0f;
        UpGradeMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeGame()
    {
        /*if (Time.timeScale == 0.0f)
        {
            UpGradeMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
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
        gameManager.ResetVar();
        Destroy(gameManager.gameObject);
        SceneManager.LoadScene(0);
    }

    private void increaseMaxPerRound()
    {
        if (currentRound <= 20)
        {
            totalSpawnedEnemies = 0;
            currentRound++;
            /*
            0 = dv1
            1 = dv2
            2 = dv3
            3 = sv1
            4 = sv2
            5 = sv3
            6 = tv1
            7 = tv2
            8 = tv3
            */
            switch (currentRound)
            {
                case 1:
                    maxPerWave = 10;
                    enemies[0].maxPerRound = 6;
                    enemies[3].maxPerRound = 4;
                    break;
                case 2:
                    maxPerWave = 16;
                    enemies[0].maxPerRound = 8;
                    enemies[3].maxPerRound = 6;
                    enemies[6].maxPerRound = 2;
                    break;
                case 3:
                    maxPerWave = 22;
                    enemies[0].maxPerRound = 10;
                    enemies[3].maxPerRound = 8;
                    enemies[6].maxPerRound = 4;
                    break;
                case 4:
                    maxPerWave = 28;
                    enemies[0].maxPerRound = 8;
                    enemies[3].maxPerRound = 10;
                    enemies[6].maxPerRound = 6;
                    enemies[1].maxPerRound = 4;
                    break;
                case 5:
                    maxPerWave = 34;
                    enemies[0].maxPerRound = 6;
                    enemies[3].maxPerRound = 12;
                    enemies[6].maxPerRound = 8;
                    enemies[1].maxPerRound = 8;
                    break;
                case 6:
                    maxPerWave = 40;
                    enemies[0].maxPerRound = 4;
                    enemies[3].maxPerRound = 14;
                    enemies[6].maxPerRound = 10;
                    enemies[1].maxPerRound = 12;
                    break;
                case 7:
                    maxPerWave = 46;
                    enemies[0].maxPerRound = 2;
                    enemies[3].maxPerRound = 12;
                    enemies[6].maxPerRound = 12;
                    enemies[1].maxPerRound = 16;
                    enemies[4].maxPerRound = 4;
                    break;
                case 8:
                    maxPerWave = 52;
                    enemies[3].maxPerRound = 10;
                    enemies[6].maxPerRound = 14;
                    enemies[1].maxPerRound = 20;
                    enemies[4].maxPerRound = 8;
                    break;
                case 9:
                    maxPerWave = 58;
                    enemies[3].maxPerRound = 8;
                    enemies[6].maxPerRound = 16;
                    enemies[1].maxPerRound = 22;
                    enemies[4].maxPerRound = 12;
                    break;
                case 10:
                    maxPerWave = 64;
                    enemies[3].maxPerRound = 6;
                    enemies[6].maxPerRound = 14;
                    enemies[1].maxPerRound = 24;
                    enemies[4].maxPerRound = 16;
                    enemies[7].maxPerRound = 4;
                    break;
                case 11:
                    maxPerWave = 70;
                    enemies[3].maxPerRound = 4;
                    enemies[6].maxPerRound = 12;
                    enemies[1].maxPerRound = 26;
                    enemies[4].maxPerRound = 20;
                    enemies[7].maxPerRound = 8;
                    break;
                case 12:
                    maxPerWave = 76;
                    enemies[3].maxPerRound = 2;
                    enemies[6].maxPerRound = 10;
                    enemies[1].maxPerRound = 28;
                    enemies[4].maxPerRound = 24;
                    enemies[7].maxPerRound = 12;
                    break;
                case 13:
                    maxPerWave = 82;
                    enemies[6].maxPerRound = 8;
                    enemies[1].maxPerRound = 26;
                    enemies[4].maxPerRound = 28;
                    enemies[7].maxPerRound = 16;
                    enemies[2].maxPerRound = 4;
                    break;
                case 14:
                    maxPerWave = 88;
                    enemies[6].maxPerRound = 6;
                    enemies[1].maxPerRound = 24;
                    enemies[4].maxPerRound = 30;
                    enemies[7].maxPerRound = 20;
                    enemies[2].maxPerRound = 8;
                    break;
                case 15:
                    maxPerWave = 94;
                    enemies[6].maxPerRound = 4;
                    enemies[1].maxPerRound = 22;
                    enemies[4].maxPerRound = 32;
                    enemies[7].maxPerRound = 24;
                    enemies[2].maxPerRound = 12;
                    break;
                case 16:
                    maxPerWave = 100;
                    enemies[6].maxPerRound = 2;
                    enemies[1].maxPerRound = 20;
                    enemies[4].maxPerRound = 30;
                    enemies[7].maxPerRound = 28;
                    enemies[2].maxPerRound = 16;
                    enemies[5].maxPerRound = 4;
                    break;
                case 17:
                    maxPerWave = 106;
                    enemies[1].maxPerRound = 18;
                    enemies[4].maxPerRound = 28;
                    enemies[7].maxPerRound = 32;
                    enemies[2].maxPerRound = 20;
                    enemies[5].maxPerRound = 8;
                    break;
                case 18:
                    maxPerWave = 112;
                    enemies[1].maxPerRound = 16;
                    enemies[4].maxPerRound = 26;
                    enemies[7].maxPerRound = 34;
                    enemies[2].maxPerRound = 24;
                    enemies[5].maxPerRound = 12;
                    break;
                case 19:
                    maxPerWave = 118;
                    enemies[1].maxPerRound = 14;
                    enemies[4].maxPerRound = 24;
                    enemies[7].maxPerRound = 32;
                    enemies[2].maxPerRound = 28;
                    enemies[5].maxPerRound = 16;
                    enemies[8].maxPerRound = 4;
                    break;
                case 20:
                    maxPerWave = 124;
                    enemies[1].maxPerRound = 12;
                    enemies[4].maxPerRound = 22;
                    enemies[7].maxPerRound = 30;
                    enemies[2].maxPerRound = 32;
                    enemies[5].maxPerRound = 20;
                    enemies[8].maxPerRound = 8;
                    break;
               case 21:
                    EndGame();
                    break;
            }


            //StartCoroutine(roundFinish());

            pm.GetComponent<PauseMenu>().ShowUpgradeMenu();

            EnableWaveSpawner();
        }    
        else
        {
            EndGame();
        }

    }

}
