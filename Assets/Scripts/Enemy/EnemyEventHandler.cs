using System;
using UnityEngine;


public class EnemyEventHandler : MonoBehaviour
{
    //check if player is within hearing raidus
    private bool _isPlayerNear;
    
    
    
    private void Start()
    {
        GameEventSystem.current.OnPlayerCastSpell += OnPlayerCastSpell;
    }


    private void OnPlayerCastSpell(ESoundPitch[] list)
    {
        if (_isPlayerNear)
        {
            
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayerNear = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayerNear = false;
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.current.OnPlayerCastSpell -= OnPlayerCastSpell;
    }
}
