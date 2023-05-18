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
    public float timeRecoverMultiplyer = 0.015f;

    private float SecondSkillUseTime ;
    private float SecondSkillCooldown;

    private float elapsedTime = 0;

    [Header("UI")]
    public GameObject QSkill;
    public GameObject ESkill;

    [Header("Templar Skills")]
    public GameObject templarPistol;
    public GameObject shotgunSkillIcon;
    public GameObject swordSkillIcon;
    public GameObject templarShotgun;

    private bool shotgunIconrecover = false;
    private bool shotgunActive = false;
    private bool shotgunIsInCD = false;

    public GameObject sword;

    private bool swordIconrecover = false;
    private bool swordActive = false;
    private bool swordIsInCD = false;

    [Header("Nun Skill")]   
    public GameObject CrossBowSkillIcon;
    public GameObject NadeSkillIcon;

    public GameObject nunRevolver;
    public GameObject nunCrossBow;
    public GameObject nunNade;

    private bool crossBowIconrecover = false;
    private bool crossBowActive = false;
    private bool crossIsInCD = false;

    private Animator nunAc;
    

    private bool nadeIconrecover = false;
    private bool nadeActive = false;
    private bool nadeIsInCD = false;
    // Start is called before the first frame update
    void Start()
    {
        switch (currentSet)
        {
            case HabilitiesClassSet.Templar:
                FirstSkillUseTime = 5.0f;
                FirstSkillCooldown = 15.0f;
                SecondSkillUseTime = 0.3f;
                SecondSkillCooldown = 8.0f;
                break;
            case HabilitiesClassSet.Nun:
                FirstSkillUseTime = 2.0f;
                FirstSkillCooldown = 0.5f;
                SecondSkillUseTime = 0.3f;
                SecondSkillCooldown = 8.0f;
                nunAc = GetComponent<Animator>();               
                break;
        }
        templarShotgun.SetActive(false);
        sword.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerSkill();
        switch (currentSet)
        {            
            case HabilitiesClassSet.Templar:                
                if (shotgunIconrecover)
                    shotgunIconRecovery();
                swordSkill();
                if (swordIconrecover)
                    swordIconRecovery();
                break;
            case HabilitiesClassSet.Nun:               
                if (crossBowIconrecover)
                    crossbowIconRecovery();
                /*nadeSkill();
                if (nadeIconrecover)
                    nadeIconRecovery();*/
                break;
        }
    }

    private void PlayerSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !shotgunActive && !shotgunIsInCD)
        {
            switch (currentSet)
            {
                case HabilitiesClassSet.Templar:
                    templarPistol.SetActive(false);
                    //crida funcio de disable de guntype
                    templarShotgun.SetActive(true);
                    shotgunSkillIcon.GetComponent<CanvasGroup>().alpha = 0.0f;
                    shotgunActive = true;
                    StartCoroutine(useShotgunTimeOut());
                    QSkill.SetActive(false);
                    break;
                case HabilitiesClassSet.Nun:
                    nunRevolver.SetActive(false);
                    nunCrossBow.SetActive(true);
                    shotgunSkillIcon.GetComponent<CanvasGroup>().alpha = 0.0f;//Aqui s'ha de canviar als altres icones
                    crossBowActive= true;
                    StartCoroutine(useCrossBowTimeOut());
                    QSkill.SetActive(false);
                    break;
            }
        }
    }
    //Templar
    private IEnumerator useShotgunTimeOut()
    {
        yield return new WaitForSeconds(FirstSkillUseTime);
        templarShotgun.SetActive(false);
        templarPistol.SetActive(true);
        shotgunIsInCD = true;
        shotgunActive = false;
        StartCoroutine(shotgunCooldown());
        shotgunIconrecover = true;
        elapsedTime = 0f;
    }
    private IEnumerator shotgunCooldown()
    {
        yield return new WaitForSeconds(FirstSkillCooldown);
        shotgunIsInCD = false;
        QSkill.SetActive(true);
    }
    private void shotgunIconRecovery()
    {
        if (shotgunSkillIcon.GetComponent<CanvasGroup>().alpha < 1)
        {
            // shotgunSkillIcon.GetComponent<CanvasGroup>().alpha += Time.deltaTime * timeRecoverMultiplyer;
            shotgunSkillIcon.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / FirstSkillCooldown);
            elapsedTime += Time.deltaTime;
        }
        else
            shotgunIconrecover = false;
    }
    private void swordSkill()
    {
        if (Input.GetKeyDown(KeyCode.E) && !shotgunActive && !swordActive && !swordIsInCD)
        {
            switch (currentSet)
            {
                case HabilitiesClassSet.Templar:
                    templarPistol.SetActive(false);
                    sword.SetActive(true);
                    swordActive = true;
                    swordSkillIcon.GetComponent<CanvasGroup>().alpha = 0.0f;
                    StartCoroutine(useSwordTimeOut());
                    elapsedTime = 0f;
                    ESkill.SetActive(false);
                    break;
            }
        }
    }
    private IEnumerator useSwordTimeOut()
    {
        yield return new WaitForSeconds(SecondSkillUseTime);
        sword.SetActive(false);
        templarPistol.SetActive(true);
        swordIsInCD = true;
        swordActive = false;
        StartCoroutine(swordCooldown());
        swordIconrecover = true;
    }
    private IEnumerator swordCooldown()
    {
        yield return new WaitForSeconds(SecondSkillCooldown);
        swordIsInCD = false;
        ESkill.SetActive(true);
    }
    private void swordIconRecovery()
    {
        if (swordSkillIcon.GetComponent<CanvasGroup>().alpha < 1)
        {
            swordSkillIcon.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / SecondSkillCooldown);
            elapsedTime += Time.deltaTime;
        }
        else
            swordIconrecover = false;
    }
    //Nun
    private IEnumerator useCrossBowTimeOut()
    {
        yield return new WaitForSeconds(FirstSkillUseTime);
        nunCrossBow.SetActive(false);
        nunRevolver.SetActive(true);
        crossIsInCD = true;
        crossBowActive = false;
        StartCoroutine(crossbowCooldown());
        crossBowIconrecover = true;
        elapsedTime = 0f;
    }
    private IEnumerator crossbowCooldown()
    {
        yield return new WaitForSeconds(FirstSkillCooldown);
        crossIsInCD = false;
        QSkill.SetActive(true);
    }
    private void crossbowIconRecovery()
    {
        if (CrossBowSkillIcon.GetComponent<CanvasGroup>().alpha < 1)
        {
            // shotgunSkillIcon.GetComponent<CanvasGroup>().alpha += Time.deltaTime * timeRecoverMultiplyer;
            CrossBowSkillIcon.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / FirstSkillCooldown);
            elapsedTime += Time.deltaTime;
        }
        else
            crossBowIconrecover = false;
    }

    //Upgrades
    public void reduceCD()
    {
        if(FirstSkillCooldown > 5f) //limite the cd, hay que ajustarlo y desactivar la opcion cuando sea inferior
            FirstSkillCooldown -= 0.5f;
        if (SecondSkillCooldown > 2f)
            SecondSkillCooldown -= 0.2f;
    }
    public float GetCooldownValue1()
    {
        return FirstSkillCooldown;
    }
    public float GetCooldownValue2()
    {
        return SecondSkillCooldown;
    }
}
