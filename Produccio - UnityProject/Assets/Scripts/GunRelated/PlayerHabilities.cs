using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject shotgunSkillIcon;
    private bool shotgunIconrecover = false;

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
        if(shotgunIconrecover)
            shotgunIconRecovery();
    }

    private void shotgunSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !shotgunActive && !shotgunIsInCD)
        {
            pistol.SetActive(false);
            shotgun.SetActive(true);
            shotgunActive = true;
            shotgunSkillIcon.GetComponent<CanvasGroup>().alpha = 0.0f;
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
        shotgunIconrecover = true;
    }
    private IEnumerator shotgunCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        shotgunIsInCD = false;
    }
    private void shotgunIconRecovery()
    {
        if (shotgunSkillIcon.GetComponent<CanvasGroup>().alpha < 1)
            shotgunSkillIcon.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 0.015f;
        else
            shotgunIconrecover = false;
    }
}
