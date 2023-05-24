using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    //Tipo Enemigo
    public enum MonsterClass {Saltarin,Demonio,Tank};
    public MonsterClass currentClass;


    //Variaciones
    public enum RankTypeSaltarin {Null, V2, V3};
    public RankTypeSaltarin VersionSaltarin;

    public enum RankTypeDemonio { Null, V2, V3 };
    public RankTypeDemonio VersionDemonio;

    public enum RankTypeTanke { Null, V2, V3 };
    public RankTypeTanke VersionTanke;



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
    private float SaltarinChargeDistance;
    private bool isCharging;
    private bool canCharge = true;

    public float damageDelay = 15.0f;

    private bool canDoDamage = false;   //demonio y tanke
    private bool animAttackOnce;

   
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
    private bool HitCD = false;
    public AudioClip spawnDemon;
    public AudioClip spawnTanke;
    public AudioClip spawnSaltarin;

    [Header("Ui")]
    public GameObject Xray;


    private bool deathSoundOnce;
    private int targetLayer;

    private void Start(){
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        IAanim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rbs = transform.GetComponentsInChildren<Rigidbody>();//
        Asc = GetComponent<AudioSource>();
        switch (currentClass)
        {
            case MonsterClass.Saltarin:
                Health = 50f;
                Speed = 3.5f;
                attackDamage = 16f;
                SaltarinChargeDistance = 5f;
                Asc.PlayOneShot(spawnSaltarin);
                break;
            case MonsterClass.Demonio:
                Health = 100f;
                Speed = 2.5f;
                attackDamage = 20f;
                Asc.PlayOneShot(spawnDemon);
                break;
            case MonsterClass.Tank:
                Health = 400f;
                Speed = 1.5f;
                attackDamage = 35f;
                Asc.PlayOneShot(spawnTanke);
                break;
        }
        switch (VersionSaltarin)
        {
            case RankTypeSaltarin.V2:
                Health += 25f;
                //Speed += 1.15f;
                attackDamage += 4f;
                SaltarinChargeDistance = 7f;
                break;
            case RankTypeSaltarin.V3:
                Health += 35f;
                //Speed += 1.15f;
                attackDamage += 7f;
                SaltarinChargeDistance = 10f;
                break;
        }
        switch (VersionDemonio)
        {
            case RankTypeDemonio.V2:
                Health += 30f;
                Speed += 0.5f;
                attackDamage += 25f;
                break;
            case RankTypeDemonio.V3:
                Health += 40f;
                Speed += 1.0f;
                attackDamage += 30f;
                break;
        }
        switch (VersionTanke)
        {
            case RankTypeTanke.V2:
                Health += 200f;
                Speed -= 0.5f;
                attackDamage += 20f;
                break;
            case RankTypeTanke.V3:
                Health += 400f;
                Speed -= 1.0f;
                attackDamage += 65f;
                break;
        }
        currentHealth = Health;
        IaSpawner = GameObject.FindGameObjectWithTag("Spawner");
        Xray.SetActive(false);
        Radar = GameObject.FindGameObjectWithTag("Radar");
        
    }

    //Die
    public void TakeDamage(float damageAmount, float knockbackForce)
    {

        if (stateValue != -1)
        {
            if (currentHealth <= 0)
            {
                stateValue = -1;                
            }
            else
            {
                currentHealth -= damageAmount;
            }

            switch (currentClass)
            {
                case MonsterClass.Demonio:
                    IAanim.SetTrigger("isDamaged");
                    break;
            }
            rb.AddForce(-transform.forward * knockbackForce, ForceMode.Impulse);
            if (!HitCD)
            {
                Asc.PlayOneShot(bulletImpact);
                StartCoroutine(FixTakeDamageSound());
                HitCD = true;
            }
           
        }
    }
    private IEnumerator FixTakeDamageSound() 
    {
        yield return new WaitForSeconds(0.5f);
        HitCD= false;
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
        player.GetComponent<Player>().takeDamage(attackDamage * 1.2f);
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
            nav.speed = 0;
            nav.velocity = Vector3.zero;
            nav.enabled = false;
            
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
                    this.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
                    BipedDeath();
                    break;

                case MonsterClass.Tank:
                    this.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
                    BipedDeath();
                    break;
            }
            
            Radar.GetComponent<NewRadar>().RemoveEnemy(this.gameObject.transform);
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (!deathSoundOnce)
            {
                Asc.PlayOneShot(deathSound);
                deathSoundOnce = true;
            }
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
            if (!animAttackOnce)
            {
                IAanim.SetTrigger("isAttacking");
                animAttackOnce = true;
            }
            //gameObject.GetComponent<SphereCollider>().enabled = false;
            //StartCoroutine(enableAttackCollider());            
            StartCoroutine(enableAttackAnim());
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
        //this.gameObject.layer = 8;
       
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
    private IEnumerator enableAttackAnim() //para evitar dobles animaciones de ataques
    {
        yield return new WaitForSeconds(7f);
        animAttackOnce = false;
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
        removeFromList();
        gameObject.SetActive(false);
        
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
                //IaSpawner.GetComponent<IASpawner>().substractEnemySaltarin();
                IaSpawner.GetComponent<IASpawnerV2>().substractEnemySaltarin();
                break;
            case MonsterClass.Demonio:
                //IaSpawner.GetComponent<IASpawner>().substractEnemyDemonio();
                IaSpawner.GetComponent<IASpawnerV2>().substractEnemyDemonio();
                break;
            case MonsterClass.Tank:
                //IaSpawner.GetComponent<IASpawner>().substractEnemyTank();
                IaSpawner.GetComponent<IASpawnerV2>().substractEnemyTank();
                break;
        }
        
        //Radar.GetComponent<RadarController>().RemoveEnemy(this.gameObject.transform);
        
    }
    //Colisiones
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (currentClass)
            {
                case MonsterClass.Saltarin:
                    if (stateValue != -1)
                    {
                        stateValue = 0;
                        doDamage();
                        StartCoroutine(MoveDelay(moveDelaySaltarin));
                    }
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
            //Debug.Log("BIPED: Can Attack");
            canDoDamage = true;
        }
        if (other.gameObject.tag == "TriggerDamage")
        {
            switch (currentClass)
            {
                case MonsterClass.Saltarin:
                    //ChargeDamage();
                    if(isCharging)
                        rb.AddForce(-transform.forward * recoil, ForceMode.Impulse);
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canDoDamage = false;
            //Debug.Log("BIPED: Cannot Attack");
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
