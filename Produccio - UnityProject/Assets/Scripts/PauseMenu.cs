using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    private bool isUpgradeMenu;
    public GameObject PauseMenuUI;
    public GameObject UpGradeMenu;
    private GameManager gameManager;

   
    //private bool isUpgradeMenu;
    // Start is called before the first frame update
    void Start()
    {
        PauseMenuUI.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //PauseGame();          
            if (!isPaused)
                isPaused = true;
            else
                isPaused = false;
        }
        PauseGame();       
    }
    void PauseGame()
    {
        if (isPaused && !isUpgradeMenu)
        {
            PauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        if (!isPaused && !isUpgradeMenu)
        {
            PauseMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }
    public void ShowUpgradeMenu()
    {
        Time.timeScale = 0.0f;
        isUpgradeMenu = true;
        isPaused = true;
        //PauseMenuUI.SetActive(false);
        UpGradeMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }
    public void HideUpgradeMenu()
    {
        UpGradeMenu.SetActive(false);
        isUpgradeMenu = false;
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void BackToMainMenu()
    {
        gameManager.ResetVar();
        Destroy(gameManager.gameObject);
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
