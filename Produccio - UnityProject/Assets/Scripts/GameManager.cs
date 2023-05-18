using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CharacterSelected;


    public void SetTemplar()
    {
        CharacterSelected = 1;
    }
    public void SetNun()
    {
        CharacterSelected = 2;
    }
}
