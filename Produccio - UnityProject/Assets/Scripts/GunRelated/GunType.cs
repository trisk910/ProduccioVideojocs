using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunType : MonoBehaviour
{
    [SerializeField]
    Transform muzzle;

    [SerializeField]
    protected GunStats weaponSO;

    float nextTimeToFire = 0f;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool CheckFireRate()
    {
        if(Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1f / weaponSO.fireRate);
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //if()
        
    }
}

//https://www.youtube.com/watch?v=cPUvYK3965M