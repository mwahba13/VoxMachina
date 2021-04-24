using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceDoor : MonoBehaviour
{
    [SerializeField] private ESoundPitch[] _voiceKey = new ESoundPitch[3];


    private Animator _animator;
    
    private bool _playerIsNear;
    private bool _isDoorOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        GameEventSystem.current.OnPlayerCastSpell += OnPlayerCastSpell;

        _animator = GetComponentInParent<Animator>();
    }



    private void OnPlayerCastSpell(ESoundPitch[] list)
    {
        
        if (_playerIsNear && IsKeyMatched(list))
        {
            _animator.SetTrigger("OpenDoor");
            _isDoorOpen = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = false;

            if (_isDoorOpen)
            {
                _animator.ResetTrigger("OpenDoor");
                _animator.SetTrigger("CloseDoor");
            }
        }
    }


    private bool IsKeyMatched(ESoundPitch[] list)
    {
        return (list[0] == _voiceKey[0] && list[1] == _voiceKey[1] && list[2] == _voiceKey[2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
