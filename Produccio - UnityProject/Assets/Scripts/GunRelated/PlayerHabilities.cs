using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHabilities : MonoBehaviour
{

    public enum HabilitiesClassSet { Templar,Nun };
    public HabilitiesClassSet currentSet;

    public float useTimer = 10.0f;
    public float cooldown = 15.0f;
    public GameObject pistol;
    public GameObject shotgun;
    public GameObject sword;

    private bool shotgunActive = false;
    private bool shotgunIsInCD = false;

    // Start is called before the first frame update
    void Start()
    {
        shotgun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        shotgunSkill();
    }

    private void shotgunSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !shotgunActive && !shotgunIsInCD)
        {
            pistol.SetActive(false);
            shotgun.SetActive(true);
            shotgunActive = true;
            StartCoroutine(useShotgunTimeOut());
        }
    }
    private IEnumerator useShotgunTimeOut()
    {
        yield return new WaitForSeconds(useTimer);
        shotgun.SetActive(false);
        pistol.SetActive(true);
        shotgunIsInCD = true;
        shotgunActive = false;
        StartCoroutine(shotgunCooldown());
    }
    private IEnumerator shotgunCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        shotgunIsInCD = false;
    }
}
