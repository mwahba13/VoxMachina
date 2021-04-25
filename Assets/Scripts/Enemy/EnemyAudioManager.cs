using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{


    public AudioClip enemyPowerDown;
    public AudioClip enemyHit1;
    public AudioClip enemyHit2;

    public AudioClip smallEnemyBark;
    public AudioClip medEnemyBark;
    public AudioClip largeEnemyBark;
    
    public AudioClip smallEnemyMoving;
    public AudioClip medEnemyMoving;
    public AudioClip largeEnemyMoving;


    private AudioSource _source;
    private EEnemyType _type;
    
    // Start is called before the first frame update
    void Start()
    {
        _type = GetComponentInChildren<EnemyEventHandler>()._type;
        
        _source = GetComponent<AudioSource>();
    }


    public void PlayPowerDown()
    {
        _source.PlayOneShot(enemyPowerDown);
    }

    public void PlayBark()
    {

        switch (_type)
        {
            case EEnemyType.Small:
                _source.PlayOneShot(smallEnemyBark);
                break;
            case EEnemyType.Med:
                _source.PlayOneShot(medEnemyBark);
                break;
            case EEnemyType.Large:
                _source.PlayOneShot(largeEnemyBark);
                break;
            default:
                break;
        }

    }

    public void PlayMovingSound()
    {

        
        switch (_type)
        {
            case EEnemyType.Small:
                _source.PlayOneShot(smallEnemyMoving);
                break;
            case EEnemyType.Med:
                _source.PlayOneShot(medEnemyMoving);
                break;
            case EEnemyType.Large:
                _source.PlayOneShot(largeEnemyMoving);
                break;
            default:
                break;
        }

        
    }

    public void StopSounds()
    {
        _source.Stop();
    }
    
}
