using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHabilities : MonoBehaviour
{

    public enum HabilitiesClassSet { Templar,Nun };
    public HabilitiesClassSet currentSet;

    private float FirstSkillUseTime ;
    private float FirstSkillCooldown;

    private float SecondSkillUseTime ;
    private float SecondSkillCooldown;

    public GameObject pistol;

    public GameObject templarShotgun;
   
    public GameObject shotgunSkillIcon;
    private bool shotgunIconrecover = false;
    private bool shotgunActive = false;
    private bool shotgunIsInCD = false;

    public GameObject sword;
    public GameObject swordSkillIcon;
    private bool swordIconrecover = false;
    private bool swordActive = false;
    private bool swordIsInCD = false;


    // Start is called before the first frame update
    void Start()
    {
        switch (currentSet)
        {
            case HabilitiesClassSet.Templar:
                FirstSkillUseTime = 10.0f;
                FirstSkillCooldown = 15.0f;
                SecondSkillUseTime = 0.4f;
                SecondSkillCooldown = 8.0f;
                break;
        }
        templarShotgun.SetActive(false);
        sword.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        shotgunSkill();        
        if (shotgunIconrecover)
            shotgunIconRecovery();
        swordSkill();
        if (swordIconrecover)
            swordIconRecovery();
    }

    private void shotgunSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !shotgunActive && !shotgunIsInCD)
        {
            pistol.SetActive(false);
            templarShotgun.SetActive(true);
            shotgunActive = true;
            shotgunSkillIcon.GetComponent<CanvasGroup>().alpha = 0.0f;
            StartCoroutine(useShotgunTimeOut());
        }
    }
    
    private IEnumerator useShotgunTimeOut()
    {
        yield return new WaitForSeconds(FirstSkillUseTime);
        templarShotgun.SetActive(false);
        pistol.SetActive(true);
        shotgunIsInCD = true;
        shotgunActive = false;
        StartCoroutine(shotgunCooldown());
        shotgunIconrecover = true;
    }
    private IEnumerator shotgunCooldown()
    {
        yield return new WaitForSeconds(FirstSkillCooldown);
        shotgunIsInCD = false;
    }
    private void shotgunIconRecovery()
    {
        if (shotgunSkillIcon.GetComponent<CanvasGroup>().alpha < 1)
            shotgunSkillIcon.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 0.015f;
        else
            shotgunIconrecover = false;
    }
    private void swordSkill()
    {
        if (Input.GetKeyDown(KeyCode.E) && !shotgunActive && !swordActive && !swordIsInCD)
        {
            pistol.SetActive(false);
            sword.SetActive(true);
            swordActive = true;
            swordSkillIcon.GetComponent<CanvasGroup>().alpha = 0.0f;
            StartCoroutine(useSwordTimeOut());
        }
    }
    private IEnumerator useSwordTimeOut()
    {
        yield return new WaitForSeconds(SecondSkillUseTime);
        sword.SetActive(false);
        pistol.SetActive(true);
        swordIsInCD = true;
        swordActive = false;
        StartCoroutine(swordCooldown());
        swordIconrecover = true;
    }
    private IEnumerator swordCooldown()
    {
        yield return new WaitForSeconds(SecondSkillCooldown);
        swordIsInCD = false;
    }
    private void swordIconRecovery()
    {
        if (swordSkillIcon.GetComponent<CanvasGroup>().alpha < 1)
            swordSkillIcon.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 0.08f;
        else
            swordIconrecover = false;
    }
}
