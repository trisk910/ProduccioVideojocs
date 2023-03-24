using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInpact : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy(this.gameObject, 15f);//temporal
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

     private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
