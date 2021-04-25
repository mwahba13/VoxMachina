using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{

    private AudioSource _source;

    public AudioClip hitSound;
    public AudioClip successSound;
    public AudioClip playerhitSound;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayTargetHitClip(){ _source.PlayOneShot(hitSound);}
    
    public void PlayTargetSuccessClip(){_source.PlayOneShot(successSound);}
    
    public void PlayPlayerHitClip() {_source.PlayOneShot(playerhitSound);}
    
}
