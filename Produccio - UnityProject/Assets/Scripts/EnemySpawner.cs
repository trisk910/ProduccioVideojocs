using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  [SerializeField]
     private GameObject enemyPrefab;
     private int spawnIndex;
     private Transform[] spawnpoints;
     private Vector3 spawnPos;
     private int count;
     
     void Start(){
         count = transform.childCount;
         spawnpoints = new Transform[count];
         for(int i = 0; i < count; i++){
             spawnpoints[i] = transform.GetChild(i);
         }
         
         InvokeRepeating("spawnEnemys", 1, 5);
     }
     
     public void spawnEnemys(){
        spawnIndex = Random.Range(0, count);
         
        Instantiate(enemyPrefab, spawnpoints[spawnIndex].position, enemyPrefab.transform.rotation);
     }
}
