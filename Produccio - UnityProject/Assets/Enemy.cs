using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //The box's current health point total
    public float currentHealth = 3;
    public float movementSpeed = 5.0f;

     public NavMeshAgent nav;

     //public Transform target;
    private GameObject player;
    private void Start(){
         nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
         player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Damage(float damageAmount)
    {
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            //if health has fallen below zero, deactivate it 
            gameObject.SetActive(false);
        }
    }
    private void Update(){
        nav.SetDestination(player.transform.position);
    }
}