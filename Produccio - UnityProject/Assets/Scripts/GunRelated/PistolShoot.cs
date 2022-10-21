using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : MonoBehaviour
{

    public List<GameObject> bulletList;
    public GameObject bullet;
    public int amout;
    public Transform initialPos;
    public float bulletSpeed;


    void Start()
    {
        bulletList = new List<GameObject>();
        for (int i = 0; i < amout; i++)
        {
            GameObject objBullet = (GameObject)Instantiate(bullet);
            objBullet.SetActive(false);
            bulletList.Add(objBullet);
        }
    }


    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeInHierarchy)
            {
                bulletList[i].transform.position = initialPos.position;///
               // bulletList[i].transform.rotation = initialPos.rotation;
                bulletList[i].SetActive(true);
                Rigidbody tempRigidBodyBullet = bulletList[i].GetComponent<Rigidbody>();
                tempRigidBodyBullet.AddForce(tempRigidBodyBullet.transform.forward * bulletSpeed);
                break;
            }
        }
    }
    /*public void OnMouseDown()
    {
        Debug.Log("I am Shooting");
        Shoot();
    }*/
}