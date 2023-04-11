using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    public enum MonsterClass {Saltarin,Demonio,Tank};
    public MonsterClass currentClass;

    [Header("Health")]
    private float Health;
    public float currentHealth;

    [Header("Movement")]
    private float Speed;

    [Header("Damage")]
    private float attackDamage = 16f;
    public float recoil = 100f;

    public float SaltarinChargeForce;
    private float SaltarinDistance;
    public float SaltarinChargeDistance = 5f;
    private bool isCharging;
    private bool canCharge = true;

    public float damageDelay = 15.0f;

    private bool canDoDamage = false;   //demonio

   
    private int stateValue = 1;

    private NavMeshAgent nav;

    private Animator IAanim;

    private Rigidbody rb;

     
    private GameObject player;

    private GameObject IaSpawner;

    private GameObject Radar;    

    [Header("Sounds")]
    private AudioSource Asc;
    public AudioClip deathSound;
    public AudioClip bulletImpact;

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
                attackDamage = 16f;
                break;
            case MonsterClass.Demonio:
                Health = 100f;
                Speed = 2.5f;
                attackDamage = 20f;
                break;
            case MonsterClass.Tank:
                Health = 400f;
                Speed = 1.5f;
                attackDamage = 35f;
                break;
        }
        currentHealth = Health;
        IaSpawner = GameObject.FindGameObjectWithTag("Spawner");
        Xray.SetActive(false);
        Radar = GameObject.FindGameObjectWithTag("Radar");
        Asc = GetComponent<AudioSource>();
    }

    //Die
    public void TakeDamage(float damageAmount, float knockbackForce)
    {
        
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            stateValue = -1;
            Asc.PlayOneShot(deathSound);
        }

        switch (currentClass)
        {
            case MonsterClass.Demonio:
                IAanim.SetTrigger("isDamaged");
                break;
        }
        rb.AddForce(-transform.forward * knockbackForce, ForceMode.Impulse);
        Asc.PlayOneShot(bulletImpact);
    }
    public void doDamage()
    {        
        switch (currentClass)
        {
            case MonsterClass.Saltarin:
                rb.AddForce(-transform.forward * recoil, ForceMode.Impulse);
                player.GetComponent<Player>().takeDamage(attackDamage);
                break;
                case MonsterClass.Demonio:
                if(canDoDamage)
                    player.GetComponent<Player>().takeDamage(attackDamage);
                break;
            case MonsterClass.Tank:
                if (canDoDamage)
                    player.GetComponent<Player>().takeDamage(attackDamage);
                break;
        }
    }
    private void Update(){
        navStates();
        switch (currentClass)
        {
            case MonsterClass.Saltarin:
                if(!isCharging && canCharge)
                {
                    SaltarinDistance = CalcularSaltarinSDistancia();
                    SaltarinAttack();
                }                
                break;
        }
    }
    private void navStates()
    {
        if(stateValue == -1)
        {
            nav.enabled = false;
            nav.speed = 0;
            IAanim.SetTrigger("isDead");
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //this.gameObject.GetComponent<Collider>().enabled = false;
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
            rb.isKinematic = false;
            StartCoroutine(disableDeathDelayer());
        // GetComponent<Animator>().enabled = false;
       // https://www.youtube.com/watch?v=zjuI5Jdzjxo
            EnableRagdoll();

           
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
        if(stateValue == 2)//demonio y tanke
        {
            nav.enabled = false;
            nav.speed = 0;
            IAanim.SetTrigger("isAttacking");
            gameObject.GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(enableAttackCollider());            
        }
        if(stateValue == 3)//ataque saltarin
        {
            //////
            nav.enabled = false;
            nav.speed = 0;
            StartCoroutine(saltarinChargeAttack());
            stateValue = 0;
           
        }
        
    }
    private float CalcularSaltarinSDistancia()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        return distance;
    }

    private void SaltarinAttack()
    {
        if ((SaltarinDistance <= SaltarinChargeDistance) && !isCharging)
        {
            //GeneratePlayerTransform();
            stateValue = 3;
            isCharging = true;
            canCharge = false;
        }
    }
    /*private void GeneratePlayerTransform()
    {
        Transform PlayerTransform = player.transform;
        stateValue = 3;        
    }*/

    private IEnumerator saltarinChargeAttack() //para evitar dobles ataques
    {
        yield return new WaitForSeconds(1f);
        //rb.AddForce(transform.forward * SaltarinChargeForce, ForceMode.Impulse);
        rb.AddForce(transform.forward * SaltarinChargeForce, ForceMode.VelocityChange);
        StartCoroutine(MoveDelay(15f));  
    }
    private IEnumerator resetChargeSkill()
    {
        yield return new WaitForSeconds(10f);
        canCharge = true;
    }

        private IEnumerator enableAttackCollider() //para evitar dobles ataques
    {
        yield return new WaitForSeconds(5f);
        gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    private IEnumerator MoveDelay(float damageDelay)
    {
        yield return damageDelay;
        stateValue = 1;
        switch (currentClass)
        {
            case MonsterClass.Saltarin:
                isCharging = false;
                StartCoroutine(resetChargeSkill());
                break;            
        }
    }
    private IEnumerator disableDeathDelayer()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
        removeFromList();
    }


    private void EnableRagdoll()
    { 
    
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
            case MonsterClass.Tank:
                IaSpawner.GetComponent<IASpawner>().substractEnemyTank();
                break;
        }
        Radar.GetComponent<RadarController>().RemoveEnemy(this.gameObject.transform);
    }
    //Colisiones
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (currentClass)
            {
                case MonsterClass.Saltarin:
                    stateValue = 0;
                    doDamage();
                    StartCoroutine(MoveDelay(8f));
                    break;
            }           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       /* if (other.gameObject.tag == "Sword")
        {
            TakeDamage(100, 25);
        }*/
        if (other.gameObject.tag == "Player")
        {
            switch (currentClass)
            {
                case MonsterClass.Demonio:
                    stateValue = 2;
                    break;
                case MonsterClass.Tank:
                    stateValue = 2;
                    break;
            }
            StartCoroutine(MoveDelay(15f));
            canDoDamage = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canDoDamage = false;
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
