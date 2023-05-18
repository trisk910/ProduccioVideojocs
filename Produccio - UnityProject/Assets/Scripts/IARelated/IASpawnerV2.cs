using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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

    void Start()
    {
       

        EnableWaveSpawner();
        //SpawnEnemy();       
        maxPerWave = 10;
        UpGradeMenu.SetActive(false);
        Player = GameObject.FindGameObjectWithTag("Player");
        Radar = GameObject.FindGameObjectWithTag("Radar");
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
        if (totalSpawnedEnemies >= maxPerWave)
        {
            CancelInvoke();
            
            if (enemiesAlive < 1)
            {
                increaseMaxPerRound();
                enemiesAlive = 0;
            }
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
   /* private IEnumerator roundFinish()
    {
        yield return new WaitForSeconds(5);
        canSpawn = true;
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

    private void increaseMaxPerRound()
    {
        if (currentRound <= 20)
        {
            maxPerWave += 6;
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
            if (currentRound <= 3)//[0:3]
            {
                enemies[0].maxPerRound =+ 2;//demoniov1
                enemies[3].maxPerRound =+ 2;//saltarinv1
                enemies[6].maxPerRound =+ 2;//tankev1
            }
            if(currentRound >= 4 && currentRound <= 8 )//[4:8]
            {
                enemies[0].maxPerRound =- 2;//dv1
                if(currentRound <= 6)
                    enemies[3].maxPerRound =+ 2;//sv1
                else
                    enemies[3].maxPerRound =- 2;
                enemies[6].maxPerRound =+ 2; //tv1

               enemies[1].maxPerRound =+ 4; //dv2

                if(currentRound >=7)
                    enemies[4].maxPerRound =+ 4; //sv2

            }
            if (currentRound >= 9 && currentRound <= 13)//[9:13]
            {
                enemies[3].maxPerRound =- 2; //sv1
                if(currentRound == 9)
                    enemies[6].maxPerRound =+ 2; //tv1
                else
                    enemies[6].maxPerRound =- 2; //tv1
                if(currentRound >= 9 && currentRound <= 12)
                    enemies[1].maxPerRound =+ 2; //dv2
                else
                    enemies[1].maxPerRound =- 2; //dv2

                enemies[4].maxPerRound =+ 4; //sv2
                if(currentRound >= 10)
                    enemies[7].maxPerRound =+ 4; //tv2
                if(currentRound == 13)
                    enemies[2].maxPerRound =+ 4; //dv3
            }
            if (currentRound >= 14 && currentRound <= 18)//[14:18]
            {
                if (currentRound <= 17)
                {
                    enemies[6].maxPerRound =- 2; //tv1
                    enemies[7].maxPerRound =+ 4; //tv2
                }
                enemies[1].maxPerRound =- 2; //dv2
                if (currentRound <= 15)
                    enemies[4].maxPerRound =+ 2 ; //sv2
                else
                    enemies[4].maxPerRound =- 2; //sv2
                if (currentRound == 18)
                    enemies[7].maxPerRound =+ 2; //tv2
                enemies[2].maxPerRound =+ 4; //dv3
                if(currentRound >= 16)
                    enemies[5].maxPerRound =+ 4; //sv3

            }
            if(currentRound >= 19 && currentRound <= 20)
            {
                enemies[1].maxPerRound =- 2; //dv2
                enemies[4].maxPerRound =- 2; //sv2
                enemies[7].maxPerRound =- 2; //tv2
                enemies[2].maxPerRound =+ 4; //dv3
                enemies[5].maxPerRound =+  4; //sv3
                enemies[8].maxPerRound = +4; //tv3
            }


                //StartCoroutine(roundFinish());

                ShowUpgradeMenu();
            EnableWaveSpawner();
        }
        else
        {
            EndGame();
        }

    }

}
