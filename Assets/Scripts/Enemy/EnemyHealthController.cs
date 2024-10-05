using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public event Action OnEnemyDie;
    
    [SerializeField] private Collider2D _collider;
    
    [SerializeField, ReadonlyField] private int _health;

    public void Init(int health)
    {
        _health = health;
    }
    
    public void GetDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            OnEnemyDie?.Invoke();
            _collider.enabled = false;
        }
    }
}
