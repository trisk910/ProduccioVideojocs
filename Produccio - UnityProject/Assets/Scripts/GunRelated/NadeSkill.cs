using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class NadeSkill : MonoBehaviour
{
   
    public GameObject AreaEffect;
    private bool nadeOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!nadeOnce)
        {
            Instantiate(AreaEffect, transform.position, transform.rotation);
            nadeOnce = true;
        }
        Destroy(this.gameObject);
    }
}
