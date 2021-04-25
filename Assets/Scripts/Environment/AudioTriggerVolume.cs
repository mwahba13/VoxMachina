using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerVolume : MonoBehaviour
{

    private AudioSource _source;
    private Collider _collider;

    private bool _isTriggered;
    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _collider = GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _source.Play();
            _isTriggered = true;
            _collider.enabled = false;
        }
    }

    private void Update()
    {
        if(_isTriggered && !_source.isPlaying)
            Destroy(this.gameObject);
    }   
}
