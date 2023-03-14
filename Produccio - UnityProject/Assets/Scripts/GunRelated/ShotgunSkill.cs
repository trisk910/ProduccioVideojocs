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
    private float gunDammage = 10f;

    public float bulletForce = 22000f;
    private float knockBackForce = 5f;

    public ParticleSystem blooodEffect;
    public ParticleSystem muzzleFlash;
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
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(bulletDirection));
            bullet.GetComponent<Rigidbody>().AddForce(bulletDirection * bulletForce);

            // Use raycasting to detect and interact with objects in the scene
            RaycastHit hit;
            if (Physics.Raycast(bulletSpawnPoint.position, bulletDirection, out hit))
            {
                Debug.DrawLine(bulletSpawnPoint.position, hit.point, Color.red, 2f);
                if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(gunDammage, knockBackForce);
                    Instantiate(blooodEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    blooodEffect.Play();
                }
            }
        }
        //ac.SetBool("Shoot", false);

    }
}
