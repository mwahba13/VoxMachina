using System;
using UnityEngine;


public class EnemyEventHandler : MonoBehaviour
{
    //check if player is within hearing raidus
    private bool _isPlayerNear;
    [SerializeField] ESoundPitch _sensitiveSound;            //enemy will only respond to this sound
    private EnemyStateMachine _stateMachine;
    
    private void Start()
    {
        GameEventSystem.current.OnPlayerCastSpell += OnPlayerCastSpell;
        _stateMachine = GetComponentInParent<EnemyStateMachine>();
    }


    private void OnPlayerCastSpell(ESoundPitch[] list)
    {
        //if player is within hearing radius and makes sensitive sound
        if (_isPlayerNear && ListHasSensitiveSound(list))
        {
            _stateMachine.StateTransition(EEnemyState.SeekingSound_Moving);   
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

    #region Utilities

    private bool ListHasSensitiveSound(ESoundPitch[] list)
    {
        if (_sensitiveSound.Equals(ESoundPitch.All))
            return true;
        else if (_sensitiveSound.Equals(ESoundPitch.None))
            return false;
        
        return (list[0].Equals(_sensitiveSound) ||
                list[1].Equals(_sensitiveSound) ||
                list[2].Equals(_sensitiveSound));
    }

    #endregion
}
