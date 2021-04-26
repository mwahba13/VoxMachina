using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioManager : MonoBehaviour
{

    private AudioSource _source;

    public AudioClip aiHitClip;
    public AudioClip aiDestroyedClip;
    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }
    
    public void PlayAIHitClip() {_source.PlayOneShot(aiHitClip);}

    public  void PlayAIDestroyedClip(){_source.PlayOneShot(aiDestroyedClip);}
    // Update is called once per frame
    void Update()
    {
        
    }
}
