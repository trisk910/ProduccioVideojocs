using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GunType;

public class ShotgunSkill : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    private AudioSource audioS;

    public int pelletCount = 10;
    public float fireRate = 0.5f;
    public float spread = 10f;
    private float nextFireTime = 0f;
    private float damagePerPellet = 20f;
    public float maxDistance = 50f;

    public float bulletForce = 22000f;
    private float knockBackForce = 5f;

    public GameObject blooodEffect;
    public ParticleSystem muzzleFlash;
    public Camera mainCamera;
    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }
    void Fire()
    {
       
        audioS.Play();
        muzzleFlash.Play();
        //ac.SetBool("Shoot", true);
        for (int i = 0; i < pelletCount; i++)
        {
           Vector3 bulletDirection = bulletSpawnPoint.forward;
            bulletDirection = Quaternion.Euler(Random.Range(-spread, spread), Random.Range(-spread, spread), 0) * bulletDirection;
            /*GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(bulletDirection));
            bullet.GetComponent<Rigidbody>().AddForce(bulletDirection * bulletForce);*/

            float remainingDistance = maxDistance;
            RaycastHit hit;
            /*if (mainCamera != null && Physics.Raycast(mainCamera.transform.position, bulletDirection, out hit))
            {
                Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red, 2f);

                if (hit.collider.gameObject.GetComponentInParent<Enemy>() != null)
                {
                    float weakSpotMultiplyer = 1.0f;
                    if (hit.collider.tag == "WeakSpot")
                    {
                        weakSpotMultiplyer = 1.5f;
                        Debug.Log("Crit");

                    }

                    float totalDamage = damagePerPellet * weakSpotMultiplyer;
                    //enemy.TakeDamage(totalDamage, knockBackForce);
                    hit.collider.gameObject.GetComponentInParent<Enemy>().TakeDamage(damagePerPellet, knockBackForce);

                    GameObject bloodParticle = Instantiate(blooodEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    bloodParticle.GetComponent<ParticleSystem>().Play();
                    Destroy(bloodParticle, 2f);
                }
            }*/
            while (remainingDistance > 0f && Physics.Raycast(mainCamera.transform.position, bulletDirection, out hit, remainingDistance))
            {
                Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red, 2f);

                float distanceTraveled = hit.distance;
                remainingDistance -= distanceTraveled;

                if (hit.collider.gameObject.GetComponentInParent<Enemy>() != null)
                {
                    float weakSpotMultiplyer = 1.0f;
                    if (hit.collider.tag == "WeakSpot")
                    {
                        weakSpotMultiplyer = 2f;
                        //Debug.Log("Crit");
                    }

                    float distanceReduction = distanceTraveled / maxDistance;
                    float totalDamage = damagePerPellet * weakSpotMultiplyer * (1f - distanceReduction);
                    if (distanceTraveled > 0f)
                    {
                        totalDamage *= 0.2f; // reduce damage if the ray went through an object
                    }

                    hit.collider.gameObject.GetComponentInParent<Enemy>().TakeDamage(totalDamage, knockBackForce);

                    GameObject bloodParticle = Instantiate(blooodEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    bloodParticle.GetComponent<ParticleSystem>().Play();
                    Destroy(bloodParticle, 2f);
                }

                if (remainingDistance > 0f)
                {
                    Vector3 newOrigin = hit.point + bulletDirection * 0.001f;
                    remainingDistance -= 0.001f;
                }
            }
        }
        //ac.SetBool("Shoot", false);

    }
  
}
