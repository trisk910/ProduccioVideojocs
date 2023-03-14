using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEvents : MonoBehaviour
{
    public Slider healthbar;
    private float maxHP = 100;
    public float currentHP;
    public float damageDelay = 15.0f;

    public GameObject deathScreen;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        healthbar.maxValue = maxHP;
        healthbar. value = healthbar.maxValue;
        deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage(float damage)
    {
       // if(canTakeDamage)
       // {
       if(currentHP <=0)
        {
            Die();
        }
            currentHP -= damage;
            healthbar.value = currentHP - Time.time;
            StartCoroutine(DamageDelayer(damageDelay));
        

       // }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //canTakeDamage = false;
            
        }
    }
    private IEnumerator DamageDelayer(float damageDelay)
    {        
        yield return damageDelay;
        if (currentHP <= 0)
        {
            Die();
        }
        // canTakeDamage = true;
    }

    private void Die()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
