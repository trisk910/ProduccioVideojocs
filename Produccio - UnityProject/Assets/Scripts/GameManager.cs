using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CharacterSelected = 0;
    public float mouseSens;
    public void SetTemplar()
    {
        CharacterSelected = 1;
    }
    public void SetNun()
    {
        CharacterSelected = 2;
    }

    public void ResetVar()
    {
        CharacterSelected = 0;
    }
    public void UpdateSensitivityCamera(float value)
    {
        mouseSens = value;  
    }
}
