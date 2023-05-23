using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadeSkillDamage : MonoBehaviour
{
    public float damage;
    //public ParticleSystem explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,1.9f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Call TakeDamage() function on enemy
            other.GetComponentInParent<Enemy>().TakeDamage(damage, 0);
        }
    }

    public void IncreaseNadeDamage()
    {
        damage += 5f;
    }
   
}
