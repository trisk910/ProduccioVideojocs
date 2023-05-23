using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GunType;

public class CrossBowSkill : MonoBehaviour
{
    [Header("Camera")]
    public Camera mainCamera;

    [Header("CrossbowStats")]
    public float damage;
    public int maxPenetratedObjects = 8; // Maximum number of game objects the ray can penetrate
    public float knockback;


    [Header("Sounds")]
    private AudioSource audioS;
    public AudioClip GunShot;
    public AudioClip CritSound;

    private Animator canim;
    [Header("Particles")]
    public GameObject blooodEffect;
    public GameObject impactEffect;

    void Start()
    {
        canim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        //StartCoroutine(ReloadAndShoot());
    }
    void OnEnable()
    {
        StartCoroutine(ReloadAndShoot());
    }


    IEnumerator ReloadAndShoot()
    {
        yield return new WaitForSeconds(1f);
        Shoot();
    }

    void Shoot()
    {
        // Get the center of the camera
       audioS.PlayOneShot(GunShot, 0.7F);
        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, mainCamera.transform.forward);
       

        for (int i = 0; i < Mathf.Min(hits.Length, maxPenetratedObjects); i++)
        {
            RaycastHit hit = hits[i];
            Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red, 2f);

            if (hit.collider.gameObject.GetComponentInParent<Enemy>() != null)
            {
                float weakSpotMultiplier = 1.0f;
                if (hit.collider.tag == "WeakSpot")
                {
                    weakSpotMultiplier = 2f;
                    //Debug.Log("Crit");
                }

                float totalDamage = damage * weakSpotMultiplier;

                hit.collider.gameObject.GetComponentInParent<Enemy>().TakeDamage(totalDamage, knockback);

                GameObject bloodParticle = Instantiate(blooodEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                bloodParticle.GetComponent<ParticleSystem>().Play();
                Destroy(bloodParticle, 2f);
            }
            else
            {
                GameObject impactParticle = Instantiate(impactEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                impactParticle.GetComponent<ParticleSystem>().Play();
                Destroy(impactParticle, 2f);
            }
        }


    }
    //upgrades

    public void IncreaseDamage()
    {
        damage += 5f;
    }
  
}
