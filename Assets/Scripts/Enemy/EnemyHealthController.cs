using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public event Action OnEnemyDie;
    
    [SerializeField, Range(1, 5)] private int _health = 2;
    [SerializeField] private Collider2D _collider;
    
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
