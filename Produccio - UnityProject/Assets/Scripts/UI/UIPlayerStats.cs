using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerStats : MonoBehaviour
{
    public TextMeshProUGUI HPUI;
    public TextMeshProUGUI RegenUI;
    public TextMeshProUGUI DamageUI;
    public TextMeshProUGUI SpeedUI;
    public TextMeshProUGUI Skill1UI;
    public TextMeshProUGUI Skill2UI;


    //private GunType weaponStats;
    private GameObject weaponStats;
    //private Player playerStats;
    private GameObject playerStats;
    //private PlayerHabilities habilitiesStats;
    private GameObject habilitiesStats;

    private float HP;
    private float Regen;
    private float Damage;    
    private float Speed;
    private float Skill1;
    private float Skill2;

    private void Start()
    {
        //playerStats = Object.FindObjectOfType<Player>();
        //weaponStats = Object.FindObjectOfType<GunType>();

        playerStats  = GameObject.FindGameObjectWithTag("Player");
        weaponStats = GameObject.FindGameObjectWithTag("MainGun");
        habilitiesStats = GameObject.FindGameObjectWithTag("PlayerSkills");

    }


    // Update is called once per frame
    void Update()
    {
        HP = playerStats.GetComponent<Player>().GetHP();        
        Regen = playerStats.GetComponent<Player>().GetRegenRate();
        Speed = playerStats.GetComponent<Player>().GetBaseSpeed();

        Damage = weaponStats.GetComponent<GunType>().GetPistolDamage();        
        
        Skill1 = habilitiesStats.GetComponent<PlayerHabilities>().GetCooldownValue1();
        Skill2 = habilitiesStats.GetComponent<PlayerHabilities>().GetCooldownValue2();

        HPUI.SetText("HP: " + HP);
        RegenUI.SetText("Regen: " + Regen);
        DamageUI.SetText("Main Gun Damage: " + Damage);

        SpeedUI.SetText("Speed: " + Speed);
        Skill1UI.SetText("1st Skill CD: " + Skill1);
        Skill2UI.SetText("2nd Skill CD: " + Skill2);
    }
}
