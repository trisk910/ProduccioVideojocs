using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    public enum MonsterClass {Saltarin,Demonio};
    public MonsterClass currentClass;

    [Header("Health")]
    private float Health;
    public float currentHealth;

    [Header("Movement")]
    private float Speed;

    [Header("Damage")]
    public float attackDamage = 16f;
    public float recoil = 5000f;    

    public float damageDelay = 15.0f;

    public bool attacking = false;   

   
    private int stateValue = 1;

    private NavMeshAgent nav;

    private Animator IAanim;

    private Rigidbody rb;

     
    private GameObject player;

    private GameObject IaSpawner;

    [Header("Ui")]
    public GameObject Xray;
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
            case MonsterClass.Demonio:
                Health = 100f;
                Speed = 2.5f;
                break;
        }
        currentHealth = Health;
        IaSpawner = GameObject.FindGameObjectWithTag("Spawner");
        Xray.SetActive(false);
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

        switch (currentClass)
        {
            case MonsterClass.Demonio:
                IAanim.SetTrigger("isDamaged");
                break;
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
            IAanim.SetTrigger("isDead");
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
        if(stateValue == 2)
        {
            nav.enabled = false;
            nav.speed = 0;
            IAanim.SetTrigger("isAttacking");
            attacking = true;
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (currentClass)
            {
                case MonsterClass.Saltarin:
                    stateValue = 0;
                    doDamage();
                    break;
                case MonsterClass.Demonio:
                    stateValue = 2; 
                    break;
            }          
            StartCoroutine(MoveDelay(5f));
        }
    }
    private IEnumerator MoveDelay(float damageDelay)
    {
        yield return damageDelay;
        stateValue = 1;
    }
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
            case MonsterClass.Demonio:
                IaSpawner.GetComponent<IASpawner>().substractEnemyDemonio();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            TakeDamage(100, 10);
            rb.AddForce(-transform.forward * 20f, ForceMode.Impulse);
        }
        if (other.gameObject.tag == "Player")
        {
            if (attacking)
            {
                doDamage();
                attacking = false;
            }
        }
    }

    public void EnableLastAlive(bool activate)
    {
        if(activate)
            Xray.SetActive(true);
        else
            Xray.SetActive(false);
    }
}
