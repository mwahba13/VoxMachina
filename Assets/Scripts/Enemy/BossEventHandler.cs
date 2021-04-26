using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BossEventHandler : MonoBehaviour
{

    private Animator _animator;
    private BossAudioManager _audioManager;

    private int _currentSequence;
    [SerializeField] private ESoundPitch[] _firstWeakness;
    [SerializeField] private ESoundPitch[] _secondWeakness;
    [SerializeField] private ESoundPitch[] _thirdWeakness;
    private int _health = 3;
    private static readonly int IsDead = Animator.StringToHash("isDead");

    // Start is called before the first frame update
    void Start()
    {
        GameEventSystem.current.OnTerminalCastSpell += OnTerminalCastSpell;
    }

    private void OnTerminalCastSpell(ESoundPitch[] list)
    {
        if(list == _firstWeakness && _health == 3)
            OnTakeDamage();
        
        
        else if(list == _secondWeakness && _health == 2)
            OnTakeDamage();
        
        
        else if(list == _thirdWeakness && _health == 1)
            OnBossKilled();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _animator.SetBool("isActivated",true);
            
    }

    private void OnTakeDamage()
    {
        _audioManager.PlayAIHitClip();

        _currentSequence++;
        _animator.SetInteger("currentSequence",_currentSequence);
    }

    private void OnDestroy()
    {
        GameEventSystem.current.OnTerminalCastSpell -= OnTerminalCastSpell;
    }

    // Update is called once per frame
    void Update()
    {
        if(_health == 0)
            OnBossKilled();
    }

    private void OnBossKilled()
    {
        _animator.SetBool("IsDead",true);
        _audioManager.PlayAIDestroyedClip();
    }

    
    
}
