using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class GunType : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol,Revolver
    }

    public WeaponType currentWeapon = WeaponType.Pistol;


    [Header("Bullet Pool")]
    public List<GameObject> bulletList;
    public int amount;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public ParticleSystem muzzleFlash;
    

    public GameObject blooodEffect;
    public GameObject impactEffect;

    [Header("Sounds")]
    private AudioSource audioS;
    public AudioClip GunShot;
    public AudioClip CritSound;




    [Header("Gun Stats")]
    private int maxAmmo;
    public int currentAmmo;
    public float fireRate = 0.5f;
    public float reloadTime = 2f;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private float gunDammage;

    private float weakSpotMultiplyer;

    public float bulletForce = 22000f;
    private float knockBackForce;

    /*[Header("Shotgun")]
    public int pelletCount = 10;
    public float spread = 10f;*/

    private Animator ac;

    [Header("Camera")]
    public Camera mainCamera;

    [Header("UI")]
    public GameObject reloadingText;
    public TextMeshProUGUI currentMagazineUI;

    void Start()
    {
        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                maxAmmo = 15;
                reloadTime = 1.5f;
                gunDammage = 24.5f;
                fireRate = 0.25f;
                knockBackForce = /*5f*/2f;
                break;
            case WeaponType.Revolver:
                maxAmmo = 6;
                reloadTime = 1.0f;
                gunDammage = 50f;
                fireRate = 0.5f;
                knockBackForce = /*5f*/2f;
                break;
        }
        currentAmmo = maxAmmo;
        audioS = GetComponent<AudioSource>();
        ac = GetComponent<Animator>();
        // Create bullet pool
        bulletList = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            GameObject objBullet = (GameObject)Instantiate(bulletPrefab);
            objBullet.SetActive(false);
            bulletList.Add(objBullet);
        }
        reloadingText.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && !isReloading)
        {
            nextFireTime = Time.time + fireRate;           
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && (currentAmmo != maxAmmo))
        {
            StartCoroutine(Reload());
        }
        upDateUIAmmo();
    }

    void Fire()
    {
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        audioS.PlayOneShot(GunShot, 0.7F);
        muzzleFlash.Play();
        ac.SetTrigger("Shoot");
        //ac.SetBool("Shoot", true);
        //ac.GetComponent<Animator>().SetBool("Shoot", true);
        currentAmmo--;
        FireGun();
        /*switch (currentWeapon)
        {
            case WeaponType.Pistol:              
                FirePistol();
                break;
            
            case WeaponType.Revolver:
                FireRevolver();
                break;
        }*/
        ac.SetTrigger("Shoot");
       
        //ac.GetComponent<Animator>().SetBool("Shoot", false);
    }
  

    void FireGun()
    {
        GameObject currentBullet = getBulletPool();
        currentBullet.transform.position = bulletSpawnPoint.position;
        currentBullet.transform.rotation = bulletSpawnPoint.rotation;
        currentBullet.SetActive(true);
        Rigidbody tempRigidBodyBullet = currentBullet.GetComponent<Rigidbody>();
        tempRigidBodyBullet.angularVelocity = Vector3.zero;
        tempRigidBodyBullet.velocity = Vector3.zero;
        tempRigidBodyBullet.AddForce(tempRigidBodyBullet.transform.forward * bulletForce, ForceMode.Impulse);

        RaycastHit hit;
        
        if (mainCamera != null && Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red, 2f);

            if ( hit.collider.gameObject.GetComponentInParent<Enemy>() != null)
            {
                weakSpotMultiplyer = 1.0f;
                if (hit.collider.tag == "WeakSpot")
                {
                    audioS.PlayOneShot(CritSound, 0.7F);
                    switch (currentWeapon)
                    {
                        case WeaponType.Revolver:
                            weakSpotMultiplyer = 3.5f;
                            break;
                        case WeaponType.Pistol:
                            weakSpotMultiplyer = 2.0f;
                            break;
                    }
                   
                    //Debug.Log("Crit");

                }
                float totalDamage = gunDammage * weakSpotMultiplyer;
                
                hit.collider.gameObject.GetComponentInParent<Enemy>().TakeDamage(totalDamage, knockBackForce);

                GameObject bloodParticle = Instantiate(blooodEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                bloodParticle.GetComponent<ParticleSystem>().Play();
                Destroy(bloodParticle, 2f);
            }
            else if (hit.collider.gameObject.GetComponentInParent<Enemy>() == null)
            {
                
                GameObject impactParticle = Instantiate(impactEffect,hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                impactParticle.GetComponent<ParticleSystem>().Play();
                Destroy(impactParticle, 2f);
            }

        }
    }
   

    private GameObject getBulletPool()
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeInHierarchy)
            {
                return bulletList[i];
            }
        }
        GameObject objBullet = (GameObject)Instantiate(bulletPrefab);
        objBullet.SetActive(false);
        bulletList.Add(objBullet);
        return objBullet;
    }

    IEnumerator Reload()
    {
        switch (currentWeapon)
        {
            case WeaponType.Revolver:
                ac.SetTrigger("Reload");
                break;
        }
        
        isReloading = true;
        reloadingText.SetActive(true);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        reloadingText.SetActive(false);
    }
    private void upDateUIAmmo()
    {
        currentMagazineUI.SetText(currentAmmo + "/" + maxAmmo);
    }

    public void IncreasePistolDamage()
    {
        gunDammage += 5f;
    }
    public float GetPistolDamage()
    {
        return gunDammage;
    }

    private void OnEnable()
    {
        currentMagazineUI.SetText(currentAmmo + "/" + maxAmmo);
    }
    void OnDisable()// cambia la funcio per a k es cridi a playerHabilities
    {
        isReloading = false;
        //currentAmmo = maxAmmo;
        reloadingText?.SetActive(false);
        currentMagazineUI.SetText("--/--");
    }

}

