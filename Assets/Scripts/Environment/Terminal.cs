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
        
        if(_isPlayerNear)
            GameEventSystem.current.TerminalCastSpell(list);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _isPlayerNear = false;
    }


    private void OnDestroy()
    {
        GameEventSystem.current.OnPlayerCastSpell -= OnPlayerCastSpell; 
    }
}
