using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
   
    private void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager"); // Assuming your GameManager object is named "GameManager"
        DontDestroyOnLoad(gameManager);
    }

  
    public void LoadNextScene()
    {
        //SceneManager.LoadSceneAsync("Playground");
        SceneManager.LoadScene("Playground");
       
    }
   /* public void Scene1()
    {
        SceneManager.LoadScene("Playground");
    }*/
   
}
