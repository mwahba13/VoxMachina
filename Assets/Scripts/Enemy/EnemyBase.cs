using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int health;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 0)
            OnDeath();
    }

    public void DecrementHealth(int d)
    {
        health -= d;
    }

    private void OnDeath()
    {
        Destroy(this.gameObject);
    }
}
