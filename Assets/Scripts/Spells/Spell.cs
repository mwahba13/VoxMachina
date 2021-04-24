using System;
using UnityEngine;


public class Spell : MonoBehaviour
{

    [SerializeField] private int _damage;
    [SerializeField] private int _speed;

    [SerializeField]private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void AddForce(Vector3 forceDir)
    {
        _rb.AddForce(forceDir*_speed,ForceMode.Impulse);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
            other.gameObject.GetComponent<EnemyBase>().DecrementHealth(_damage);
        }
    }
}
