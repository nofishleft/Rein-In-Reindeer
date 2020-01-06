using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpMenu : MonoBehaviour
{
    public GameObject pMenu;
    public GameObject cMenu;
    public GameObject creditMenu;
    public AudioSource blizzard;

    bool inCMenu;
    bool started = false;

    public TMP_Text startedText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.player.paused = true;
        startedText.text = "Start";
    }

    public bool EscapeClicked()
    {
        if (inCMenu)
        {
            //closeControlMenu
            pMenu.SetActive(true);
            cMenu.SetActive(false);
            creditMenu.SetActive(false);

            return false; ;
        }

        return true;
    }

    //Pause Menu

    public void ResumeClicked()
    {
        if (started == false) {
            startedText.text = "Resume";
            started = true;

            blizzard.Play();
        }
        Debug.Log("R:1");
        PlayerController.player.paused = false;
        Debug.Log("R:2");
        cMenu.SetActive(false);
        Debug.Log("R:3");
        pMenu.SetActive(false);
        Debug.Log("R:4");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ControlsClicked()
    {
        pMenu.SetActive(false);
        cMenu.SetActive(true);
        creditMenu.SetActive(false);
        Cursor.visible = true;
    }

    public void CloseControls()
    {
        pMenu.SetActive(true);
        cMenu.SetActive(false);
        creditMenu.SetActive(false);
        Cursor.visible = true;
    }

    public void ExitClicked() 
    {
        Application.Quit();
    }

    public void CreditsClicked()
    {
        pMenu.SetActive(false);
        cMenu.SetActive(false);
        creditMenu.SetActive(true);
        Cursor.visible = true;
    }

    public void CloseCredits()
    {
        pMenu.SetActive(true);
        cMenu.SetActive(false);
        creditMenu.SetActive(false);
        Cursor.visible = true;
    }

    //Controls Menu
}
