using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{

    public static GameEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action<ESoundPitch[]> OnPlayerCastSpell;

    public void PlayerCastSpell(ESoundPitch[] list)
    {
        if (OnPlayerCastSpell != null)
        {
            OnPlayerCastSpell(list);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
