using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public GameObject pauseMenuObj;
    
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                //Cursor.visible = true;
                Time.timeScale = 0;
                pauseMenuObj.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                Debug.Log("Return to game");
                //Cursor.visible = false;

                pauseMenuObj.SetActive(false);
            }

        }

        if (Input.GetKeyDown(KeyCode.Q) && Time.timeScale == 0)
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }

    public void OnReturnToGameButtonPress()
    {

    }
}
