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

    public EEnemyType _type;
    //check if player is within hearing raidus
    private bool _isPlayerNear;
    [SerializeField] private ESoundPitch[] _weakness = new ESoundPitch[3];

    public Material redMat;
    public Material greenMat;
    
    //for changing color/material of enemy weakness
    [SerializeField] private GameObject _weakspotMat1;
    [SerializeField] private Light _weakspotLight1;
    
    [SerializeField] private GameObject _weakspotMat2;
    [SerializeField] private Light _weakspotLight2;
    
    [SerializeField] private GameObject _weakspotMat3;
    [SerializeField] private Light _weakspotLight3;
    
    
    
    private EnemyStateMachine _stateMachine;
    
    private void Start()
    {
        GameEventSystem.current.OnPlayerCastSpell += OnPlayerCastSpell;
        GameEventSystem.current.OnTerminalCastSpell += OnTerminalCastSpell;
        _stateMachine = GetComponentInParent<EnemyStateMachine>();
        RandomizeWeakness();
        
        
        
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
        else
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
        GameEventSystem.current.OnTerminalCastSpell -= OnTerminalCastSpell;
    }

    #region Utilities

    private bool IsTargetedByTerminal(ESoundPitch[] list)
    {
        if (_type.Equals(EEnemyType.Small))
        {
            return (list[0] == _weakness[0] ||
                    list[1] == _weakness[0] ||
                    list[2] == _weakness[0]);
        }
        else if (_type.Equals(EEnemyType.Med))
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
        
        //adjust colors of lights and materials to reflect weakness
        _weakspotLight1.color = GetColorFromPitch(_weakness[0]);
        _weakspotMat1.GetComponent<Renderer>().material = GetMaterialFromPitch(_weakness[0]);

        if (_type.Equals(EEnemyType.Large) || _type.Equals(EEnemyType.Med))
        {
            _weakspotLight2.color = GetColorFromPitch(_weakness[1]);
            _weakspotMat2.GetComponent<Renderer>().material = GetMaterialFromPitch(_weakness[1]);
        }
        
        
        

    }


    private Material GetMaterialFromPitch(ESoundPitch p)
    {
        if (p.Equals(ESoundPitch.Low))
            return redMat;
        else
            return greenMat;

    }
    
    private Color GetColorFromPitch(ESoundPitch p)
    {
        if (p.Equals(ESoundPitch.Low))
            return Color.red;
        else
            return Color.green;

    }

    #endregion
}
