using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public GameObject pauseMenuObj;
    public GameObject deathMenuObj;


    private FirstPersonController _fpsController;
    private Animator _animator;
    private PlayerAudioManager _audioManager;

    public int health = 100;

    private bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuObj.SetActive(false);
        deathMenuObj.SetActive(false);
        _animator = GetComponent<Animator>();
        _audioManager = GetComponentInChildren<PlayerAudioManager>();
        _fpsController = GetComponent<FirstPersonController>();

        _fpsController.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        
        if(health < 0)
            OnPlayerDeath();
            
    }

    
    private void OnPlayerDeath()
    {

        _fpsController.enabled = false;
        isDead = true;
        deathMenuObj.SetActive(true);
        pauseMenuObj.SetActive(false);
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            OnTakeDamage();
            DecrementHealth(10);
            
        }
    }

    private void OnTakeDamage()
    {
        _audioManager.PlayPlayerHitClip();
        _animator.SetTrigger("TakeDamage");
    }

    public void DecrementHealth(int damage)
    {
        health -= damage;
        OnTakeDamage();
    }
    
    private void HandleInput()
    {
        if (!isDead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 1)
                {
                    _fpsController.enabled = false;
                    //Cursor.visible = true;
                    Time.timeScale = 0;
                    pauseMenuObj.SetActive(true);
                }
                else
                {
                    _fpsController.enabled = true;
                    Time.timeScale = 1;
                    //Cursor.visible = false;

                    pauseMenuObj.SetActive(false);
                }

            }

            if (Input.GetKeyDown(KeyCode.R) && Time.timeScale == 0)
            {
                health = 100;
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q) && Time.timeScale == 0)
            {
                Application.Quit();
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _fpsController.enabled = true;
                health = 100;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }

    }

}
