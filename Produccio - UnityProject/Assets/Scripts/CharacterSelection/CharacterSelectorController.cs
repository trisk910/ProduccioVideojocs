using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectorController : MonoBehaviour
{
    [Header("Templar")]
    public GameObject TemplarCharacter;
    public GameObject SwordSkillTemplarIcon;
    public GameObject ShotgunSkillIcon;
    [Header("Nun")]
    public GameObject NunCharacter;
    public GameObject CrossbowSkillIcon;
    public GameObject NadeSkillIcon;

    
    // Start is called before the first frame update
    private void Start()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        // Access the variable value
        int variableValue = gameManager.CharacterSelected;
        switch (variableValue)
        {
            case 1:
                TemplarCharacter.SetActive(true);
                NunCharacter.SetActive(false);
                SwordSkillTemplarIcon.SetActive(true);
                ShotgunSkillIcon.SetActive(true);
                CrossbowSkillIcon.SetActive(false);
                NadeSkillIcon.SetActive(false);
                //radar.SetTemplar();
                break;
            case 2:
                TemplarCharacter.SetActive(false);
                NunCharacter.SetActive(true);
                SwordSkillTemplarIcon.SetActive(false);
                ShotgunSkillIcon.SetActive(false);
                CrossbowSkillIcon.SetActive(true);
                NadeSkillIcon.SetActive(true);
               //radar.SetNun();
                break;
            case 0:
                Debug.Log("ERROR: No Character was Loaded");
                break;
        }
        
        
    }
    

}
