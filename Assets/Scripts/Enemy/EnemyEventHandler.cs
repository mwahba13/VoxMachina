using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EEnemyType
{
    Small,
    Med,
    Large,
}

public class EnemyEventHandler : MonoBehaviour
{

    [SerializeField] private EEnemyType _type;
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

    private void RandomizeWeakness()
    {
        int numToAdd = 0;
        
        switch (_type)
        {
            case EEnemyType.Small:
                numToAdd = 1;
                break;
            case EEnemyType.Med:
                numToAdd = 2;
                break;
            case EEnemyType.Large:
                numToAdd = 3;
                break;
            default:
                break;
        }

        ESoundPitch[] randPitch = new ESoundPitch[2];
        randPitch[0] = ESoundPitch.Low;
        randPitch[1] = ESoundPitch.High;
        
        for (int i = 0; i < numToAdd; i++)
        {
            Random.Range(0, 2);

            _weakness[i] = randPitch[Random.Range(0, 2)];
        }
        
    }

    #endregion
}
