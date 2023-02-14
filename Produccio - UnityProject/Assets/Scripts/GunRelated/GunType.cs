using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;


public class GunType : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol,
        Shotgun,
        Rifle
    }

    public WeaponType currentWeapon = WeaponType.Pistol;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI currentMagazineUI;

    public ParticleSystem blooodEffect;

    private AudioSource audioS;

    public int pelletCount = 10;
    public float fireRate = 0.5f;
    public float spread = 10f;
    private int maxAmmo;
    public int currentAmmo;
    public float reloadTime = 2f;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private float gunDammage;

    public float bulletForce = 22000f;
    private float knockBackForce;

    private Animator ac;


    void Start()
    {
        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                maxAmmo = 15;
                reloadTime = 1.5f;
                gunDammage = 24.5f;
                fireRate = 0.25f;
                knockBackForce = 5f;
                break;
            case WeaponType.Shotgun:
                maxAmmo = 8;
                reloadTime = 1.5f;
                gunDammage = 9.5f;
                fireRate = 0.25f;
                knockBackForce = 5f;
                break;
            case WeaponType.Rifle:
                maxAmmo = 30;
                reloadTime = 1.5f;
                gunDammage = 45.0f;
                fireRate = 0.25f;
                knockBackForce = 7f;
                break;
        }
        currentAmmo = maxAmmo;
        audioS = GetComponent<AudioSource>();
        ac = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && !isReloading)
        {
            nextFireTime = Time.time + fireRate;           
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
        upDateUIAmmo();
    }

    void Fire()
    {
        if (currentAmmo <= 0)
        {
            return;
        }
        audioS.Play();
        muzzleFlash.Play();
        ac.SetBool("Shoot", true);
        currentAmmo--;
        switch (currentWeapon)
        {
            case WeaponType.Pistol:              
                FirePistol();
                break;
            case WeaponType.Shotgun:              
                FireShotgun();
                break;
            case WeaponType.Rifle:                
                FireRifle();
                break;
        }
        ac.SetBool("Shoot", false);
    }

    void FirePistol()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * bulletForce);

        // Use raycasting to detect and interact with objects in the scene
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.forward, out hit))
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

    void FireShotgun()
    {
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
                }
            }
        }
    }

    void FireRifle()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * bulletForce);

        // Use raycasting to detect and interact with objects in the scene
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.forward, out hit))
        {
            Debug.DrawLine(bulletSpawnPoint.position, hit.point, Color.red, 2f);
            if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(gunDammage, knockBackForce);
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
    private void upDateUIAmmo()
    {
        currentMagazineUI.SetText(currentAmmo + "/" + maxAmmo);
    }
}

