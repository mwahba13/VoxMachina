using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject CreditsPane;


    private void Start()
    {
        CreditsPane.SetActive(false);
        Cursor.visible = true;
        
    }

    public void OnPlayButtonPress()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnCreditsButtonPress()
    {
        CreditsPane.SetActive(true);
    }
    
    public void OnCreditsHidePress()
    {
        CreditsPane.SetActive(false);
    }


    public void OnQuitButtonPress()
    {
        Application.Quit();
    }
}
