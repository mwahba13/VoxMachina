using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EEnemyState
{
    Wandering,          //wandering aimlessly
    SeekingSound,       //moving towards sound it heard within hearing radius
    Patrolling,         //moving along a pre-determined path
}


public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private EEnemyState _state;
    [SerializeField] private float _destinationBuffer;      //how close it needs to be to dest to switch to next dest
    [SerializeField] private float _wanderDistance;
    
    //
    public Transform[] patrolPathNodes;
    private int patrolPathIndex;
    
    
    
    //Nav mesh agent
    private NavMeshAgent _navAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        
        


        
        StateTransition(_state);

    }   

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case EEnemyState.Patrolling:
                PatrolStateBehavior();
                break;
            
            case EEnemyState.Wandering:
                WanderingStateBehavior();
                break;
            
            default:
                break;
                
        }
    }


    #region StateFunctions
    public void StateTransition(EEnemyState newState)
    {
        if (newState.Equals(EEnemyState.Wandering))
        {
            //set random destination
            _navAgent.SetDestination(GetRandomNavMeshPoint(_wanderDistance));
        }

        if (newState.Equals(EEnemyState.Patrolling))
        {
            //if no patrol path then ignore transition
            if(patrolPathNodes.Length == 0)
                StateTransition(EEnemyState.Wandering);
            
            
            //walk along path
            patrolPathIndex = 0;
            _navAgent.SetDestination(patrolPathNodes[patrolPathIndex].position);
        }
    }

    private void WanderingStateBehavior()
    {
        //check if close to destination
        if (_navAgent.remainingDistance < _destinationBuffer)
        {
            _navAgent.SetDestination(GetRandomNavMeshPoint(_wanderDistance));
        }
    }
    
    private void PatrolStateBehavior()
    {
        //check if close to destination
        if (_navAgent.remainingDistance < _destinationBuffer)
        {
            patrolPathIndex++;
            patrolPathIndex %= patrolPathNodes.Length;
            _navAgent.SetDestination(patrolPathNodes[patrolPathIndex].position);
        }
            
    }
    

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

    #endregion


}
