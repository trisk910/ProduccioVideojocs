using System.Collections;
using System.Collections.Generic;
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
    public float recoil;
    public float moveDelaySaltarin = 3f;

    public float SaltarinChargeForce;
    private float SaltarinDistance;
    public float SaltarinChargeDistance = 5f;
    private bool isCharging;
    private bool canCharge = true;

    public float damageDelay = 15.0f;

    private bool canDoDamage = false;   //demonio y tanke

   
    private int stateValue = -2;

    private NavMeshAgent nav;

    private Animator IAanim;

    private Rigidbody rb;
    private Rigidbody[] rbs;//


    private GameObject player;

    private GameObject IaSpawner;

    private GameObject Radar;    

    [Header("Sounds")]
    private AudioSource Asc;
    public AudioClip deathSound;
    public AudioClip bulletImpact;

    [Header("Ui")]
    public GameObject Xray;



    private int targetLayer;

    private void Start(){
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        IAanim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rbs = transform.GetComponentsInChildren<Rigidbody>();//
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
                StartCoroutine(MoveDelay(moveDelaySaltarin));
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
    public void ChargeDamage()//Saltarin
    {
        player.GetComponent<Player>().takeDamage(attackDamage);
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
        if (stateValue == -2)//Spawn
        {
            nav.enabled = false;
            nav.speed = 0;

            switch (currentClass)
            {
                case MonsterClass.Saltarin:
                    StartCoroutine(EnableEnemyRespawn(0f));
                    break;
                case MonsterClass.Demonio:
                    StartCoroutine(EnableEnemyRespawn(2.2f));
                    break;
                case MonsterClass.Tank:
                    StartCoroutine(EnableEnemyRespawn(2f));
                    break;
            }
            
        }
            if (stateValue == -1)//Death
        {
            nav.enabled = false;
            nav.speed = 0;
            switch (currentClass)
            {
                case MonsterClass.Saltarin:
                    IaSpawner.GetComponent<IASpawner>().substractEnemySaltarin();
                    IAanim.SetTrigger("isDead");
                   /* this.gameObject.GetComponent<Collider>().enabled = false;
                    foreach (Collider c in GetComponentsInChildren<Collider>())
                    {
                        c.enabled = false;
                    }*/
                    this.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
                    this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    break;

                case MonsterClass.Demonio:
                    BipedDeath();
                    break;

                case MonsterClass.Tank:
                    BipedDeath();
                    break;
            }
                   

            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
    private IEnumerator EnableEnemyRespawn(float timer) //EnableEenmy after respawn
    {
        yield return new WaitForSeconds(timer);
        stateValue = 1;
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
            stateValue = 3;
            isCharging = true;
            canCharge = false;
        }
    }
   

    private IEnumerator saltarinChargeAttack() //para evitar dobles ataques
    {
        yield return new WaitForSeconds(1f);
        //rb.AddForce(transform.forward * SaltarinChargeForce, ForceMode.Impulse);
        rb.AddForce(transform.forward * SaltarinChargeForce, ForceMode.VelocityChange);
        this.gameObject.layer = 8;
        StartCoroutine(MoveDelay(moveDelaySaltarin));  
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

    private IEnumerator MoveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        stateValue = 1;
        switch (currentClass)
        {
            case MonsterClass.Saltarin:
                isCharging = false;
                StartCoroutine(resetChargeSkill());
                this.gameObject.layer = 0;
                break;            
        }
    }
    private IEnumerator disableDeathDelayer()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
        removeFromList();
    }

    private void BipedDeath()
    {
        targetLayer = LayerMask.NameToLayer("IgnorePlayer");
        // Set the layer recursively
        SetLayerRecursively(gameObject, targetLayer);
        rb.isKinematic = false;


        GetComponent<Animator>().enabled = false;
        // https://www.youtube.com/watch?v=zjuI5Jdzjxo
        EnableRagdoll();
        //https://www.youtube.com/watch?v=sCcerKKQhsQ
    }

    private void EnableRagdoll()
    { 
        foreach(Rigidbody rigidbody in rbs)
        {
            rigidbody.isKinematic = false;
            rigidbody.drag = 1.0f;
        }

        //this.GetComponent<Rigidbody>().isKinematic = false;
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
                    StartCoroutine(MoveDelay(moveDelaySaltarin));
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
            StartCoroutine(MoveDelay(3f));
            canDoDamage = true;
        }
        if (other.gameObject.tag == "TriggerDamage")
        {
            switch (currentClass)
            {
                case MonsterClass.Saltarin:
                    ChargeDamage();
                    break;
            }
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


    //Layers Collisions
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
