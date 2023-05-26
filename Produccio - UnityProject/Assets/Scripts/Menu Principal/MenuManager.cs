using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    private int menuIndex;
    public GameObject CaracterSelector;
    private AudioSource asc;
    public GameObject flashScreen;
    public GameObject videoPlayer;
    public GameObject Logo;
    void Start()
    {
        CaracterSelector.SetActive(false);
        mainMenu.SetActive(true);
        asc = GetComponent<AudioSource>();

        flashScreen.SetActive(false);
        videoPlayer.SetActive(false);
        mainMenu.SetActive(false);
        Logo.SetActive(true);
        StartCoroutine(InitialCanvas());
        menuIndex = -1;
        Time.timeScale= 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuIndex == 0)
        {
            if (Input.GetButton("Fire1"))
            {

                flashScreen.SetActive(true);
                StartCoroutine(FlashScreen());
                menuIndex = 1;
                PlayGunShot();
                StartCoroutine(ChangeCanvas());
            }
        }
    }
    IEnumerator ChangeCanvas()
    {
        yield return new WaitForSeconds(1.5f);
        CaracterSelector.SetActive(true);
        mainMenu.SetActive(false);
        //videoPlayer.SetActive(true);
    }
    IEnumerator FlashScreen()
    {
        yield return new WaitForSeconds(0.08f);
        flashScreen.SetActive(false);
    }
    public void PlayGunShot()
    {
        asc.Play();
    }
    IEnumerator InitialCanvas()
    {
        yield return new WaitForSeconds(8f);
        mainMenu.SetActive(true);
        menuIndex = 0;
        videoPlayer.SetActive(true);
        Logo.SetActive(false);
    }
}
