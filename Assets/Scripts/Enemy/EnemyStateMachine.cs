﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EEnemyState
{
    //Base States
    Idle,
    Wandering,          //wandering aimlessly
    Patrolling,         //moving along a pre-determined path
    Stunned,            //stunned for time period
    
    //Seeking sound sub tree
    SeekingSound_Moving,       //moving towards sound it heard within hearing radius
    SeekingSound_Patrolling,
    
    //Hunting sub tree
    Hunting,
}


public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private EEnemyState _state;
    [SerializeField] private float _destinationBuffer;      //how close it needs to be to dest to switch to next dest
    [SerializeField] private float _wanderDistance;
    [SerializeField] private float _huntingPlayerDistance;      //how clsoe player needs to be for enemy to be deadyl
    private GameObject _playerObj;
    
    
    //
    public Transform[] patrolPathNodes;
    private int patrolPathIndex;
    
    //for dynamic patrol routes
    private Vector3[] _dynamicPatrolNodes = new Vector3[4];
    private int _dynPatrolIndex;

    [SerializeField] private float _stunTimer;
    public float stunDuration;
    
    //Nav mesh agent
    private NavMeshAgent _navAgent;

    private EnemyAudioManager _audioManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _playerObj = GameObject.FindWithTag("Player");
        _audioManager = GetComponent<EnemyAudioManager>();


        
        StateTransition(_state);

    }   

    //TODO - implement Hunting state
    // Update is called once per frame
    void Update()
    {
        /*
        //if player is very close to enemy then it starts hunting
        if((_playerObj.transform.position-transform.position).magnitude < _huntingPlayerDistance)
            StateTransition(EEnemyState.Hunting);
        */
        
        switch (_state)
        {
            case EEnemyState.Patrolling:
                PatrolStateBehavior();
                break;
            
            case EEnemyState.Wandering:
                WanderingStateBehavior();
                break;
            case EEnemyState.SeekingSound_Moving:
                SeekingSoundMovingBehavior();
                break;
            case EEnemyState.SeekingSound_Patrolling:
                SeekingSoundPatrollingBehavior();
                break;
            case EEnemyState.Stunned:
                StunnedStateBehavior();
                break;
            case EEnemyState.Idle:
                break;
            default:
                break;
                
        }
        
        
        
    }


    #region StateFunctions
    public void StateTransition(EEnemyState newState)
    {
        //if stunned, ignore state transition
        if (_state.Equals(EEnemyState.Stunned) && !newState.Equals(EEnemyState.Wandering))
            return;
        
        if (newState.Equals(EEnemyState.Wandering))
        {
            //set random destination
            _navAgent.SetDestination(GetRandomNavMeshPoint(_wanderDistance));
        }
        
        else if (newState.Equals(EEnemyState.SeekingSound_Moving))
        {
            _audioManager.PlayBark();
            _navAgent.SetDestination(_playerObj.transform.position);
        }

        else if (newState.Equals(EEnemyState.Patrolling))
        {
            //if no patrol path then ignore transition
            if(patrolPathNodes.Length == 0)
                StateTransition(EEnemyState.Wandering);
            
            
            //walk along path
            patrolPathIndex = 0;
            _navAgent.SetDestination(patrolPathNodes[patrolPathIndex].position);
        }
        
        else if (newState.Equals(EEnemyState.Stunned))
        {
            _audioManager.PlayPowerDown();
            _stunTimer = stunDuration;
            _navAgent.SetDestination(transform.position);
        }

        _state = newState;
    }

    private void StunnedStateBehavior()
    {
        _stunTimer -= Time.deltaTime;

        if (_stunTimer < 0.0f)
        {
            StateTransition(EEnemyState.Wandering);
            _audioManager.PlayBark();
        }
    }

    private void WanderingStateBehavior()
    {
        //check if close to destination
        if (_navAgent.remainingDistance < _destinationBuffer)
        {
            _audioManager.PlayMovingSound();
            _navAgent.SetDestination(GetRandomNavMeshPoint(_wanderDistance));
        }
    }
    
    private void PatrolStateBehavior()
    {
        //check if close to destination
        if (_navAgent.remainingDistance < _destinationBuffer)
        {
            _audioManager.PlayMovingSound();
            patrolPathIndex++;
            patrolPathIndex %= patrolPathNodes.Length;
            _navAgent.SetDestination(patrolPathNodes[patrolPathIndex].position);
        }
            
    }
    

    #endregion

    #region SeekingSoundStateTree

    private void SeekingSoundMovingBehavior()
    {
        if (_navAgent.remainingDistance < _destinationBuffer)
        {
            _audioManager.PlayMovingSound();
            //create a random patrol around this area
            _dynamicPatrolNodes[0] = (this.transform.position);
            _dynamicPatrolNodes[1] = (GetRandomNavMeshPoint(Random.Range(5.0f,10.0f),_dynamicPatrolNodes[0]));
            _dynamicPatrolNodes[2] = (GetRandomNavMeshPoint(Random.Range(5.0f,10.0f),_dynamicPatrolNodes[1]));
            _dynamicPatrolNodes[3] = (GetRandomNavMeshPoint(Random.Range(5.0f,10.0f),_dynamicPatrolNodes[2]));

            _navAgent.SetDestination(_dynamicPatrolNodes[1]);
            _dynPatrolIndex = 1;
            
            StateTransition(EEnemyState.SeekingSound_Patrolling);
            
        }
    }

    private void SeekingSoundPatrollingBehavior()
    {
        if (_navAgent.remainingDistance < _destinationBuffer)
        {
            _audioManager.PlayMovingSound();

            _dynPatrolIndex++;
            _dynPatrolIndex %= _dynamicPatrolNodes.Length;
            _navAgent.SetDestination(_dynamicPatrolNodes[_dynPatrolIndex]);
        }
    }

    #endregion

    
    
    #region HuntingPlayerStateTree

    

    #endregion

    
    
    
    #region Utilities

    private Vector3 GetRandomNavMeshPoint(float radius)
    {
        Vector3 randDir = Random.insideUnitSphere * radius;
        randDir += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randDir, out hit, radius, 1);
        return hit.position;

    }

    private Vector3 GetRandomNavMeshPoint(float radius, Vector3 offsetPos)
    {
        Vector3 randDir = Random.insideUnitSphere * radius;
        randDir += offsetPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randDir, out hit, radius, 1);
        return hit.position;
    }

    #endregion


}
