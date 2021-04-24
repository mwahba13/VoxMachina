using System;
using UnityEngine;


public class EnemyEventHandler : MonoBehaviour
{
    //check if player is within hearing raidus
    private bool _isPlayerNear;
    [SerializeField] private ESoundPitch[] _weakness = new ESoundPitch[3];
    
    private EnemyStateMachine _stateMachine;
    
    private void Start()
    {
        GameEventSystem.current.OnPlayerCastSpell += OnPlayerCastSpell;
        GameEventSystem.current.OnTerminalCastSpell += OnTerminalCastSpell;
        _stateMachine = GetComponentInParent<EnemyStateMachine>();
    }


    private void OnPlayerCastSpell(ESoundPitch[] list)
    {
        //if player is within hearing radius and makes sensitive sound
        if (_isPlayerNear)
        {
            _stateMachine.StateTransition(EEnemyState.SeekingSound_Moving);   
        }
    }

    private void OnTerminalCastSpell(ESoundPitch[] list)
    {
        if (IsTargetedByTerminal(list))
        {
            _stateMachine.StateTransition(EEnemyState.Stunned);
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
        GameEventSystem.current.OnTerminalCastSpell -= OnTerminalCastSpell;
    }

    #region Utilities

    private bool IsTargetedByTerminal(ESoundPitch[] list)
    {
        if (_weakness.Length == 1)
        {
            return (list[0] == _weakness[0] ||
                    list[1] == _weakness[0] ||
                    list[2] == _weakness[0]);
        }
        else if (_weakness.Length == 2)
        {
            return ((list[0] == _weakness[0] && list[1] == _weakness[1]) ||
                    (list[1] == _weakness[0] && list[2] == _weakness[1]));
        }
        else
        {
            return (list[0] == _weakness[0] &&
                    list[1] == _weakness[1] &&
                    list[2] == _weakness[2]);
        }
    }

    #endregion
}
