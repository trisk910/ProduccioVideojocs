using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    // Start is called before the first frame update
    public float swordDamage;
    public float knockBackForce;
    public GameObject bloodHit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Enemy"))
        {
            // Call TakeDamage() function on enemy
            other.GetComponent<Enemy>().TakeDamage(swordDamage, knockBackForce);
            //Debug.Log("Slash");
            GameObject bloodParticle = Instantiate(bloodHit, other.transform.position, Quaternion.FromToRotation(Vector3.right, other.normal));
            bloodParticle.GetComponent<ParticleSystem>().Play();
            Destroy(bloodParticle, 2f);

        }*/
        if (other.CompareTag("Enemy"))
        {
            // Call TakeDamage() function on enemy
            other.GetComponent<Enemy>().TakeDamage(swordDamage, knockBackForce);

            // Instantiate blood hit particle effect at the contact point
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            Quaternion contactRotation = Quaternion.FromToRotation(Vector3.right, other.transform.position - contactPoint);
            GameObject bloodParticle = Instantiate(bloodHit, contactPoint, contactRotation);
            bloodParticle.GetComponent<ParticleSystem>().Play();
            Destroy(bloodParticle, 2f);
        }
    }

    public void UpgradeSword()
    {
        swordDamage += 5f;
        knockBackForce += 2f;
    }
   
}
