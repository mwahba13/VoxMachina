using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    private bool _isPlayerNear = false;
    
    // Start is called before the first frame update
    void Start()
    {
        GameEventSystem.current.OnPlayerCastSpell += OnPlayerCastSpell;
    }

    private void OnPlayerCastSpell(ESoundPitch[] list)
    {

        if (_isPlayerNear)
        {
            Debug.Log("Player Cast spell in range");
            GameEventSystem.current.TerminalCastSpell(list);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;
            Debug.Log("Player is near");            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left area");
            _isPlayerNear = false;

        }
    }


    private void OnDestroy()
    {
        GameEventSystem.current.OnPlayerCastSpell -= OnPlayerCastSpell; 
    }
}
