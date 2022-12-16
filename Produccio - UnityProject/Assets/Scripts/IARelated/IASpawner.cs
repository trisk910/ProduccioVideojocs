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
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    /*public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;*/

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private bool canSpawn;

    private int spawnTimerBetweenWaves;

    public int Currency = 0;

    void Start()
    {
        canSpawn = true;
        GenerateFirstWave();
        Currency = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //spawnIndex = Random.Range(0, count);

    }

    void GenerateFirstWave()
    {
        GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnLocation.Length].position, Quaternion.identity);
        enemiesToSpawn.RemoveAt(0); 
        spawnedEnemies.Add(enemy);
    }

    private IEnumerator SpawnerDelay()
    {
        yield return new WaitForSeconds(3);
    }
}
