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
    private bool _isDead = false;
    private static readonly int IsDead = Animator.StringToHash("isDead");

    // Start is called before the first frame update
    void Start()
    {
        GameEventSystem.current.OnTerminalCastSpell += OnTerminalCastSpell;
        _animator = GetComponent<Animator>();
        _audioManager = GetComponent<BossAudioManager>();
    }

    private void OnTerminalCastSpell(ESoundPitch[] list)
    {
        if(CompareList(list) && _health == 3)
            OnTakeDamage();
        
        
        else if(CompareList(list) && _health == 2)
            OnTakeDamage();
        
        
        else if(CompareList(list) && _health == 1)
            OnBossKilled();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _animator.SetBool("isActivated",true);
            
    }

    private void OnTakeDamage()
    {
        Debug.Log("Damage Taken");
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
        
        if(_isDead)
            if(Input.GetKeyDown(KeyCode.Q))
                Application.Quit();
    }

    private bool CompareList(ESoundPitch[] list)
    {
        ESoundPitch[] currentWeakness = _firstWeakness;
        if (_currentSequence == 0)
            currentWeakness = _firstWeakness;
        else if (_currentSequence == 1)
            currentWeakness = _secondWeakness;
        else if (_currentSequence == 2)
            currentWeakness = _thirdWeakness;
        
        for (int i = 0; i < 3; i++)
        {
            if (list[i] != currentWeakness[i])
                return false;
        }

        return true;
    }

    private void OnBossKilled()
    {
        _isDead = true;
        _animator.SetBool("IsDead",true);
        _audioManager.PlayAIDestroyedClip();
    }

    
    
}
