using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    public enum MonsterClass { Saltarin};
    public MonsterClass currentClass;

    [Header("Health")]
    private float Health;
    private float currentHealth;

    [Header("Movement")]
    private float Speed;

    [Header("Damage")]
    public float attackDamage = 16f;
    public float recoil = 5000f;    

    public float damageDelay = 15.0f;
    

    [Header("IA Status")]
    private int stateValue = 1;

    private NavMeshAgent nav;

    private Animator IAanim;

    private Rigidbody rb;

     
    private GameObject player;

    private GameObject IaSpawner;
    private void Start(){
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        IAanim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        switch (currentClass)
        {
            case MonsterClass.Saltarin:
                Health = 50f;
                Speed = 3.5f;
                break;
        }
        currentHealth = Health;
        IaSpawner = GameObject.FindGameObjectWithTag("Spawner");
    }

    //Die
    public void TakeDamage(float damageAmount, float knockbackForce)
    {
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            stateValue = -1;           
        }
       
        if (rb != null)
        {
            rb.AddForce(-transform.forward * knockbackForce, ForceMode.Impulse);
        }
    }
    private void doDamage()
    {        
       player.GetComponent<Player>().takeDamage(attackDamage);
       rb.AddForce(-transform.forward * 20f, ForceMode.Impulse);

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
            this.gameObject.GetComponent<Rigidbody>().isKinematic= true;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            
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
            nav.speed = Speed;
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stateValue = 0;
            doDamage();
           /* this.GetComponent<Rigidbody>().AddForce(-transform.forward * recoil);
            this.GetComponent<Rigidbody>().AddForce(0,recoil,0);*/
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
        removeFromList();
    }

    private void removeFromList()
    {
        switch (currentClass)
        {
            case MonsterClass.Saltarin:
                IaSpawner.GetComponent<IASpawner>().substractEnemySaltarin();
                break;
        }
    }
}
