using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        DontDestroyOnLoad(gameManager);       
    }
    public void LoadNextScene()
    {        
        SceneManager.LoadScene("Playground");       
    }   
}
