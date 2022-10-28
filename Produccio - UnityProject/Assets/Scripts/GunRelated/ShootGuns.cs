using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGuns : MonoBehaviour
{

    public List<GameObject> bulletList;
    public GameObject bullet;
    public int amount;
    public Transform initialPos;
    public float bulletSpeed;
    

    //enum weapon{Pistol,SMG};
    private float damage;
    private int magazine = 15;
    private float reloadTime = 3.0f;
    private float fireRate = .25f;
    private bool fireGun = true;
    void Start()
    {
        //Physics.IgnoreLayerCollision(3,3);

        bulletList = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            GameObject objBullet = (GameObject)Instantiate(bullet);
            objBullet.SetActive(false);
            bulletList.Add(objBullet);
        }
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && fireGun)
        { 
            Shoot();
            fireGun = false;
            StartCoroutine(DelayShoot(fireRate));
        }
      
    }
    IEnumerator DelayShoot(float fireRate)
    {
       yield return new WaitForSeconds(fireRate);
       fireGun = true;
    }

    void Shoot()
    {
        GameObject currentBullet = getBulletPool();
        currentBullet.transform.position = initialPos.position;
        currentBullet.transform.rotation = initialPos.rotation;
        currentBullet.SetActive(true);
        Rigidbody tempRigidBodyBullet = currentBullet.GetComponent<Rigidbody>();
        tempRigidBodyBullet.angularVelocity = Vector3.zero;
        tempRigidBodyBullet.velocity = Vector3.zero;
        tempRigidBodyBullet.AddForce(tempRigidBodyBullet.transform.forward * bulletSpeed, ForceMode.Impulse);
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
        GameObject objBullet = (GameObject)Instantiate(bullet);
        objBullet.SetActive(false);
        bulletList.Add(objBullet);
        return objBullet;
    }
   
}