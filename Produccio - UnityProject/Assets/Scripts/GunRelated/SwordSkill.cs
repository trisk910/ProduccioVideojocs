using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    // Start is called before the first frame update
    public float swordDamage;
    public float knockBackForce;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(swordDamage, knockBackForce);
            Debug.Log("Slash");
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(swordDamage, knockBackForce);
            Debug.Log("Slash");
        }
    }*/
   
}
