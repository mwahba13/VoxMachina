using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public Light light;
    
    private bool _isPlayerNear = false;

    private AudioSource _source;
    private TerminalFace _face;
    private Collider _collider;
    
    // Start is called before the first frame update
    void Start()
    {
        GameEventSystem.current.OnPlayerCastSpell += OnPlayerCastSpell;
        light.enabled = false;
        _collider = GetComponent<Collider>();
        _source = GetComponent<AudioSource>();
        _face = GetComponentInChildren<TerminalFace>();
    }

    private void OnPlayerCastSpell(ESoundPitch[] list,bool inSilentZone)
    {

        if (_isPlayerNear)
        {
            GameEventSystem.current.TerminalCastSpell(list);
            _source.Play();

            _collider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            light.enabled = true;
            _face._isPlayerInZone = true;
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            light.enabled = false;
            _face._isPlayerInZone = false;
            _isPlayerNear = false;

        }
    }


    private void OnDestroy()
    {
        GameEventSystem.current.OnPlayerCastSpell -= OnPlayerCastSpell; 
    }
}
