using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastAliveLookUp : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z));
    }
}
