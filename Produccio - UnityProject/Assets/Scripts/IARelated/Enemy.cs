using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    //The box's current health point total
    public float currentHealth = 5f;
    public float movementSpeed = 5.0f;
    public float attackDamage = 16f;
    public float recoil = 5000f;

    private float initialSpeed = 3.5f;

    public float damageDelay = 15.0f;
    private bool canDoDamage = true;

    private int stateValue = 1;

    private NavMeshAgent nav;

    private Animator IAanim;

    private PlayerEvents playerEvents;
     
    private GameObject player;
    private void Start(){
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        IAanim = GetComponent<Animator>();


    }

    //Die
    public void Damage(float damageAmount)
    {
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            /* nav.speed = 0;
             nav.SetDestination(transform.position);
             IAanim.SetBool("isDead", true);
            // IAanim.SetBool("isDead", false);

             StartCoroutine(disableDeathDelayer());
             //if health has fallen below zero, deactivate it */
            stateValue = -1;
           
        }
    }
    private void doDamage()
    {
        //if (canDoDamage)
        //{
            player.GetComponent<PlayerEvents>().takeDamage(attackDamage);
        //    StartCoroutine(DamageDelayer(damageDelay));
         //   canDoDamage = false;
       // }
    }
    private void Update(){
        navStates();
    }
    private void navStates()
    {
        if(stateValue == -1)
        {
            nav.enabled = false;
            nav.speed = 0;
            //nav.SetDestination(transform.position);
            IAanim.SetBool("isDead", true);
            StartCoroutine(disableDeathDelayer());
        }
        if (stateValue == 0)
        {
            nav.enabled = false;
            nav.speed = 0;
        }
        if (stateValue == 1)
        {
            nav.enabled = true;
            nav.SetDestination(player.transform.position);
            nav.speed = initialSpeed;
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stateValue = 0;
            doDamage();
            Debug.Log("TOuch");
            this.GetComponent<Rigidbody>().AddForce(-transform.forward * recoil);
            this.GetComponent<Rigidbody>().AddForce(0,recoil,0);
            StartCoroutine(MoveDelay(4f));
            //player.GetComponent<Rigidbody>().AddForce(-transform.forward * recoil);

        }
    }
    private IEnumerator MoveDelay(float damageDelay)
    {
        yield return damageDelay;
        stateValue = 1;
    }
    /*private IEnumerator DamageDelayer(float damageDelay)
    {
        yield return damageDelay;
        stateValue = 1;
        canDoDamage = true;
    }*/
    private IEnumerator disableDeathDelayer()
    {
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }
}
